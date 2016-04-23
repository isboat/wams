using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wams.Common.IoC;
using Wams.Interfaces;
using Wams.Web.Models;

namespace Wams.Web.Controllers
{
    public class AccountingController : BaseController
    {
        #region Instances Variables

        private readonly IAccounting accounting = IoC.Instance.Resolve<IAccounting>();

        #endregion

        public ActionResult TotalMonthlyDues()
        {
            return View();
        }

        public ActionResult GetTotalMonthlyDues(int year)
        {
            var data = this.accounting.TotalMonthlyDues(year);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Investment

        public ActionResult InvestmentView()
        {
            return View();
        }

        public ActionResult GetInvestmentData(int year)
        {
            var result = this.accounting.InvestmentData(year);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        // POST: Accounting/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounting/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Accounting/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Accounting/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
