﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects.Accounts
{
    public class MemberDues
    {
        public int DuesId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public decimal Amount { get; set; }

        public string DuesMonth { get; set; }

        public int DuesYear { get; set; }

        public string AddedDate { get; set; }

        public string AddedBy { get; set; }

        public int AddedById { get; set; }
    }
}
