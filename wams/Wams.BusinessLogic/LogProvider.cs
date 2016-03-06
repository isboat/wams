using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.Interfaces;

namespace Wams.BusinessLogic
{
    public class LogProvider : ILogProvider
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("default");

        public LogProvider()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Error(string error)
        {
            log.Error(error);
        }


        public void Error(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Info(string message)
        {
            log.Info(message);
        }
    }
}
