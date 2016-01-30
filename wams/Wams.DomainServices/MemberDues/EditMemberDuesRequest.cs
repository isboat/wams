using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wams.ViewModels.MemberDues
{
    public class EditMemberDuesRequest : AddMemberDuesRequest
    {
        public int DuesId { get; set; }
    }
}
