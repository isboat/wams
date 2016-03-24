using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels.Admin
{
    public class EditAdminRequest : RegisterAdminRequest
    {
        public int Id { get; set; }
    }
}
