using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Common.Logging
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
#if !DEBUG
            log.Error(error);
#endif
        }


        public void Error(string message, Exception exception)
        {
#if !DEBUG
            log.Error(message, exception);
#endif
        }

        public void Info(string message)
        {
#if !DEBUG
            log.Info(message);
#endif
        }
    }
}
