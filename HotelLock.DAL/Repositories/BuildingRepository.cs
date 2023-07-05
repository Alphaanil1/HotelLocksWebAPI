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
    public class BuildingRepository
    {

        public List<MasterDoorViewModel> GetAllBuildingDetailsByBranchID(long BranchID, long ParentID, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objManagers = objDBConnection.connection.Query<MasterDoorViewModel>("[dbo].[USP_GET_MasterDoorDetails]", new { @BranchID = BranchID, @ParentID = ParentID, @NodeLevel = NodeLevel },
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

        public bool CheckBuildingLimit(long BranchID, long ParentID, int NodeLevel)
        {
            try
            {
                List<MasterDoorViewModel> objResponse = GetAllBuildingDetailsByBranchID(BranchID, ParentID, NodeLevel);

                if (objResponse == null && objResponse.Count == 0)
                {
                    return false;
                }
                else if (objResponse.Count >= 26)
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

        public MasterDoorViewModel AddMasterDoorDetails(MasterDoorViewModel objMstDoor, string Action, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BranchID", objMstDoor.BranchID);
                parameters.Add("@ParentID", objMstDoor.ParentID);
                parameters.Add("@NodeNo", objMstDoor.NodeNo);
                parameters.Add("@NodeName", objMstDoor.NodeName);
                parameters.Add("@UserID", objMstDoor.UserID);
                // // parameters.Add("@Action", Action);
                parameters.Add("@NodeLevel", NodeLevel);

                var objReturnvalue = objDBConnection.connection.Query<MasterDoorViewModel>("[dbo].[USP_INSERT_MasterNode]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<MasterDoorViewModel> GetBuildingDetailsByName(string BuildingName, long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objManagers = objDBConnection.connection.Query<MasterDoorViewModel>("[dbo].[USP_GET_MasterDoorDetailsByBuildingName]", new { @BranchID = BranchID, @BuildingName = BuildingName },
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

        public bool CheckBuildingDetailsByName(string BuildingName, long BranchID)
        {
            try
            {
                List<MasterDoorViewModel> objResponse = GetBuildingDetailsByName(BuildingName, BranchID);

                if (objResponse == null && objResponse.Count == 0)
                {
                    return false;
                }
                else if (objResponse.Count == 0)
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

        public MasterDoorViewModel UpdateBuilding(MasterDoorViewModel objMstDoor, string Action, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BranchID", objMstDoor.BranchID);
                parameters.Add("@ParentID", objMstDoor.ParentID);
                parameters.Add("@NodeID", objMstDoor.NodeID);
                parameters.Add("@NodeName", objMstDoor.NodeName);
                parameters.Add("@UserID", objMstDoor.UserID);

                var objReturnvalue = objDBConnection.connection.Query<MasterDoorViewModel>("[dbo].[USP_UPDATE_MasterNode]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public MasterDoorViewModel GetBuildingDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objManagers = objDBConnection.connection.Query<MasterDoorViewModel>("[dbo].[USP_GET_BuildingDetailsByNodeID]", new { @NodeID = NodeID, @BranchID = BranchID, @ParentID = ParentID },
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

        public bool DeleteMasterDoorDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objUser = objDBConnection.connection.Query<string>("[dbo].[Usp_DELETE_MasterDoorDetailByNodeID]", new { @NodeID = NodeID, @BranchID = BranchID, @ParentID = ParentID },
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


    }
}
