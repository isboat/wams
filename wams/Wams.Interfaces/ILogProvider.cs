using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Interfaces
{
    public interface ILogProvider
    {
        void Error(string message);
        void Error(string message, Exception exception);

        void Info(string message);
    }
}
