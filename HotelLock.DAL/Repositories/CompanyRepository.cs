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
    public class CompanyRepository
    {


        public companyMaster AddUpdateCompany(companyMaster Objcompany, string Action, int UserID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CompanyID", Objcompany.CompanyID);
                parameters.Add("@CompanyName", Objcompany.CompanyName);
                parameters.Add("@Address", Objcompany.Address);
                parameters.Add("@State", Objcompany.State);
                parameters.Add("@City", Objcompany.City);
                parameters.Add("@ZipCode", Objcompany.ZipCode);
                parameters.Add("@MobileNumber", Objcompany.MobileNumber);
                parameters.Add("@TelephoneNo", Objcompany.TelephoneNo);
                parameters.Add("@EmailID", Objcompany.EmailID);
                parameters.Add("@Action", Action);
                parameters.Add("@UserID", UserID);
                parameters.Add("@Isactive", Objcompany.Isactive);

                var objReturnvalue = objDBConnection.connection.Query<companyMaster>("[dbo].[USP_INSERTUPDATE_CompanyDetails]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool DeleteCompany(long CompanyID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUser = objDBConnection.connection.Query<string>("[dbo].[Usp_DELETE_CompanyDetails]", new { @CompanyID = CompanyID },
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

        public companyMaster GetCompanyDetailsByCompanyID(long CompanyID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<companyMaster>("[dbo].[USP_GET_CompanyDetailsByCompanyID]", new { @CompanyID = CompanyID },
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objUserLogin;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<companyMaster> GetCompanyDetailList()
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUserLogin = objDBConnection.connection.Query<companyMaster>("[dbo].[USP_GET_CompanyDetailsByCompanyID]", new { @CompanyID = 0 },
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
