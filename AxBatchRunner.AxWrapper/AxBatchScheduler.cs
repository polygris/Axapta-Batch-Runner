using System;
using System.Timers;
using NLog;

namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   This class schedule RunBaseBatch classes
    /// </summary>
    /// TODO: В этом классе должна быть реализована простая логика запуска батчей
    /// Всю логику работы с коннектром - подключен отключен переложить на AxProxy
    /// В AxProxy реализовать getter говорящий о том что AxProxy готов выполнять запросы к Аксу
    /// по идее может быть такого вида AllOk { return IsConnected && !IsShutdown}
    public class AxBatchScheduler
    {
        public delegate string StartBatchDelegate();

        private AxProxy _proxy;
        private readonly AxSettings _settings;
        private Timer _timer;
        private readonly StartBatchDelegate _delegateBatchDelegate;
        private readonly ThreadSafeFlag _isRunning = new ThreadSafeFlag();
        private readonly ThreadSafeFlag _isShutdownProcess = new ThreadSafeFlag();

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AxBatchScheduler(AxSettings settings)
        {
            _isRunning.Clear();
            _isShutdownProcess.Clear();

            _settings = settings;
            _proxy = new AxProxy(_settings);

            _timer = new Timer {Interval = _settings.TimerInterval*1000};
            _timer.Elapsed += timer_Elapsed;
            _delegateBatchDelegate = StartBatchFacade;
        }

        /// <summary>
        ///   RunBaseBatch is running
        /// </summary>
        private bool IsRunning
        {
            get { return _isRunning.Value; }
        }

        /// <summary>
        ///   System is now shutdown
        /// </summary>
        public bool IsShutdownProcess
        {
            get { return _isShutdownProcess.Value; }
        }

        public AxProxy Proxy
        {
            get { return _proxy; }
        }

        /// <summary>
        ///   Run derived class of RunBaseBatch
        /// </summary>
        private string StartBatchFacade()
        {
            return _proxy.StartBatch(_settings.BatchGroup, _settings.CancelJobIfError);
        }

        /// <summary>
        ///   APM callback method
        /// </summary>
        /// <param name = "result">Result of BeginInvoke</param>
        private void BatchIsDone(IAsyncResult result)
        {
            StartBatchDelegate batchDelegate = (StartBatchDelegate) result.AsyncState;
            _isRunning.Clear();

            if (batchDelegate != null)
            {
                string message = string.Empty;
                try
                {
                    message = batchDelegate.EndInvoke(result);
                }
                catch (AxException exception)
                {
                    _logger.Error(string.Format("Error connecting to Axapta Business Connector. Message: {0}",
                                                exception.Message));
                }
                catch (Exception exception)
                {
                    _logger.ErrorException("Throw exception", exception);
                }

                if (string.IsNullOrEmpty(message))
                {
                    _logger.Info("There are no batches to processing.");
                }
                else
                {
                    _logger.Info("Batch has completed!");
                    _logger.Info(message);
                }
            }
        }

        /// <summary>
        ///   This method run, when time is elapsed
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _logger.Info("Time elapsed.");
            _timer.Stop();

            try
            {
                if (!_proxy.IsConnected)
                {
                    _logger.Info("Trying reconnect to Axapta...");
                    _proxy.Logon();
                    return;
                }

                if (IsShutdownProcess)
                {
                    _logger.Warn("Can't run batch, because system in shutdown process.");
                    return;
                }

                if (IsRunning)
                {
                    _logger.Warn("Batch didn't start, because another batch is running.");
                    return;
                }

                if (!_proxy.HasBatches(_settings.BatchGroup))
                {
                    _logger.Info("There are no batches to processing.");
                    return;
                }

                _isRunning.Set();
                _delegateBatchDelegate.BeginInvoke(BatchIsDone, _delegateBatchDelegate);
                _logger.Info("Batch was started.");
            }
            catch (AxException exception)
            {
                _logger.Error(string.Format("Error connecting to the Axapta Business Connector. Message: {0}",
                                            exception.Message));
            }
            finally
            {
                _timer.Start();
            }
        }

        /// <summary>
        ///   Start Scheduler
        /// </summary>
        public void Start()
        {
            _logger.Info("Starting service...");
            _proxy.Logon();
            _timer.Start();
        }

        /// <summary>
        ///   Start Scheduler
        /// </summary>
        public void Stop()
        {
            _logger.Warn("Shutdown service...");
            _isShutdownProcess.Set();
            _timer.Elapsed -= timer_Elapsed;
            _timer.Stop();
            _timer.Enabled = false;
            _timer = null;

            _proxy.Logoff();
            _proxy = null;
        }
    }
}