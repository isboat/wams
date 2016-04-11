using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.ViewModels.Accounting;

namespace Wams.Interfaces
{
    public interface IAccounting
    {
        TotalMonthlyDues TotalMonthlyDues(int year);
    }
}
