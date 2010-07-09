using System.ServiceProcess;

namespace AxBatchRunner.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new AxBatchRunnerService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
