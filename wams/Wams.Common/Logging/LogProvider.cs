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
            var str = string.Format("{0} {1}", message, ToStr(exception));
            log.Error(str);
#endif
        }

        public void Info(string message)
        {
#if !DEBUG
            log.Info(message);
#endif
        }

        private string ToStr(Exception ex)
        {
            var str = string.Format("{0} {1}", ex, ex.Message);
            if (ex.InnerException != null)
            {
                str += ". InnerException: " + ToStr(ex.InnerException);
            }
            return str;
        }
    }
}
