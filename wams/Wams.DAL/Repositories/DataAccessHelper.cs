using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DAL.Repositories
{
    using System.Data;

    public static class DataAccessHelper
    {
        public static bool IsNull(IDataRecord row, string key)
        {
            return row[key] is DBNull || row[key] == null;
        }

        public static int ToInt(IDataRecord row, string column)
        {
            return Convert.ToInt32(row[column]);
        }

        public static DateTime ToDateTime(IDataRecord row, string column)
        {
            return Convert.ToDateTime(row[column]);
        }

        public static string ToStr(IDataRecord row, string column)
        {
            return IsNull(row, column) ? null : (string)row[column];
        }
    }
}
