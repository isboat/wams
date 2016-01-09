using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DomainObjects
{
    public class BaseResponse
    {
        public string Message { get; set; }

        public bool Success { get; set; }
    }
}
