using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Wams.Enums;

namespace Wams.DataObjects
{
    [DataContract]
    public class Source
    {
        /// <summary>
        /// Checksum containing app Id and timestamp
        /// </summary>
        [DataMember]
        public string Checksum { get; set; }

        [DataMember]
        public string TimeStamp { get; set; }

        ///// <summary>
        ///// Name of app server making request
        ///// </summary>
        //[DataMember(IsRequired = false)]
        //public string ApplicationServerName { get; set; }

        ///// <summary>
        ///// IP address of web user
        ///// </summary>
        //[DataMember(IsRequired = true)]
        //public string ClientIpAddress { get; set; }

        ///// <summary>
        ///// Browser user agent string
        ///// </summary>
        //[DataMember(IsRequired = true)]
        //public string ClientUserAgent { get; set; }

        ///// <summary>
        ///// Gets or sets the type of the source.
        ///// </summary>
        ///// <value>
        ///// The type of the source.
        ///// </value>
        //[DataMember(IsRequired = true)]
        //public SourceType SourceType { get; set; }

        ///// <summary>
        ///// Web device name
        ///// </summary>
        //[DataMember(IsRequired = false)]
        //public string WebDeviceName { get; set; }

        ///// <summary>
        ///// Name of web server making request
        ///// </summary>
        //[DataMember(IsRequired = false)]
        //public string WebServerName { get; set; }

        ///// <summary>
        ///// Web session id
        ///// </summary>
        //[DataMember(IsRequired = true)]
        //public string WebSessionKey { get; set; }

        ///// <summary>
        ///// Current web site id
        ///// </summary>
        //[DataMember(IsRequired = true)]
        //public string WebsiteKey { get; set; }

        ///// <summary>The to string.</summary>
        ///// <returns>The <see cref="string"/>.</returns>
        //public override string ToString()
        //{
        //    return
        //        string.Format(
        //            "[SourceType='{0}' ClientIpAddress='{1}' ClientUserAgent='{2}' ApplicationServerName='{3}' WebServerName='{4}' WebsiteKey='{5}' WebSessionKey='{6}' WebDeviceName='{7}']",
        //            this.SourceType,
        //            this.ClientIpAddress,
        //            this.ClientUserAgent,
        //            this.ApplicationServerName,
        //            this.WebServerName,
        //            this.WebsiteKey,
        //            this.WebSessionKey,
        //            this.WebDeviceName);
        //}
    }
}
