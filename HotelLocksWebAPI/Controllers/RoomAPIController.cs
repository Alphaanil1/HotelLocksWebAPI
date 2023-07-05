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
    public class RoomAPIController : ControllerBase
    {
        public RoomService objRoomService;
        public IEmailService iemailService;
        private readonly IJWTManagerRepository _jWTManager;

        public RoomAPIController(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objRoomService = new RoomService(emailService, jWTManager);
            this._jWTManager = jWTManager;
        }


        [HttpGet("CreateRoomList")]
        public ResultViewModel<List<RoomsViewModel>> CreateRoomList(CreateRoomList objRooms)
        {
            ResultViewModel<List<RoomsViewModel>> objResult = objRoomService.CreateRoomList(objRooms);
            return objResult;
        }

        [HttpGet("GetRoomTypeListByBranchID")]
        public ResultViewModel<List<RoomTypes>> GetRoomTypeListByBranchID(long branchID)
        {
            ResultViewModel<List<RoomTypes>> objResult = objRoomService.GetRoomTypeListByBranchID(branchID);
            return objResult;
        }

        [HttpPost("AddUpdateRoomType")]
        public ResultViewModel<RoomTypes> AddUpdateRoomType(RoomTypes objRooms, string Action)
        {
            ResultViewModel<RoomTypes> objResult = objRoomService.AddUpdateRoomType(objRooms, Action);
            return objResult;
        }

        [HttpPost("AddRooms")]
        public ResultViewModel<string> AddRooms(List<RoomsViewModel> objRooms, string Action, long UserID)
        {
            ResultViewModel<string> objResult = objRoomService.AddRooms(objRooms, Action, UserID);
            return objResult;
        }

        [HttpGet("GetRoomDetailListByBranchID")]
        public ResultViewModel<List<RoomsDetailsViewModel>> GetRoomDetailListByBranchID(long branchID)
        {
            ResultViewModel<List<RoomsDetailsViewModel>> objResult = objRoomService.GetRoomDetailListByBranchID(branchID);
            return objResult;
        }

    }
}
