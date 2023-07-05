using HotelLock.BusinessObjects.Models;
using HotelLock.BusinessObjects.Models.Utility;
using HotelLock.DAL.Repositories;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotelLock.BLL
{

    public class BranchService
    {
        private BranchRepository objBranchRepository;
        IEmailService objEmailService = null;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Encrypt _encrypt = new Encrypt("alphaict2019");

        public BranchService(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objBranchRepository = new BranchRepository();
            objEmailService = emailService;
            this._jWTManager = jWTManager;
        }

        public ResultViewModel<List<BranchDetails>> GetAllBranchDetails(bool IsLoginForm)
        {
            try
            {
                List<BranchDetails> objResponse = objBranchRepository.GetAllBranchDetails(IsLoginForm);
                if (objResponse == null)
                {
                    return new ResultViewModel<List<BranchDetails>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchdetailNotFoundMessage };
                }
                else
                {
                    return new ResultViewModel<List<BranchDetails>> { Result = objResponse, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.Success };
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<BranchDetails>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = Message.Exception.ToString() + ": " + ex.Message };
            }
        }

        public ResultViewModel<HotelDetailViewModel> AddHotelInfo(HotelDetailViewModel ObjHotel, string Action, int UserID)
        {
            try
            {
                if (ObjHotel == null)
                {
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (objBranchRepository.IsBranchNameExists(ObjHotel.BranchID, ObjHotel.BranchName, ObjHotel.CompanyID, Action))
                {
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchExists };
                }
                else
                {
                    ResultViewModel<HotelDetailViewModel> objvalidation = BranchValidation(ObjHotel, Action);

                    if (objvalidation.Message == Message.Success.ToString())
                    {
                        string Password = ObjHotel.Password;
                        //string EncryptPassword = _encrypt.EncryptString(Password);
                        //ObjHotel.Password = EncryptPassword;

                        HotelDetailViewModel objResult = objBranchRepository.AddUpdateBranch(ObjHotel, Action, UserID);
                        if (objResult != null)
                        {
                            string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.BranchAddSuccessMessage : UserMessage.BranchEditSuccessMessage);
                            return new ResultViewModel<HotelDetailViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                        }
                        else
                        {
                            string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.BranchAddFailureMessage : UserMessage.BranchEditFailureMessage);
                            return new ResultViewModel<HotelDetailViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = objvalidation.UserMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        private ResultViewModel<HotelDetailViewModel> BranchValidation(HotelDetailViewModel objBranch, string strMode = "Add")
        {
            string errorMSG = "";
            string regexstring = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";

            try
            {
                DateTime Currentdate = Convert.ToDateTime(DateTime.Now.ToString("MMM-dd-yyyy") + " 00:00:00");

                if (objBranch.BranchName.ToString() == null)
                {
                    errorMSG = "Hotel Name is required";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objBranch.Address.ToString() == null)
                {
                    errorMSG = "Hotel address is required";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objBranch.State.ToString() == null)
                {
                    errorMSG = "State is required";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objBranch.City.ToString() == null)
                {
                    errorMSG = "City is required";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objBranch.ZipCode.ToString() == null)
                {
                    errorMSG = "ZipCode is required";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                if (objBranch.EmailID.ToString() == null)
                {
                    errorMSG = "Email ID is required";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                else if (objBranch.MobileNo == null)
                {
                    errorMSG = "Mobile Number is required";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                if (objBranch.MobileNo.Length > 10)
                {
                    errorMSG = "Mobile Number cannot be greater than 10 digits";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (IsValid(objBranch.EmailID) == false)
                {
                    errorMSG = "Invalid EmailID";
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                if (objBranch.Password != objBranch.ConfirmPassword)
                {
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordConfirmPasswordNotMatchMessage };
                }
                else if (!Regex.Match(objBranch.Password, regexstring).Success)
                {
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordFormatChangeMessage };
                }


                return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
            }
            catch (Exception ex)
            {
                return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

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


        public ResultViewModel<string> DeleteHotel(long BranchID)
        {
            try
            {
                if (BranchID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid branch detail" };
                }
                else
                {
                    HotelDetailViewModel objResult = objBranchRepository.GetBranchDetailsByBranchID(BranchID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        objBranchRepository.DeleteBranch(BranchID);
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.BranchDeletedMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<HotelDetailViewModel> GetBranchDetailByBranchID(long BranchID)
        {
            try
            {
                if (BranchID <= 0)
                {
                    return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid branch detail" };
                }
                else
                {
                    
                    HotelDetailViewModel objResult = objBranchRepository.GetBranchDetailsByBranchID(BranchID);
                    if (objResult == null)
                    {
                        return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        //string Password = objResult.Password;
                        //string DecryptPassword = _encrypt.DecryptString(Password);
                        //objResult.Password = DecryptPassword;

                        return new ResultViewModel<HotelDetailViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<HotelDetailViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<HotelDetailViewModel>> GetBranchDetailList()
        {
            try
            {
                List<HotelDetailViewModel> objResult = objBranchRepository.GetBranchDetailList();

                if (objResult == null)
                {
                    return new ResultViewModel<List<HotelDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                }
                else
                {
                    return new ResultViewModel<List<HotelDetailViewModel>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<HotelDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }


    }
}
