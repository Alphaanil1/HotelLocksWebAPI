using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
    public class Rooms
    {
        public long BranchID { set; get; }
        public long DoorID { set; get; }
        public long NodeID { set; get; }
        public string DoorNo { set; get; }
        public string DoorName { set; get; }
        public bool CommonDoor { set; get; }
        public string GuestCount { set; get; }
        public string RoomStatus { set; get; }
        public int intRoomStatus { set; get; }
        public string RoomType { set; get; }
        public int? RoomTypeID { set; get; }
        public string LinkID { set; get; }
        public string DoorDescription { set; get; }
        public int ReservationDoorID { set; get; }
        public bool Inactive { set; get; }
        public string InactiveStatus { set; get; }
        public bool IsInnerRoom { set; get; }

        //---lockbatteries table
        public DateTime? ChangedBatteryDateTime { set; get; }
        public bool IsDisplay { set; get; }
    }

    public class RoomsViewModel : Rooms
    {
        public long UserID { get; set; }
    }

    public class CreateRoomList
    {
        public long BranchID { set; get; }
        public long BuildingID { set; get; }
        public long FloorID { set; get; }
        public long TotalRooms { set; get; }
        public bool IsInnerRoom{ set; get; }
        public string ParentRoomNo { set; get; }
        public long InnerRoomCount { set; get; }
    }

    public class RoomTypes
    {
        public long BranchID { set; get; }
        public long RoomTypeID { set; get; }
        public string RoomType { set; get; }
        public long UserID { set; get; }
        public string BranchName { set; get; }
    }

    public class RoomsDetailsViewModel: FloorDetailViewModel
    {
        public long DoorID { set; get; }
        public long RoomParentID { set; get; }
        public string DoorNo { set; get; }
        public string DoorName { set; get; }
        public string RoomStatus { set; get; }
        public int intRoomStatus { set; get; }
        public string RoomType { set; get; }
        public int? RoomTypeID { set; get; }
    }

}
