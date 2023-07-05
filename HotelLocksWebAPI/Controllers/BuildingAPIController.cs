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
    public class BuildingAPIController : ControllerBase
    {
        public BuildingService objBuildingService;
        public IEmailService iemailService;
        private readonly IJWTManagerRepository _jWTManager;

        public BuildingAPIController(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objBuildingService = new BuildingService(emailService, jWTManager);
            this._jWTManager = jWTManager;
        }


        [HttpPost("AddBuilding")]
        public ResultViewModel<MasterDoorViewModel> AddBuilding(long BranchID, string Action, int UserID)
        {
            ResultViewModel<MasterDoorViewModel> objResult = objBuildingService.AddBuilding(BranchID, Action, UserID);
            return objResult;
        }

        [HttpGet("GetBuildingDetailList")]
        public ResultViewModel<List<MasterDoorViewModel>> GetBuildingDetailList(long BranchID)
        {
            ResultViewModel<List<MasterDoorViewModel>> objResult = objBuildingService.GetBuildingDetailList(BranchID);
            return objResult;
        }

        [HttpPut("UpdateBuildingName")]
        public ResultViewModel<MasterDoorViewModel> UpdateBuildingName(MasterDoorViewModel objmasterDoor, string Action)
        {
            ResultViewModel<MasterDoorViewModel> objResult = objBuildingService.UpdateBuildingName(objmasterDoor, Action);
            return objResult;
        }

        [HttpGet("GetBuildingDetailByNodeID")]
        public ResultViewModel<MasterDoorViewModel> GetBuildingDetailByNodeID(long NodeID, long BranchID,long ParentID)
        {
            ResultViewModel<MasterDoorViewModel> objResult = objBuildingService.GetBuildingDetailByNodeID(NodeID,BranchID, ParentID);
            return objResult;
        }

        [HttpDelete("DeleteBuildingDetailByNodeID")]
        public ResultViewModel<string> DeleteBuildingDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            ResultViewModel<string> objResult = objBuildingService.DeleteBuildingDetailByNodeID(NodeID, BranchID, ParentID);
            return objResult;
        }

    }
}
