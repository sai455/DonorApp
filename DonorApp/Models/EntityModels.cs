using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonorApp.Models
{
    public class ThidiMaster
    {
        
        public int Id { get; set; }
        public string Thidi_Telugu { get; set; }
        public string Paksha_Telugu { get; set; }
        public string Thidi_English { get; set; }
        public string Paksha_English { get; set; }
        public string Thidi_Paksha_TeluguName { get; set; }
        public string Thidi_Paksha_EnglishName { get; set; }
    }
    public class MonthMaster
    {
        public int Id { get; set; }
        public string TeluguMonth { get; set; }
        public string EnglishMonth { get; set; }
    }
    public class DonarDetail
    {
        public Int64 Id { get; set; }
        public string DonationFor { get; set; }
        public string Purpose { get; set; }
        public string DonationBy { get; set; }
        public string Relation { get; set; }
        public string Gothram { get; set; }
        public string Address { get; set; }
        public string Thidi { get; set; }
        public string DonationDateTelugu { get; set; }
        public DateTime? DonationDate { get; set; }
        public string Amount { get; set; }
        public string Satram { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}