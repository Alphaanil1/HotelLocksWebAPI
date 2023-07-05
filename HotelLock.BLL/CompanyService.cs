using HotelLock.BusinessObjects.Models;
using HotelLock.BusinessObjects.Models.Utility;
using HotelLock.DAL.Repositories;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BLL
{
    public class CompanyService
    {
        private CompanyRepository objCompanyRepository;
        IEmailService objEmailService = null;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Encrypt _encrypt = new Encrypt("alphaict2019");

        public CompanyService(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objCompanyRepository = new CompanyRepository();
            objEmailService = emailService;
            this._jWTManager = jWTManager;
        }


        public ResultViewModel<companyMaster> AddCompanyInfo(companyMaster Objcompany, string Action, int UserID)
        {
            try
            {

                if (Objcompany == null)
                {
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                //else if (objCompanyRepository.IsCompanyExists(objcompany.CompanyName)) 
                //{
                //    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserExists };
                //}               
                else
                {
                    ResultViewModel<companyMaster> objvalidation = CompanyValidation(Objcompany, "Add");

                    if (objvalidation.Message == Message.Success.ToString())
                    {
                        companyMaster objResult = objCompanyRepository.AddUpdateCompany(Objcompany, Action, UserID);
                        if (objResult != null)
                        {
                            return new ResultViewModel<companyMaster> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.CompanyAddSuccessMessage };
                        }
                        else
                        {
                            return new ResultViewModel<companyMaster> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.CompanyAddFailureMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = objvalidation.UserMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = UserMessage.CompanyAddFailureMessage + " " + ex.ToString() };
            }
        }

        public ResultViewModel<companyMaster> UpdateCompanyInfo(companyMaster Objcompany, string Action, int UserID)
        {
            try
            {

                if (Objcompany == null)
                {
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    ResultViewModel<companyMaster> objvalidation = CompanyValidation(Objcompany, "Edit");

                    if (objvalidation.Message == Message.Success.ToString())
                    {
                        companyMaster objResult = objCompanyRepository.AddUpdateCompany(Objcompany, Action, UserID);
                        if (objResult != null)
                        {
                            return new ResultViewModel<companyMaster> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.CompanyEditSuccessMessage };
                        }
                        else
                        {
                            return new ResultViewModel<companyMaster> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.CompanyEditFailureMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = objvalidation.UserMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = UserMessage.CompanyEditFailureMessage + " " + ex.ToString() };
            }
        }

        private ResultViewModel<companyMaster> CompanyValidation(companyMaster objcompany, string strMode = "Add")
        {
            string errorMSG = "";
            string regexstring = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";

            try
            {
                DateTime Currentdate = Convert.ToDateTime(DateTime.Now.ToString("MMM-dd-yyyy") + " 00:00:00");

                if (objcompany.CompanyName.ToString() == null)
                {
                    errorMSG = "Company Name is required";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objcompany.Address.ToString() == null)
                {
                    errorMSG = "Company address is required";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objcompany.State.ToString() == null)
                {
                    errorMSG = "State is required";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objcompany.City.ToString() == null)
                {
                    errorMSG = "City is required";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objcompany.ZipCode.ToString() == null)
                {
                    errorMSG = "ZipCode is required";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                if (objcompany.EmailID.ToString() == null)
                {
                    errorMSG = "Email ID is required";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                else if (objcompany.MobileNumber == null)
                {
                    errorMSG = "Mobile Number is required";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                if (objcompany.MobileNumber.Length > 10)
                {
                    errorMSG = "Mobile Number cannot be greater than 10 digits";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (IsValid(objcompany.EmailID) == false)
                {
                    errorMSG = "Invalid EmailID";
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }



                return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
            }
            catch (Exception ex)
            {
                return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

            }
        }

        private static bool IsValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }


        public ResultViewModel<string> DeleteCompany(long CompanyID)
        {
            try
            {
                if (CompanyID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid company detail" };
                }
                else
                {
                    companyMaster objResult = objCompanyRepository.GetCompanyDetailsByCompanyID(CompanyID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        objCompanyRepository.DeleteCompany(CompanyID);
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.CompanyDeletedMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

            }
        }

        public ResultViewModel<companyMaster> GetCompanyDetailByCompanyID(long CompanyID)
        {
            try
            {
                if (CompanyID <= 0)
                {
                    return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid company detail" };
                }
                else
                {
                    companyMaster objResult = objCompanyRepository.GetCompanyDetailsByCompanyID(CompanyID);
                    if (objResult == null)
                    {
                        return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        return new ResultViewModel<companyMaster> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<companyMaster> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<companyMaster>> GetCompanyDetailList()
        {
            try
            {
                List<companyMaster> objResult = objCompanyRepository.GetCompanyDetailList();

                if (objResult == null)
                {
                    return new ResultViewModel<List<companyMaster>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                }
                else
                {
                    return new ResultViewModel<List<companyMaster>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<companyMaster>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }


    }
}
