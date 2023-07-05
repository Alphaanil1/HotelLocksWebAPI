using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
    public class BranchDetails 
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public bool IsMainBranch { get; set; }
        public string MobileNo { get; set; }
        public string TelephoneNo { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public int Isactive { get; set; }
    }

    public class HotelDetailViewModel : BranchDetails
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
