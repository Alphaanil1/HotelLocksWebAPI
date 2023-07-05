using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
    public class MasterDoor
    {
        // //  NodeID, ParentID, BranchID, NodeNo,    NodeName, ModifiedDate, ModifiedBy
        public long NodeID { get; set; }
        public long ParentID { get; set; }
        public long BranchID { get; set; }
        public string NodeNo { get; set; }
        public string NodeName { get; set; }
    }

    public class MasterDoorViewModel : MasterDoor
    {
        public long UserID { get; set; }
        public string BranchName { get; set; }
        public string BuildingName { get; set; }
    }

    public class FloorDetailViewModel
    {
        public long BranchID { get; set; }
        public string BranchName { get; set; }
        public long BuildingID { get; set; }
        public long BuildingParentID { get; set; }
        public string BuildingNo { get; set; }
        public string BuildingName { get; set; }
        public long FloorID { get; set; }
        public long FloorParentID { get; set; }
        public string FloorNo { get; set; }
        public string FloorName { get; set; }
        public long ZoneID { get; set; }
        public long ZoneParentID { get; set; }
        public string ZoneNo { get; set; }
        public string ZoneName { get; set; }
        public long UserID { get; set; }
    }


    public class UpdateZoneDetailModel
    {
        public long BranchID { get; set; }
        public long FloorID { get; set; }
        public long ZoneID { get; set; }
        public string ZoneName { get; set; }
        public long UserID { get; set; }
        public long IsDeleted { get; set; }
    }

    public class FloorZoneDetailViewModel
    {
        public long BranchID { get; set; }
        public string BranchName { get; set; }
        public long BuildingID { get; set; }
        public long BuildingParentID { get; set; }
        public string BuildingNo { get; set; }
        public string BuildingName { get; set; }
        public long FloorID { get; set; }
        public long FloorParentID { get; set; }
        public string FloorNo { get; set; }
        public string FloorName { get; set; }
        public List<ZoneDetailsViewModel> zoneDetails { get; set; }
        public long UserID { get; set; }
    }

    public class ZoneDetailsViewModel
    {
        public long ZoneID { get; set; }
        public long ZoneParentID { get; set; }
        public string ZoneNo { get; set; }
        public string ZoneName { get; set; }
    }
}
