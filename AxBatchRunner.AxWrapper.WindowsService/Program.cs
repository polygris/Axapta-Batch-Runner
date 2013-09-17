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
            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new AxBatchRunnerService() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
