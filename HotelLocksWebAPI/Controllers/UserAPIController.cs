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
    public class UserAPIController : ControllerBase
    {
        public UserService objUserService;
        public IEmailService iemailService;
        private readonly IJWTManagerRepository _jWTManager;

        public UserAPIController(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objUserService = new UserService(emailService, jWTManager);
            this._jWTManager = jWTManager;
        }

        [AllowAnonymous]
        [HttpGet("GetUserNameByUserCode")]
        public ResultViewModel<UserLoginViewModel> GetUserNameByUserCode(long BranchID, string UserCode)
        {
            ResultViewModel<UserLoginViewModel> objResult = objUserService.GetUserNameByUserCode(BranchID, UserCode);
            return objResult;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public ResultViewModel<UserLoginViewModel> Login(string username, string password, long BranchID)
        {
            ResultViewModel<UserLoginViewModel> objResult = objUserService.Login(username, password, BranchID);
            return objResult;
        }

        [AllowAnonymous]
        [HttpPost("ChangePassword")]
        public ResultViewModel<string> ChangePassword(ChangePasswordViewModel objChangePasswordViewModel, long BranchID)
        {
            ResultViewModel<string> objResult = objUserService.ChangePassword(objChangePasswordViewModel, BranchID);
            return objResult;
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public ResultViewModel<string> ForgotPassword(string UserName, long BranchID)
        {
            ResultViewModel<string> objResult = objUserService.ForgotPassword(UserName, BranchID);
            return objResult;
        }

        [HttpPost("AddUserInfo")]
        public ResultViewModel<UserViewModel> AddUserInfo(UserViewModel objUser)
        {
            ResultViewModel<UserViewModel> objResult = objUserService.AddUser(objUser);
            return objResult;
        }

        [HttpPut("UpdateUserInfo")]
        public ResultViewModel<UserViewModel> UpdateUserInfo(UserViewModel objUser)
        {
            ResultViewModel<UserViewModel> objResult = objUserService.UpdateUserInfo(objUser);
            return objResult;
        }

        [HttpDelete("DeleteUser/{id}")]
        public ResultViewModel<string> DeleteUser(int id)
        {
            ResultViewModel<string> objResult = objUserService.DeleteUser(id);
            return objResult;
        }

        [HttpGet("GetUserDetailByUserID")]
        public ResultViewModel<UserViewModel> GetUserDetailByUserID(long UserID, long BranchID = 0)
        {
            ResultViewModel<UserViewModel> objResult = objUserService.GetUserDetailByUserID(UserID,BranchID);
            return objResult;
        }

        [HttpGet("GetUserList")]
        public ResultViewModel<List<UserViewModel>> GetUserList()
        {
            ResultViewModel<List<UserViewModel>> objResult = objUserService.GetUserList();
            return objResult;
        }

        [HttpGet("GetUserRoles")]
        public ResultViewModel<List<UserRoleMaster>> GetUserRoles(int RoleID)
        {
            ResultViewModel<List<UserRoleMaster>> objResult = objUserService.GetUserRoles(RoleID);
            return objResult;
        }


    }
}
