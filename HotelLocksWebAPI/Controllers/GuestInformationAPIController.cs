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
    public class GuestInformationAPIController : ControllerBase
    {
        public GuestInformationService objGuestInformationService;
        public IEmailService iemailService;
        private readonly IJWTManagerRepository _jWTManager;

        public GuestInformationAPIController(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objGuestInformationService = new GuestInformationService(emailService, jWTManager);
            this._jWTManager = jWTManager;
        }
        [HttpPost("AddGuestInformation")]
        [AllowAnonymous]
        public ResultViewModel<GuestInformationViewModel> InsertGuestInformation(GuestInformationViewModel objGuestInformationViewModel)
        {
            ResultViewModel<GuestInformationViewModel> objResult = objGuestInformationService.InsertGuestCard(objGuestInformationViewModel);
            return objResult;
        }
        [HttpPost("ModifyGuestInformation")]
        [AllowAnonymous]
        public ResultViewModel<GuestInformationViewModel> ModifyGuestInformation(GuestInformationViewModel objGuestInformationViewModel)
        {
            ResultViewModel<GuestInformationViewModel> objResult = objGuestInformationService.ModifyGuestCard(objGuestInformationViewModel);
            return objResult;
        }

        [HttpPost("ModifyGuestCardStatus")]
        [AllowAnonymous]
        public ResultViewModel<ModifyGuestCardStatusViewModel> ModifyGuestCardStatusInformation(ModifyGuestCardStatusViewModel objGuestInformationViewModel)
        {
            ResultViewModel<ModifyGuestCardStatusViewModel> objResult = objGuestInformationService.ModifyGuestCardStatus(objGuestInformationViewModel);
            return objResult;
        }
    }
}
