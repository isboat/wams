using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DAL.Repositories
{
    using System.Configuration;

    public class BaseRepository
    {
        protected readonly string ConString;

        public BaseRepository()
        {
            this.ConString = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
        }
    }
}
