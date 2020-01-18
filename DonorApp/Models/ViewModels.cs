using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonorApp.Models
{
    public class DonarGridModel
    {
        public Int64 Id { get; set; }
        public string Satram { get; set; }
        public string DonationFor { get; set; }
        public string DonationBy { get; set; }
        public string Relation { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string DonationDate { get; set; }
        public string Amount { get; set; }
        public string Purpose { get; set; }
        public string Thidi { get; set; }
        public string EditInfo { get; set; }
        public string EmailId { get; set; }
        public DateTime RecordDate { get; set; }
    }
    public class ThidiUploadModel
    {
        public string ThidiTeluguName { get; set; }
        public string ThidiEnglishName{ get; set; }
        public string Thidi{ get; set; }
        public string Paksha{ get; set; }
        public string ThidiEnglish{ get; set; }
        public string PakshaEnglish{ get; set; }
    }
}