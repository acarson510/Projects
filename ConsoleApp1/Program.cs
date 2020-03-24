using Bayada.Common.Logging;
using log4net;
using System;
using System.IO;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            LogHelper.LogInfo("test");

            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            //log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("App.config"));
            //log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

            //log.Info("Andrew Test log file");
        }
    }
}
