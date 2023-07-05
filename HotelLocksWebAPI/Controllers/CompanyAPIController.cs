using HotelLock.BLL;
using HotelLock.BusinessObjects.Models;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  HotelLocksWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyAPIController : ControllerBase
    {
        public CompanyService objCompanyService;
        public IEmailService iemailService;
        private readonly IJWTManagerRepository _jWTManager;

        public CompanyAPIController(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objCompanyService = new CompanyService(emailService, jWTManager);
            this._jWTManager = jWTManager;
        }

        [HttpPost("AddCompanyInfo")]
        public ResultViewModel<companyMaster> AddCompanyInfo(companyMaster Objcompany, string Action, int UserID)
        {
            ResultViewModel<companyMaster> objResult = objCompanyService.AddCompanyInfo(Objcompany, Action, UserID);
            return objResult;
        }

        [HttpPut("UpdateCompanyInfo")]
        public ResultViewModel<companyMaster> UpdateCompanyInfo(companyMaster Objcompany, string Action, int UserID)
        {
            ResultViewModel<companyMaster> objResult = objCompanyService.UpdateCompanyInfo(Objcompany, Action,UserID);
            return objResult;
        }

        [HttpDelete("DeleteCompany/{CompanyID}")]
        public ResultViewModel<string> DeleteCompany(long CompanyID)
        {
            ResultViewModel<string> objResult = objCompanyService.DeleteCompany(CompanyID);
            return objResult;
        }

        [HttpGet("GetCompanyDetailByCompanyID")]
        public ResultViewModel<companyMaster> GetCompanyDetailByCompanyID(long CompanyID)
        {
            ResultViewModel<companyMaster> objResult = objCompanyService.GetCompanyDetailByCompanyID(CompanyID);
            return objResult;
        }

        [HttpGet("GetCompanyDetailList")]
        public ResultViewModel<List<companyMaster>> GetCompanyDetailList()
        {
            ResultViewModel<List<companyMaster>> objResult = objCompanyService.GetCompanyDetailList();
            return objResult;
        }
    }
}
