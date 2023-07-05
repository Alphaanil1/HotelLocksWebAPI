using Dapper;
using HotelLock.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.DAL.Repositories
{
    public class BranchRepository
    {
        public List<BranchDetails> GetAllBranchDetails(bool IsLoginForm)
        {

            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objManagers = objDBConnection.connection.Query<BranchDetails>("[dbo].[USP_GET_BranchDetails]", new { @IsLoginForm = IsLoginForm },
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objManagers;
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

        public HotelDetailViewModel GetBranchDetailsByBranchID(long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objManagers = objDBConnection.connection.Query<HotelDetailViewModel>("[dbo].[USP_GET_BranchDetailsByBranchID]", new { @BranchID = BranchID },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objManagers;
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

        public HotelDetailViewModel GetBranchDetailsByBranchName(long BranchID, string BranchName, long CompanyID,string Action)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objManagers = objDBConnection.connection.Query<HotelDetailViewModel>("[dbo].[USP_GET_BranchDetailsByBranchName]", new { @BranchID = BranchID, @BranchName = BranchName, @CompanyID = CompanyID, @Action = Action },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objManagers;
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

        public bool IsBranchExists(long BranchID)
        {
            try
            {
                HotelDetailViewModel objResponse = GetBranchDetailsByBranchID(BranchID);

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

        public bool IsBranchNameExists(long BranchID, string BranchName, long CompanyID,string Action)
        {
            try
            {
                HotelDetailViewModel objResponse = GetBranchDetailsByBranchName(BranchID, BranchName, CompanyID, Action);

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

        public HotelDetailViewModel AddUpdateBranch(HotelDetailViewModel ObjObjHotel, string Action, int UserID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BranchID", ObjObjHotel.BranchID);
                parameters.Add("@BranchName", ObjObjHotel.BranchName);
                parameters.Add("@CompanyID", ObjObjHotel.CompanyID);
                parameters.Add("@Address", ObjObjHotel.Address);
                parameters.Add("@State", ObjObjHotel.State);
                parameters.Add("@City", ObjObjHotel.City);
                parameters.Add("@ZipCode", ObjObjHotel.ZipCode);
                parameters.Add("@MobileNumber", ObjObjHotel.MobileNo);
                parameters.Add("@TelephoneNo", ObjObjHotel.TelephoneNo);
                parameters.Add("@EmailID", ObjObjHotel.EmailID);
                parameters.Add("@IsMainBranch", ObjObjHotel.IsMainBranch);
                parameters.Add("@Action", Action);
                parameters.Add("@UserID", UserID);
                parameters.Add("@Isactive", ObjObjHotel.Isactive);
                parameters.Add("@Password", ObjObjHotel.Password);

                var objReturnvalue = objDBConnection.connection.Query<HotelDetailViewModel>("[dbo].[USP_INSERTUPDATE_BranchDetails]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool DeleteBranch(long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUser = objDBConnection.connection.Query<string>("[dbo].[Usp_DELETE_BranchDetails]", new { @BranchID = BranchID },
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

        public List<HotelDetailViewModel> GetBranchDetailList()
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<HotelDetailViewModel>("[dbo].[USP_GET_BranchDetailsByBranchID]", new { @BranchID = 0 },
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<SystemInformation> GetSystemInformationByPassword(int branchid, string systemId, string password, int IsBlackLisetd)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BranchID", branchid);
                parameters.Add("@systemId", systemId);
                parameters.Add("@SystemPassword", password);
                parameters.Add("@IsBlackLisetd", IsBlackLisetd);

                var objManagers = objDBConnection.connection.Query<SystemInformation>("[dbo].[USP_GET_CheckSystemPasswordExists]", parameters,
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objManagers;
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
       
        public bool CheckPasswordIsBlackListed(int branchid, string systemId, string password, int IsBlackLisetd)
        {
            List<SystemInformation> objResponse = GetSystemInformationByPassword(branchid, systemId, password, IsBlackLisetd);

            if (objResponse == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
