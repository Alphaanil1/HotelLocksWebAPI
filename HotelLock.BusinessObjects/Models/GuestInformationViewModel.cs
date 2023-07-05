using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
   public class GuestInformationViewModel
    {       
        public string GuestID { get; set; }
        public string GuestName { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public bool Married { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; }
        public int DocumentID { get; set; }
        public string DocumentNumber { get; set; }
        public string ZipCode { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string EmailID { get; set; }
        public int CheckOutStatus { get; set; }
        public int StayingDays { get; set; }
        public int BranchID { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string GuestDescription { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
 
   
    public class InsertIssueCardViewModel
    {      
        public int DoorID { get; set; }
        public string CardNo { get; set; }
        public string CardHolderID { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }       
        public int BranchId { get; set; }
        public string ComputerName { get; set; }
        public int ModifiedBy { get; set; }
    }
    public class InsertIssueCardResponseViewModel
    {
        public int IssueCardID { get; set; }
        public int DoorID { get; set; }
        public string CardNo { get; set; }
        public string CardHolderID { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BranchId { get; set; }
        public string ComputerName { get; set; }
        public int ModifiedBy { get; set; }
        public int ModifiedDate { get; set; }
        public string CardType { get; set; }
        public int CardStatus { get; set; }

    }
    public class ModifyIssueCardViewModel
    {
        public int IssueCardID { get; set; }
        public int TimeSectionID { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool HHDAccess { get; set; }
        public int CardStatus { get; set; }
        public string CardDesc { get; set; }
        public string ComputerName { get; set; }
        public int ModifiedBy { get; set; }
        public int DoorID { get; set; }
        public string CardHolderID { get; set; }
    }
    public class ModifyGuestCardStatusViewModel
    {
        public string GuestID { get; set; }
        public int CheckOutStatus { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }

}
