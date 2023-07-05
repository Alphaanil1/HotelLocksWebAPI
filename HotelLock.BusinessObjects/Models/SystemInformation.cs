using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
    public class SystemInformation
    {

        // // // SysInformationId,BranchId,SystemId,SystemPassword,SystemMemo,IsBlackListed,ModifiedDate,ModifiedBy
        public long SysInformationId { get; set; }
        public long BranchId { get; set; }
        public string SystemId { get; set; }
        public string SystemPassword { get; set; }
        public string SystemMemo { get; set; }
        public int IsBlackListed { get; set; }
        public long UserID { get; set; }
    }
}
