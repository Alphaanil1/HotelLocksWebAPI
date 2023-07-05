using HotelLock.BusinessObjects.Models;
using HotelLock.BusinessObjects.Models.Utility;
using HotelLock.DAL.Repositories;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotelLock.BLL
{
    public class UserService
    {
        private UserRepository objUserRepository;
        private BranchRepository objBranchRepository;
        IEmailService objEmailService = null;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Encrypt _encrypt = new Encrypt("alphaict2019");

        public UserService(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objUserRepository = new UserRepository();
            objEmailService = emailService;
            this._jWTManager = jWTManager;
            objBranchRepository = new BranchRepository();
        }

        public ResultViewModel<UserLoginViewModel> GetUserNameByUserCode(long BranchID, string UserCode)
        {
            try
            {
                //if (string.IsNullOrEmpty(BranchID.ToString()) || BranchID <= 0)
                //{
                //    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchDetailsRequireMessage };
                //}

                if (string.IsNullOrEmpty(UserCode.ToString()))
                {
                    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserCodeRequireMessage };
                }
                //else if (!objBranchRepository.IsBranchExists(BranchID))
                //{
                //    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchdoesNotExistsMessage };
                //}
                else
                {
                    UserLoginViewModel objResponse = objUserRepository.GetUserNameByUserCode(BranchID, UserCode);
                    if (objResponse == null)
                    {
                        return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserdetailNotFoundMessage };
                    }
                    else
                    {
                        return new ResultViewModel<UserLoginViewModel> { Result = objResponse, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.Success };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = Message.Exception.ToString() + ": " + ex.Message };
            }
        }

        public ResultViewModel<UserLoginViewModel> Login(string username, string password, long BranchID)
        {
            try
            {
                UserLoginViewModel objUser = new UserLoginViewModel();

                if (string.IsNullOrEmpty(username))
                {
                    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BlankUserNamePasswordMessage };  //
                }
                else if (string.IsNullOrEmpty(password))
                {
                    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BlankUserNamePasswordMessage };
                }
                //else if (string.IsNullOrEmpty(BranchID.ToString()) || BranchID <= 0)
                //{
                //    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchDetailsRequireMessage };
                //}
                //else if (!objBranchRepository.IsBranchExists(BranchID))
                //{
                //    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchdoesNotExistsMessage };
                //}
                else if (!objUserRepository.IsUserExists(username, BranchID))
                {
                    return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserdoesNotExistsMessage };
                }
                else
                {
                    objUser = objUserRepository.GetLoginUserDetails(username, _encrypt.EncryptString(password), BranchID);
                    if (objUser != null && objUser.UserID > 0)
                    {
                        var token = _jWTManager.Authenticate(objUser);
                        objUser.Token = token.Token.ToString();
                        return new ResultViewModel<UserLoginViewModel> { Result = objUser, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.LoginSuccessMessage };
                    }
                    else
                    {
                        return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.Unauthorized, Message = Message.Failure.ToString(), UserMessage = UserMessage.UnauthorizedUserMessage };   //changes instead of user does not exist
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserLoginViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

            }

        }

        public ResultViewModel<string> ChangePassword(ChangePasswordViewModel objChangePasswordViewModel, long BranchID)
        {
            try
            {
                string regexstring = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
                string errorMSG = "";
                UserLoginViewModel objUser = new UserLoginViewModel();

                if (string.IsNullOrEmpty(objChangePasswordViewModel.ConfirmPassword))
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.ConfirmPasswordRequireMessage };
                }
                else if (string.IsNullOrEmpty(objChangePasswordViewModel.Password))
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordRequireMessage };
                }
                //else if (!objBranchRepository.IsBranchExists(BranchID))
                //{
                //    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchdoesNotExistsMessage };
                //}
                else if (!objUserRepository.UserOldPasswordExists(objChangePasswordViewModel.UserId, _encrypt.EncryptString(objChangePasswordViewModel.OldPassword), BranchID))
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.OldPasswordNotMatchMessage };
                }
                else if (objChangePasswordViewModel.Password != objChangePasswordViewModel.ConfirmPassword)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordConfirmPasswordNotMatchMessage };
                }
                else if (!Regex.Match(objChangePasswordViewModel.Password, regexstring).Success)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordFormatChangeMessage };
                }
                else
                {
                    objUser = objUserRepository.GetUserDetailsByUserID(objChangePasswordViewModel.UserId, BranchID);
                    if (objUser != null)
                    {
                        objUser.Password = _encrypt.EncryptString(objChangePasswordViewModel.Password);
                        objUser.IsTempPassword = false;
                        bool res = objUserRepository.ChangePassword(objUser.UserID, objUser.Password, BranchID);
                        if (res)
                        {
                            return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Success.ToString(), UserMessage = UserMessage.PasswordChangeSuccessMessage };
                        }
                        else
                        {
                            return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordChangeFailureMessage };

                        }
                    }
                    else
                    {
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserdoesNotExistsMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }

        }

        public ResultViewModel<string> ForgotPassword(string username, long BranchID)
        {
            string regexstring = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
            string errorMSG = "";
            UserLoginViewModel objUser = new UserLoginViewModel();

            if (string.IsNullOrEmpty(username))
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = "UserName is required" };
            }
            else if (string.IsNullOrEmpty(BranchID.ToString()) || BranchID <= 0)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchDetailsRequireMessage };
            }
            //else if (!objBranchRepository.IsBranchExists(BranchID))
            //{
            //    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.BranchdoesNotExistsMessage };
            //}
            else if (!objUserRepository.IsUserExists(username, BranchID))
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserdoesNotExistsMessage };
            }
            else
            {
                try
                {
                    objUser = objUserRepository.GetUserDetailsByUserName(username, BranchID);
                    string tempPassword = _encrypt.RandomString(8);
                    objUser.Password = tempPassword;

                    if (objEmailService.SendForgotPasswordEmail(objUser, "Forgot Password"))
                    {

                        ChangePasswordViewModel objChangePasswordViewModel = new ChangePasswordViewModel()
                        {
                            UserId = objUser.UserID,
                            UserName = username,
                            Password = tempPassword,
                        };

                        //ResultViewModel<string> result1 = ChangePassword(changePasswordViewModel);

                        objUser.Password = _encrypt.EncryptString(objChangePasswordViewModel.Password);
                        objUser.IsTempPassword = true;
                        bool res = objUserRepository.ChangePassword(objUser.UserID, objUser.Password, BranchID);

                        if (res)
                        {
                            return new ResultViewModel<string> { Result = Message.Success.ToString(), ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.ForgotPasswordEmailSuccessMessage };
                        }
                        else
                        {
                            return new ResultViewModel<string> { Result = Message.Failure.ToString(), ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.ForgotPasswordSetFailureMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<string> { Result = UserMessage.ForgotPasswordEmailFailureMessage, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.ForgotPasswordEmailFailureMessage };
                    }
                }
                catch (Exception ex)
                {
                    return new ResultViewModel<string> { Result = "Error has occurred", ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Exception.ToString(), UserMessage = ex.Message.ToString() };
                }
            }

        }

        public ResultViewModel<UserViewModel> AddUser(UserViewModel objUser)
        {
            try
            {

                if (objUser == null)
                {
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (objUserRepository.IsUserExists(objUser.UserName, objUser.BranchID))
                {
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserExists };
                }
                else if (objUserRepository.IsUserCodeExists(objUser.UserCode, objUser.BranchID))
                {
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserExists };
                }
                else
                {
                    ResultViewModel<UserViewModel> objvalidation = UserValidation(objUser, "Add");
                    string UserRight = string.Empty;
                    UserRight += string.IsNullOrEmpty(objUser.AllowConfiguration.ToString()) ? "0" : objUser.AllowConfiguration.ToString();
                    UserRight += string.IsNullOrEmpty(objUser.LiftControl.ToString()) ? "0" : objUser.LiftControl.ToString();
                    UserRight += string.IsNullOrEmpty(objUser.RoomCreation.ToString()) ? "0" : objUser.RoomCreation.ToString();
                    UserRight += string.IsNullOrEmpty(objUser.RoomOperation.ToString()) ? "0" : objUser.RoomOperation.ToString();
                    objUser.UserRight = UserRight;

                    if (objvalidation.Message == Message.Success.ToString())
                    {

                        string tempPassword = _encrypt.RandomString(8);
                        string Password = tempPassword;
                        objUser.Password = _encrypt.EncryptString(tempPassword); ;
                                               
                        //string EncryptPassword = _encrypt.EncryptString(Password);
                        //objUser.Password = EncryptPassword;

                        // // // Start User Profile Pic
                        string profilePicPath = objUserRepository.ConvertBase64ToImage(objUser, 1);
                        objUser.UserPicURL = profilePicPath;
                        // // // END User Profile Pic

                        UserViewModel objResult = objUserRepository.AddUpdateUser(objUser);
                        objUser.Password = Password;
                        if (objResult.EmailID != null)
                        {
                            if (objEmailService.SendUserPasswordEmail(objUser, "HotelLock APP registration"))
                            {
                                return new ResultViewModel<UserViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.UserAddSuccessMessage };
                            }
                            else
                            {
                                return new ResultViewModel<UserViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserMailSendFailMessage };
                            }
                        }
                        else
                        {
                            return new ResultViewModel<UserViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserMailSendFailMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = objvalidation.UserMessage };
                    }


                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

            }
        }

        public ResultViewModel<UserViewModel> UpdateUserInfo(UserViewModel objUser)
        {
            try
            {

                if (objUser == null)
                {
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                //else if (objUserRepository.IsUserExists(objUser.UserName, objUser.BranchID))
                //{
                //    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserExists };
                //}
                //else if (objUserRepository.IsUserCodeExists(objUser.UserCode, objUser.BranchID))
                //{
                //    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserExists };
                //}
                else
                {
                    ResultViewModel<UserViewModel> objvalidation = UserValidation(objUser, "Edit");
                    string UserRight = string.Empty;
                    UserRight += string.IsNullOrEmpty(objUser.AllowConfiguration.ToString()) ? "0" : objUser.AllowConfiguration.ToString();
                    UserRight += string.IsNullOrEmpty(objUser.LiftControl.ToString()) ? "0" : objUser.LiftControl.ToString();
                    UserRight += string.IsNullOrEmpty(objUser.RoomCreation.ToString()) ? "0" : objUser.RoomCreation.ToString();
                    UserRight += string.IsNullOrEmpty(objUser.RoomOperation.ToString()) ? "0" : objUser.RoomOperation.ToString();
                    objUser.UserRight = UserRight;

                    if (objvalidation.Message == Message.Success.ToString())
                    {
                        //string Password = objUser.Password;
                        //string EncryptPassword = _encrypt.EncryptString(Password);
                        //objUser.Password = EncryptPassword;

                        // // // Start User Profile Pic
                        if (objUser.Base64string != null && !string.IsNullOrEmpty(objUser.Base64string))
                        {
                            string profilePicPath = objUserRepository.ConvertBase64ToImage(objUser, 1);
                            objUser.UserPicURL = profilePicPath;
                        }
                        // // // END User Profile Pic

                        UserViewModel objResult = objUserRepository.AddUpdateUser(objUser);
                        //objUser.Password = Password;
                        if (objResult != null)
                        {
                            return new ResultViewModel<UserViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.UserEditSuccessMessage };
                        }
                        else
                        {
                            return new ResultViewModel<UserViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.UserMailSendFailMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = objvalidation.UserMessage };
                    }


                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

            }
        }

        private ResultViewModel<UserViewModel> UserValidation(UserViewModel objUser, string strMode = "Add")
        {
            string errorMSG = "";
            string regexstring = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";

            try
            {
                DateTime Currentdate = Convert.ToDateTime(DateTime.Now.ToString("MMM-dd-yyyy") + " 00:00:00");

                if (objUser.FirstName.ToString() == null)
                {
                    errorMSG = "FirstName is required";
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (objUser.LastName.ToString() == null)
                {
                    errorMSG = "LastName is required";
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                if (objUser.EmailID.ToString() == null)
                {
                    errorMSG = "Email ID is required";
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                else if (objUser.MobileNumber == null)
                {
                    errorMSG = "Mobile Number is required";
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                if (objUser.MobileNumber.Length > 10)
                {
                    errorMSG = "Mobile Number cannot be greater than 10 digits";
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }
                if (IsValid(objUser.EmailID) == false)
                {
                    errorMSG = "Invalid EmailID";
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = errorMSG };
                }

                //if (objUser.Password != objUser.ConfirmPassword)
                //{
                //    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordConfirmPasswordNotMatchMessage };
                //}
                //else if (!Regex.Match(objUser.Password, regexstring).Success)
                //{
                //    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.PasswordFormatChangeMessage };
                //}

                return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

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

        public ResultViewModel<string> DeleteUser(int UserID, long BranchID = 0)
        {
            try
            {
                if (UserID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid UserID" };
                }
                else
                {
                    UserLoginViewModel objResult = objUserRepository.GetUserDetailsByUserID(UserID, BranchID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        objUserRepository.DeleteUser(UserID);
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.UserDeletedMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };

            }
        }

        public ResultViewModel<UserViewModel> GetUserDetailByUserID(long UserID, long BranchID = 0)
        {
            try
            {
                if (UserID <= 0)
                {
                    return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid UserID" };
                }
                else
                {
                    UserViewModel objResult = objUserRepository.GetUserDetailByUserID(UserID, BranchID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        string Password = objResult.Password;
                        string DecryptPassword = _encrypt.DecryptString(Password);
                        objResult.Password = DecryptPassword;

                        char[] _rights;
                        string userSecuredRights = Convert.ToString(objResult.UserRight, CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(userSecuredRights))
                        {
                            _rights = new char[4];
                            _rights = userSecuredRights.ToCharArray();

                            objResult.AllowConfiguration = Convert.ToString(_rights[0], CultureInfo.InvariantCulture) == "1" ? 1 : 0;
                            objResult.LiftControl = Convert.ToString(_rights[1], CultureInfo.InvariantCulture) == "1" ? 1 : 0;
                            objResult.RoomCreation = Convert.ToString(_rights[2], CultureInfo.InvariantCulture) == "1" ? 1 : 0;
                            if (_rights.Length == 4)
                            { objResult.RoomOperation = Convert.ToString(_rights[3], CultureInfo.InvariantCulture) == "1" ? 1 : 0; }
                        }
                        return new ResultViewModel<UserViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<UserViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<UserViewModel>> GetUserList()
        {
            try
            {
                List<UserViewModel> objResult = objUserRepository.GetUserList();

                if (objResult == null)
                {
                    return new ResultViewModel<List<UserViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                }
                else
                {
                    return new ResultViewModel<List<UserViewModel>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<UserViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<UserRoleMaster>> GetUserRoles(int RoleID = 0)
        {
            try
            {
                List<UserRoleMaster> objResponse = objUserRepository.GetUserRoles(RoleID);
                if (objResponse == null)
                {
                    return new ResultViewModel<List<UserRoleMaster>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                }
                else
                {
                    return new ResultViewModel<List<UserRoleMaster>> { Result = objResponse, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.Success };
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<UserRoleMaster>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.Message };
            }

        }
    }
}
