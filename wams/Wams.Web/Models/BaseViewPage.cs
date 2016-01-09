using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wams.Web.Models
{
    using System.Web.Mvc;

    using Wams.BusinessLogic.AuthenticationModels;

    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new CustomPrincipal User
        {
            get { return base.User as CustomPrincipal; }
        }
    }
}