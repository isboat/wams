using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wams.ViewModels.MemberDues
{
    public class AddMemberDuesRequest
    {
        public int MemberId { get; set; }

        public string MemberFullName { get; set; }

        [DisplayName("Amount")]
        [Required]
        public decimal Amount { get; set; }

        [DisplayName("Dues Month")]
        [Required]
        public string DueMonth { get; set; }

        public IEnumerable<SelectListItem> DueMonthOptions { get; set; }

        [DisplayName("Dues year")]
        [Required]
        public int DueYear { get; set; }

        public IEnumerable<SelectListItem> DueYearOptions { get; set; }

        public string AddedBy { get; set; }

        public int AddedById { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
