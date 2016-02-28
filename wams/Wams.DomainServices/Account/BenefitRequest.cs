using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wams.ViewModels.Account
{
    public class BenefitRequest
    {
        public int BenefitId { get; set; }

        [DisplayName("Benefit type")]
        [Required]
        public string BenefitType { get; set; }

        public IEnumerable<SelectListItem> BenefitTypeOptions { get; set; }

        [DisplayName("Request date")]
        [Required]
        public string BenefitDate { get; set; }

        [Required]
        public string Message { get; set; }

        public int MemberId { get; set; }

        public string Address { get; set; }

        public string MemberName { get; set; }

        [DisplayName("Tick to Grant")]
        public bool Granted { get; set; }
    }
}
