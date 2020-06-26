using DonorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Globalization;

namespace DonorApp.Controllers
{
    [Authorize]
    public class DataLoadController : Controller
    {
        private readonly SangamDBContext _context = new SangamDBContext();
        // GET: DataLoad
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadThidiMaster(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedThidiFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                    file.SaveAs(path);

                    

                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    Excel.Range range;

                    string str;
                    int rCnt;
                    int cCnt;
                    int rw = 0;
                    int cl = 0;

                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    range = xlWorkSheet.UsedRange;
                    rw = range.Rows.Count;
                    cl = range.Columns.Count;

                    List<ThidiMaster> thidiMasters = new List<ThidiMaster>();
                    for (rCnt = 1; rCnt <= rw; rCnt++)
                    {
                        if (rCnt > 1)
                        {
                            ThidiMaster t = new ThidiMaster();
                            t.Thidi_Paksha_TeluguName = (string)(range.Cells[rCnt, 1] as Excel.Range).Value2;
                            t.Thidi_Paksha_EnglishName = (string)(range.Cells[rCnt, 2] as Excel.Range).Value2;
                            t.Thidi_Telugu = (string)(range.Cells[rCnt, 3] as Excel.Range).Value2;
                            t.Paksha_Telugu = (string)(range.Cells[rCnt, 4] as Excel.Range).Value2;
                            t.Thidi_English = (string)(range.Cells[rCnt, 5] as Excel.Range).Value2;
                            t.Paksha_English = (string)(range.Cells[rCnt, 6] as Excel.Range).Value2;
                            thidiMasters.Add(t);
                        }
                    }

                    xlWorkBook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);

