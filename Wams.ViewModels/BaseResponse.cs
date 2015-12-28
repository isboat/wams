using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels
{
    public class BaseResponse
    {
        public string Message { get; set; }

        public Status Status { get; set; }
    }

    public enum Status
    {
        Success = 0,

        Fail = 1,

        Pending = 2,

        Unknown = 4
    }
}
