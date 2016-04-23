using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wams.ViewModels.MemberInvmt
{
    public class AddMemberInvmtRequest
    {
        public int MemberId { get; set; }

        public string MemberFullName { get; set; }

        [DisplayName("Amount")]
        [Required]
        public decimal Amount { get; set; }

        [DisplayName("Month")]
        [Required]
        public string InvmtMonth { get; set; }

        public IEnumerable<SelectListItem> InvmtMonthOptions { get; set; }

        [DisplayName("Year")]
        [Required]
        public int InvmtYear { get; set; }

        public IEnumerable<SelectListItem> InvmtYearOptions { get; set; }

        public string AddedBy { get; set; }

        public int AddedById { get; set; }

        public string AddedDate { get; set; }
    }
}
