using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Common.Helpers
{
    using Wams.Common.Configuration;

    public class UtilityHelper
    {
        public static string GetAppId(string checksum, string httpRequestTime, out bool sameAsReqTime)
        {
            if (!string.IsNullOrEmpty(checksum))
            {
                var appReqStr = checksum;// EncryptionHelper.Decrypt(checksum, "abc123");

                var split = appReqStr.Split('=');
                if (split.Length == 2)
                {
                    // http time format: "Fri, 27 Feb 2009 03:11:21 GMT";
                    sameAsReqTime = DateTime.Parse(split[1]) == DateTime.Parse(httpRequestTime);
                    return split[0];
                }
            }

            sameAsReqTime = false;
            return null;
        }

        public static string CreateChecksum(string appKey)
        {
            return null;
        }
    }
}
