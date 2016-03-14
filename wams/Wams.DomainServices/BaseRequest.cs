using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels
{
    public class BaseRequest
    {
        public int MemberId { get; set; }

        public string Address { get; set; }

        [DisplayName("Request date")]
        public string RequestDate { get; set; }

        public string MemberName { get; set; }

        [DisplayName("Tick to Grant")]
        public bool Granted { get; set; }

        [DisplayName("Amount")]
        [Required]
        public decimal Amount { get; set; }
    }
}