                    _context.ThidiMasters.AddRange(thidiMasters);
                    _context.SaveChanges();
                }
            }
            return View("Index");
        }


        public ActionResult LoadMonthMaster(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedMonthFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                    file.SaveAs(path);



                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    Excel.Range range;

                    string str;
                    int rCnt;
                    int cCnt;
                    int rw = 0;
                    int cl = 0;

                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    range = xlWorkSheet.UsedRange;
                    rw = range.Rows.Count;
                    cl = range.Columns.Count;

                    List<MonthMaster> monthMasters = new List<MonthMaster>();
                    for (rCnt = 1; rCnt <= rw; rCnt++)
                    {
                        if (rCnt > 1)
                        {
                            MonthMaster t = new MonthMaster();
                            t.TeluguMonth = (string)(range.Cells[rCnt, 1] as Excel.Range).Value2;
                            t.EnglishMonth = (string)(range.Cells[rCnt, 2] as Excel.Range).Value2;
                            monthMasters.Add(t);
                        }
                    }

                    xlWorkBook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);

                    _context.MonthMasters.AddRange(monthMasters);
                    _context.SaveChanges();
                }
            }
            return View("Index");
        }

        public ActionResult LoadDonarDetails(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedDonarDetailsFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                    file.SaveAs(path);



                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    Excel.Range range;

                    string str;
                    int rCnt;
                    int cCnt;
                    int rw = 0;
                    int cl = 0;

                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    range = xlWorkSheet.UsedRange;
                    rw = range.Rows.Count;
                    cl = range.Columns.Count;

                    List<DonarDetail> donarDetails = new List<DonarDetail>();
                    for (rCnt = 1; rCnt <= rw; rCnt++)
                    {
                        if (rCnt > 1)
                        {
                            try
                            {
                                DonarDetail t = new DonarDetail();
                                t.Satram = Convert.ToString((range.Cells[rCnt, 1] as Excel.Range).Value2);
                                t.DonationFor = (string)(range.Cells[rCnt, 2] as Excel.Range).Value2;
                                t.Purpose = Convert.ToString((range.Cells[rCnt, 3] as Excel.Range).Value2);
                                t.DonationBy = (string)(range.Cells[rCnt, 4] as Excel.Range).Value2;
                                t.Relation = (string)(range.Cells[rCnt, 5] as Excel.Range).Value2;
                                t.Gothram = (string)(range.Cells[rCnt, 6] as Excel.Range).Value2;
                                t.Address = Convert.ToString((range.Cells[rCnt, 7] as Excel.Range).Value2);
                                t.Thidi = (string)(range.Cells[rCnt, 8] as Excel.Range).Value2;
                                t.DonationDateTelugu = Convert.ToString((range.Cells[rCnt, 9] as Excel.Range).Value2);
                                DateTime cellDate = DateTime.FromOADate(Convert.ToDouble((range.Cells[rCnt, 10] as Excel.Range).Value2));
                                if (cellDate.Year <= DateTime.Now.Year && cellDate.Year > (DateTime.Now.Year - 2))
                                {
                                    t.DonationDate = cellDate;
                                }
                                t.Amount = Convert.ToString((range.Cells[rCnt, 11] as Excel.Range).Value2);
                                t.PhoneNo = Convert.ToString((range.Cells[rCnt, 12] as Excel.Range).Value2);
                                t.EmailId = Convert.ToString((range.Cells[rCnt, 13] as Excel.Range).Value2);
                                t.CreatedDate = DateTime.Now;
                                t.ModifiedDate = DateTime.Now;

                                donarDetails.Add(t);
                            }
                            catch(Exception ex)
                            {
                                continue;
                            }
                        }
                    }

                    xlWorkBook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);

                    _context.DonarDetails.AddRange(donarDetails);
                    _context.SaveChanges();
                }
            }
            return View("Index");
        }


        public ActionResult Donations()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetDonarGridInformation()
        {
            var retDonarDetails = _context.DonarDetails.ToList().Select(x => new DonarGridModel()
            {
                Satram= x.Satram,
                DonationFor = x.DonationFor,
                DonationBy = x.DonationBy,
                Relation = x.Relation,
                Address = x.Address,
                DonationDate = x.DonationDate.HasValue ? string.Format("{0:yyyy-MM-dd}", x.DonationDate.Value) : string.Empty,
                Amount = GetFormatedAmount(x.Amount),
                Purpose = x.Purpose,
                EmailId=x.EmailId,
                Thidi = string.IsNullOrEmpty(x.Thidi) ? string.Empty : x.Thidi,
                RecordDate=x.ModifiedDate.HasValue?x.ModifiedDate.Value:x.CreatedDate.Value,
                EditInfo = "<i class='fa fa-pencil-square-o' aria-hidden='true' style='cursor:pointer' title='Edit Donar Details' onclick='addEditModal(" + x.Id + ")'></i>"
            }).ToList().OrderByDescending(x => x.RecordDate);
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
        [HttpGet]
        public ActionResult GetDonarDetailsById(int Id)
        {
            var data = _context.DonarDetails.Where(x => x.Id == Id).ToList().Select(x => new DonarGridModel()
            {
                Satram =x.Satram.Trim(),
                DonationFor = x.DonationFor,
                DonationBy = x.DonationBy,
                Relation = x.Relation,
                Address = x.Address,
                DonationDate = x.DonationDate.HasValue ? string.Format("{0:yyyy-MM-dd}", x.DonationDate.Value) : string.Empty,
                Amount = x.Amount,
                Purpose = x.Purpose,
                PhoneNo=x.PhoneNo,
                EmailId = x.EmailId,
                Thidi = string.IsNullOrEmpty(x.Thidi) ? string.Empty : x.Thidi,

                EditInfo = "<i class='fa fa-pencil-square-o' aria-hidden='true' style='cursor:pointer' title='Edit Donar Details' onclick='addEditModal(" + x.Id + ")'></i>"
            }).FirstOrDefault();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDonarDetails(DonarGridModel d)
        {
            DateTime? dt = null;
            bool retVal = false;
            if(Convert.ToDecimal(d.Amount)>0)
            {
                var donarData = _context.DonarDetails.Where(x => x.Id == d.Id).FirstOrDefault() ?? new DonarDetail();
                donarData.DonationBy = d.DonationBy;
                donarData.DonationFor = d.DonationFor;
                donarData.Relation = d.Relation;
                donarData.Purpose = d.Purpose;
                donarData.DonationDate = !string.IsNullOrEmpty(d.DonationDate)?Convert.ToDateTime(d.DonationDate): dt;
                donarData.Thidi = d.Thidi;
                donarData.Amount = d.Amount;
                donarData.Address = d.Address;
                donarData.PhoneNo = d.PhoneNo;
                donarData.Satram = d.Satram;
                donarData.EmailId = d.EmailId;
                if (d.Id==-1)
                {
                    donarData.CreatedDate = DateTime.Now;
                    _context.DonarDetails.Add(donarData);
                }
                else
                {
                    donarData.ModifiedDate = DateTime.Now;
                }
                _context.SaveChanges();
                retVal = true;

            }
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }
    }
}