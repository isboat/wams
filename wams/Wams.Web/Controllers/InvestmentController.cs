using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wams.Web.Controllers
{
    public class InvestmentController : BaseController
    {
        //
        // GET: /Investment/
        public ActionResult ViewDues()
        {
            return View();
        }

        //
        public ActionResult RequestTransfer(int id)
        {
            return View();
        }

        //
        // GET: /Investment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
