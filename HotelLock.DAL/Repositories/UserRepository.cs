using Dapper;
using HotelLock.BusinessObjects.Models;
using HotelLock.BusinessObjects.Models.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotelLock.DAL.Repositories
{
    public class UserRepository
    {
        public UserLoginViewModel GetUserNameByUserCode(long BranchID, string UserCode)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<UserLoginViewModel>("[dbo].[USP_GET_UserDetailsByUserCodeBranchID]", new { @BranchID = BranchID, @UserCode = UserCode },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDBConnection.CloseConnection();
            }
        }

        public UserLoginViewModel GetUserDetailsByUserName(string UserName, long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<UserLoginViewModel>("[dbo].[USP_GET_UserDetailsByUserName]", new { @UserName = UserName, @BranchID = BranchID },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public UserLoginViewModel GetUserDetailsByPassoword(long UserID, string password, long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<UserLoginViewModel>("[dbo].[USP_GET_UserDetailsByPassword]", new { @UserID = UserID, @password = password, @BranchID = BranchID },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public UserLoginViewModel GetUserDetailsByUserID(long UserID, long BranchID = 0)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<UserLoginViewModel>("[dbo].[USP_GET_UserDetailsByUserID]", new { @UserID = UserID, @BranchID = BranchID },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool IsUserExists(string UserName, long BranchID)
        {
            try
            {
                UserLoginViewModel objResponse = GetUserDetailsByUserName(UserName, BranchID);

                if (objResponse == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool IsUserCodeExists(string UserCode, long BranchID = 0)
        {
            try
            {
                UserLoginViewModel objResponse = GetUserNameByUserCode(BranchID, UserCode);

                if (objResponse == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public UserLoginViewModel GetLoginUserDetails(string username, string password, long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUser = objDBConnection.connection.Query<UserLoginViewModel>("[dbo].[Usp_Get_UserLoginDetails]", new { @UserName = username, @Password = password, @BranchID = BranchID },
                 commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objUser;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDBConnection.CloseConnection();
            }
        }

        public bool UserOldPasswordExists(long UserID, string oldPassword, long BranchID)
        {
            try
            {
                UserLoginViewModel objResponse = GetUserDetailsByPassoword(UserID,oldPassword, BranchID);

                if (objResponse == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool ChangePassword(int Userid, string Password, long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objReturnvalue = objDBConnection.connection.Query<string>("[dbo].[USP_UPDATE_ChangePassword]", new { @Userid = Userid, @Password = Password, @BranchID = BranchID },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                if (objReturnvalue == "Success")
                { return true; }
                else
                { return false; }
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public UserViewModel AddUpdateUser(UserViewModel ObjUser)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserID", ObjUser.UserID);
                parameters.Add("@FirstName", ObjUser.FirstName);
                parameters.Add("@LastName", ObjUser.LastName);
                parameters.Add("@address", ObjUser.address);
                parameters.Add("@EmailID", ObjUser.EmailID);
                parameters.Add("@MobileNumber", ObjUser.MobileNumber);
                parameters.Add("@UserCode", ObjUser.UserCode);
                parameters.Add("@UserName", ObjUser.UserName);
                parameters.Add("@Password", ObjUser.Password);
                parameters.Add("@UserPicURL", ObjUser.UserPicURL);
                parameters.Add("@UserRoleID", ObjUser.UserRoleID);
                parameters.Add("@IsAdmin", ObjUser.IsAdmin);
                parameters.Add("@BranchID", ObjUser.BranchID);
                parameters.Add("@Inactive", ObjUser.Inactive);
                parameters.Add("@ComponentTag", ObjUser.ComponentTag);
                parameters.Add("@UserRight", ObjUser.UserRight);
                parameters.Add("@Action", ObjUser.Action);

                var objReturnvalue = objDBConnection.connection.Query<UserViewModel>("[dbo].[USP_INSERTUPDATE_UserDetails]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public string ConvertBase64ToImage(UserAccessRights ObjUser, int IsProfile = 0)
        {
            try
            {
                string imageName = "";
                var contentTypeFile = "image/jpeg";
                var fileName = "images.jpeg";
                string filePath = Directory.GetCurrentDirectory() + "\\Content\\UserPictures";

                string VirtualfilePath = "http://64.202.191.110:2242/HotelLockContent/UserPictures/";
                // string VirtualfilePath = "http://localhost:18520/HotelLockContent/UserPictures/";

                string Base62strings = ObjUser.Base64string;

                Base62strings = Regex.Replace(Base62strings, @"^.+?(;base64),", string.Empty);
                string converted = Base62strings.Replace('-', '+');
                converted = converted.Replace('_', '/');
                Base62strings = converted;

                imageName = "User_" + ObjUser.UserCode + ".jpeg";

                byte[] imageBytes = Convert.FromBase64String(Base62strings);

                // // //  if file exist, delete file & create new
                if (System.IO.File.Exists(filePath + "\\" + imageName))
                {
                    System.IO.File.Delete(filePath + "\\" + imageName);
                }

                using (var imageFile = new FileStream(filePath + "\\" + imageName, FileMode.Create))
                {
                    imageFile.Write(imageBytes, 0, imageBytes.Length);
                    imageFile.Flush();
                }

                //ErrorLogs.ErrorLogAndNotification(filePath + "\\" + imageName);
                return VirtualfilePath + "\\" + imageName;
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLogAndNotification(ex.Message);
                return string.Empty;
            }
        }

        public bool DeleteUser(long UserID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUser = objDBConnection.connection.Query<string>("[dbo].[Usp_DELETE_UserDetails]", new { @UserID = UserID },
                 commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                if (objUser == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDBConnection.CloseConnection();
            }
        }

        public UserViewModel GetUserDetailByUserID(long UserID, long BranchID = 0)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<UserViewModel>("[dbo].[USP_GET_UserDetailsByUserID]", new { @UserID = UserID, @BranchID = BranchID },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<UserViewModel> GetUserList()
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<UserViewModel>("[dbo].[USP_GET_UserDetailList]",
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<UserRoleMaster> GetUserRoles(int RoleID=0)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<UserRoleMaster>("[dbo].[USP_GET_UserRoleDetails]", new { @RoleID = RoleID },
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
