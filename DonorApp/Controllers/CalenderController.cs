
using DonorApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DonorApp.Controllers
{
    [Authorize]
    public class CalenderController : Controller
    {
        private readonly SangamDBContext _context = new SangamDBContext();

        // GET: Calender
        [HttpPost]
        public ActionResult GetMonthInformation(DateTime date)
        {

            var retVal = Calender.Data.Calender.GetCalendarDataPerCityAndYear(date.Day, date.Month, date.Year, "Hyderabad-AP-India", Calender.Data.TimeZoneValues.India);
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDonarCalenderData(DateTime date, string masam, string tithi, int satramId)
        {
            string satram = string.Empty;
            if (satramId != 0)
            {
                if (satramId == 1)
                {
                    satram = "వారణాసి";
                }
                else if (satramId == 2)
                {
                    satram = "శ్రీశైలం";
                }
                else if (satramId == 3)
                {
                    satram = "భద్రాచలం";
                }
                else
                {
                    satram = "షిర్డీ";
                }
            }
            //var dbDonarData = _context.DonarDetails.Where(x=>x.Satram==satram).AsQueryable().AsNoTracking().ToList();
            var dbThidiData = _context.ThidiMasters.Where(x => x.Thidi_Paksha_EnglishName == tithi).AsNoTracking().FirstOrDefault();
            var dbMonthData = _context.MonthMasters.Where(x => x.EnglishMonth == masam).AsNoTracking().FirstOrDefault();
            //var donarsByDate = dbDonarData.Where(x => x.DonationDate.HasValue && x.DonationDate.Value.ToString("dd-mm") == date.ToString("dd-mm")).ToList();
            var donarsByDate= _context.DonarDetails.SqlQuery("select * from DonarDetail  where DonationDate is not null and Satram=N'"+satram+"'  and DAY(DonationDate)='"+date.Day+"' and MONTH(DonationDate)='"+date.Month+"'").ToList();
            var donarByTeluguCalender = new List<DonarDetail>();
            if (dbThidiData != null && dbMonthData != null)
            {
                var teluguCalenderVal = dbMonthData.TeluguMonth + " " + dbThidiData.Thidi_Paksha_TeluguName;
                donarByTeluguCalender = _context.DonarDetails.SqlQuery("select * from DonarDetail where Satram=N'" + satram + "' and Thidi Like N'" + teluguCalenderVal + "%'").ToList();
            }

            var retDonarDetails = donarByTeluguCalender.Union(donarsByDate).Select(x => new DonarGridModel()
            {
                DonationFor = x.DonationFor,
                DonationBy = x.DonationBy,
                Relation = x.Relation,
                Address = x.Address,
                DonationDate = x.DonationDate.HasValue ? x.DonationDate.Value.ToString("MMMM-dd") : string.Empty,
                Amount = GetFormatedAmount(x.Amount),
                Purpose = x.Purpose,
                Thidi = string.IsNullOrEmpty(x.Thidi) ? " " : x.Thidi,
                EmailId = x.EmailId
            }).ToList();

            foreach(var s in retDonarDetails)
            {
                string a = ""+s.DonationDate+"";
                string b= "" + satram + "";
                string c = "" + tithi + "";
                string d=""+s.EmailId+"";

                s.EditInfo =String.Format("<i class='fa fa-print' aria-hidden='true' style='cursor:pointer' title='Print Donar Details' onclick='PrintModal(\"{0}\",\"{1}\",\"{2}\",\"{3}\")'></i>", a,b,c,d);
            }
            var jsonData = new { data = from r in retDonarDetails select r };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        private string GetFormatedAmount(string Amount)
        {
            string retVal = string.Empty;
            double ammountDouble = Convert.ToDouble(Amount);
            CultureInfo cultureInfo = new CultureInfo("en-IN");
            string ammountString = string.Format(cultureInfo, "{0:C}", ammountDouble);
            retVal = ammountString;
            return retVal;
        }
    }
}