using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
    public class companyMaster
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNo { get; set; }
        public string EmailID { get; set; }
        public int Isactive { get; set; }
    }
}
