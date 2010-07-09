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
            AxSettings axSettings = new AxSettings();
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