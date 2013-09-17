using System.ServiceProcess;
using AxBatchRunner.AxWrapper;
using AxBatchRunner.WindowsService.Properties;

namespace AxBatchRunner.WindowsService
{
    public partial class AxBatchRunnerService : ServiceBase
    {
        private readonly AxBatchScheduler _scheduler;

        public AxBatchRunnerService()
        {
            InitializeComponent();
            Settings settings = new Settings();
            AxSettings axSettings = new AxSettings
                                        {
                                            BatchGroup = settings.BatchGroup,
                                            BatchRunnerClass = settings.BatchRunnerClass,
                                            BatchRunnerMethod = settings.BatchRunnerMethod,
                                            CancelJobIfError = settings.CancelJobIfError,
                                            Company = settings.Company,
                                            ComPlusAppl = settings.ComPlusAppl,
                                            Configuration = settings.Configuration,
                                            DaxVersion = settings.DAXVersion,
                                            Language = settings.Language,
                                            Password = settings.Password,
                                            TimerInterval = settings.TimerInterval,
                                            User = settings.User
                                        };

            _scheduler = new AxBatchScheduler(axSettings);
        }

        protected override void OnStart(string[] args)
        {
            _scheduler.Start();
        }

        protected override void OnStop()
        {
            _scheduler.Stop();
        }
    }
}