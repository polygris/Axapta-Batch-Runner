using System;
using System.Runtime.InteropServices;
using AxBatchRunner.AxWrapper;

namespace AxBatchRunner.ConsoleRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AxSettings axSettings3 = new AxSettings();
            axSettings3.DaxVersion = 3;
            axSettings3.User = "rsi";
            axSettings3.Password = "rsi";
            axSettings3.Company = "ais";
            axSettings3.Language = "ru";
            axSettings3.ComPlusAppl = "Navision Axapta Business Connector";
            axSettings3.Configuration = "ZCH_DEV_3T";
            axSettings3.BatchGroup = "PR2";
            axSettings3.CancelJobIfError = false;
            axSettings3.TimerInterval = 10;
            axSettings3.DelBatchAfterSuccess = true;
            axSettings3.ValidateSettings();

            AxSettings axSettings4 = new AxSettings();
            //axSettings4.ObjectServer = "DAX40@AX4:2714";
            //axSettings4.ObjectServer = "DAX40@AX4:2714";
            axSettings4.Configuration = "DAX40";
            axSettings4.DaxVersion = 4;
            axSettings4.BatchGroup = "PR2";
            axSettings4.CancelJobIfError = false;
            axSettings4.TimerInterval = 10;

            //axSettings4.ValidateSettings();

            AxBatchScheduler scheduler = new AxBatchScheduler(axSettings3);
            scheduler.Start();

            //AxConfig config = scheduler.Proxy.GetConfig();
            //Console.WriteLine(config);

            Console.ReadLine();
            scheduler.Stop();
        }
    }
}