using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels.MemberInvmt
{
    public class EditMemberInvmtRequest : AddMemberInvmtRequest
    {
        public int InvmtId { get; set; }
    }
}
