using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects.Accounts;

namespace Wams.DAL.Interfaces
{
    public interface IAccountingRepository
    {
        List<MemberDues> GetAll_AnnualDues(int year);

        List<MemberInvmt> GetAll_Investments(int year);
    }
}
