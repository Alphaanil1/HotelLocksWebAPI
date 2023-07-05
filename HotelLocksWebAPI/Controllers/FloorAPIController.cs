using HotelLock.BLL;
using HotelLock.BusinessObjects.Models;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  HotelLocksWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FloorAPIController : ControllerBase
    {
        public FloorService objFloorService;
        public IEmailService iemailService;
        private readonly IJWTManagerRepository _jWTManager;

        public FloorAPIController(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objFloorService = new FloorService(emailService, jWTManager);
            this._jWTManager = jWTManager;
        }


        [HttpPost("AddFloor")]
        public ResultViewModel<MasterDoorViewModel> AddFloor(FloorDetailViewModel objmasterDoor, string Action)
        {
            ResultViewModel<MasterDoorViewModel> objResult = objFloorService.AddFloor(objmasterDoor, Action);
            return objResult;
        }

        [HttpGet("GetFloorDetailList")]
        public ResultViewModel<List<FloorZoneDetailViewModel>> GetFloorDetailList(long BranchID)
        {
            ResultViewModel<List<FloorZoneDetailViewModel>> objResult = objFloorService.GetFloorDetailList(BranchID, 0);
            return objResult;
        }

        [HttpGet("GetFloorDetailByNodeID")]
        public ResultViewModel<MasterDoorViewModel> GetFloorDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            ResultViewModel<MasterDoorViewModel> objResult = objFloorService.GetFloorDetailByNodeID(NodeID, BranchID, ParentID);
            return objResult;
        }

        [HttpPut("UpdateFloorName")]
        public ResultViewModel<MasterDoorViewModel> UpdateFloorName(FloorDetailViewModel objmasterDoor, string Action)
        {
            ResultViewModel<MasterDoorViewModel> objResult = objFloorService.UpdateFloorName(objmasterDoor, Action);
            return objResult;
        }

        [HttpDelete("DeleteFloorDetailByNodeID")]
        public ResultViewModel<string> DeleteFloorDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            ResultViewModel<string> objResult = objFloorService.DeleteFloorDetailByNodeID(NodeID, BranchID, ParentID);
            return objResult;
        }

        [HttpPost("AddZone")]
        public ResultViewModel<MasterDoorViewModel> AddZone(FloorDetailViewModel objmasterDoor, string Action)
        {
            ResultViewModel<MasterDoorViewModel> objResult = objFloorService.AddZone(objmasterDoor, Action);
            return objResult;
        }

        [HttpGet("GetZoneDetailByFloorID")]
        public ResultViewModel<List<FloorDetailViewModel>> GetZoneDetailByFloorID(long BranchID, long FloorID)
        {
            ResultViewModel<List<FloorDetailViewModel>> objResult = objFloorService.GetZoneDetailByFloorID(BranchID, FloorID);
            return objResult;
        }

        [HttpPut("UpdateZoneName")]
        public ResultViewModel<string> UpdateZoneName(List<UpdateZoneDetailModel> objmasterDoor, string Action)
        {
            ResultViewModel<string> objResult = objFloorService.UpdateZoneName(objmasterDoor, Action);
            return objResult;
        }

        [HttpGet("GetFloorDetailListByBuildingID")]
        public ResultViewModel<List<FloorDetailViewModel>> GetFloorDetailListByBuildingID(long BranchID,long BuildingID)
        {
            ResultViewModel<List<FloorDetailViewModel>> objResult = objFloorService.GetFloorDetailListByBuildingID(BranchID, BuildingID);
            return objResult;
        }


    }
}
