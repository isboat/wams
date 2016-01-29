﻿using System;using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wams.Web.Models
{
    public class UIHelper
    {
        public static IEnumerable<SelectListItem> GetMonthOptions()
        {
            var months = new List<SelectListItem>();

            var curMonth = DateTime.Now.Month;

            for (var i = 0; i < 12; i++)
            {
                var x = curMonth + i;
                
                if (x > 12) x = x - 12;

                var date = new DateTime(DateTime.Now.Year, x, DateTime.Now.Day);
                months.Add(new SelectListItem{ Text = date.ToString("MMM"), Value = date.ToString("MMM"), Selected = x == curMonth});
            }

            return new SelectList(months, "Value", "Text");
        }

        public static IEnumerable<SelectListItem> GetYearOptions()
        {
            var years = new List<SelectListItem>();

            var currentYear = DateTime.Now.Year;
            years.Add(new SelectListItem { Text = (currentYear -1).ToString(), Value = (currentYear - 1).ToString() });
            years.Add(new SelectListItem { Selected = true, Text = (currentYear).ToString(), Value = (currentYear).ToString() });
            
            return new SelectList(years, "Value", "Text");
        }
    }
}