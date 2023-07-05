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
namespace HotelLocksWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BranchAPIController : ControllerBase
    {
        public BranchService objBranchService;
        public IEmailService iemailService;
        private readonly IJWTManagerRepository _jWTManager;

        public BranchAPIController(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objBranchService = new BranchService(emailService, jWTManager);
            this._jWTManager = jWTManager;
        }

        [AllowAnonymous]
        [HttpGet("GetAllBranchDetails")]
        public ResultViewModel<List<BranchDetails>> GetAllBranchDetails(bool IsLoginForm)
        {
            ResultViewModel<List<BranchDetails>> objResult = objBranchService.GetAllBranchDetails(IsLoginForm);
            return objResult;
        }

        [HttpPost("AddHotelInfo")]
        public ResultViewModel<HotelDetailViewModel> AddHotelInfo(HotelDetailViewModel ObjHotel, string Action, int UserID)
        {
            ResultViewModel<HotelDetailViewModel> objResult = objBranchService.AddHotelInfo(ObjHotel, Action, UserID);
            return objResult;
        }

        [HttpPut("UpdateHotelInfo")]
        public ResultViewModel<HotelDetailViewModel> UpdateHotelInfo(HotelDetailViewModel ObjHotel, string Action, int UserID)
        {
            ResultViewModel<HotelDetailViewModel> objResult = objBranchService.AddHotelInfo(ObjHotel, Action, UserID);
            return objResult;
        }

        [HttpDelete("DeleteHotel/{BranchID}")]
        public ResultViewModel<string> DeleteHotel(long BranchID)
        {
            ResultViewModel<string> objResult = objBranchService.DeleteHotel(BranchID);
            return objResult;
        }

        [HttpGet("GetHotelDetailByHotelID")]
        public ResultViewModel<HotelDetailViewModel> GetHotelDetailByHotelID(long BranchID)
        {
            ResultViewModel<HotelDetailViewModel> objResult = objBranchService.GetBranchDetailByBranchID(BranchID);
            return objResult;
        }

        [HttpGet("GetBranchDetailList")]
        public ResultViewModel<List<HotelDetailViewModel>> GetBranchDetailList()
        {
            ResultViewModel<List<HotelDetailViewModel>> objResult = objBranchService.GetBranchDetailList();
            return objResult;
        }
    }
}
