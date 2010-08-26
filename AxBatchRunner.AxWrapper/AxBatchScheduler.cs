using System;
using System.Timers;
using NLog;

namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   This class schedule RunBaseBatch classes
    /// </summary>
    public sealed class AxBatchScheduler
    {
        public delegate string StartBatchDelegate();

        private AxProxy _proxy;
        private readonly AxSettings _settings;
        private Timer _timer;
        private readonly StartBatchDelegate _delegateBatchDelegate;
        private readonly ThreadSafeFlag _isRunning = new ThreadSafeFlag();
        private readonly ThreadSafeFlag _isShutdownProcess = new ThreadSafeFlag();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            return _proxy.StartBatch(_settings.BatchGroup, _settings.CancelJobIfError, _settings.DelBatchAfterSuccess);
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
                    Logger.Error(string.Format("Error connecting to Axapta Business Connector. Message: {0}",
                                                exception.Message));
                }
                catch (Exception exception)
                {
                    Logger.ErrorException("Throw exception", exception);
                }

                if (string.IsNullOrEmpty(message))
                {
                    Logger.Info("There are no batches to processing.");
                }
                else
                {
                    Logger.Info("Batch has completed!");
                    Logger.Info(message);
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
            Logger.Info("Time elapsed.");
            _timer.Stop();

            try
            {
                if (!_proxy.IsConnected)
                {
                    Logger.Info("Trying reconnect to Axapta...");
                    _proxy.Logon();
                    return;
                }

                if (IsShutdownProcess)
                {
                    Logger.Warn("Can't run batch, because system in shutdown process.");
                    return;
                }

                if (IsRunning)
                {
                    Logger.Warn("Batch didn't start, because another batch is running.");
                    return;
                }

                if (!_proxy.HasBatches(_settings.BatchGroup))
                {
                    Logger.Info("There are no batches to processing.");
                    return;
                }

                _isRunning.Set();
                _delegateBatchDelegate.BeginInvoke(BatchIsDone, _delegateBatchDelegate);
                Logger.Info("Batch was started.");
            }
            catch (AxException exception)
            {
                Logger.Error(string.Format("Error connecting to the Axapta Business Connector. Message: {0}",
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
            Logger.Info("Starting service...");
            _proxy.Logon();
            _timer.Start();
        }

        /// <summary>
        ///   Start Scheduler
        /// </summary>
        public void Stop()
        {
            Logger.Warn("Shutdown service...");
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