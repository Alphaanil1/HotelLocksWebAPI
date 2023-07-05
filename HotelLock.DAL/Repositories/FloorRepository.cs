using Dapper;
using HotelLock.BusinessObjects.Models;
using HotelLock.BusinessObjects.Models.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.DAL.Repositories
{
    public class FloorRepository
    {
        BuildingRepository objBuildingRepository = new BuildingRepository();
       
        public bool CheckFloorLimit(long BranchID, long BuildingID, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                List<FloorDetailViewModel> objResponse = GetFloorDetailByBuildingID(BranchID, BuildingID);
               
                if (objResponse == null && objResponse.Count == 0)
                {
                    return false;
                }
                else if (objResponse.Count >= 99)
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

        public MasterDoorViewModel AddFloor(MasterDoorViewModel objMstDoor, string Action, int NodeLevel)
        {           
            try
            {
                var objReturnvalue = objBuildingRepository.AddMasterDoorDetails(objMstDoor, Action, NodeLevel);               
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<FloorDetailViewModel> GetAllFloorDetailsByBranchID(long BranchID, long ParentID, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<FloorDetailViewModel>("[dbo].[USP_GET_MasterDoorDetails]", new { @BranchID = BranchID, @ParentID = ParentID, @NodeLevel = NodeLevel },
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objFloors;
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

        public MasterDoorViewModel GetFloorDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            try
            {
                var objfloors = objBuildingRepository.GetBuildingDetailByNodeID(NodeID, BranchID, ParentID);
                return objfloors;
            }
            catch (Exception ex)
            {
                throw (ex);
            }            
        }

        public bool CheckFloorDetailsByName(string FloorName,long ParentID, long BranchID)
        {
            try
            {
                List<MasterDoorViewModel> objResponse = GetFloorDetailsByName(FloorName, ParentID, BranchID);

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

        public List<MasterDoorViewModel> GetFloorDetailsByName(string BuildingName, long ParentID, long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objManagers = objDBConnection.connection.Query<MasterDoorViewModel>("[dbo].[USP_GET_MasterDoorDetailsByNodeName]", new { @BranchID = BranchID, @ParentID = ParentID, @NodeName = BuildingName },
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

        public MasterDoorViewModel UpdateFloor(FloorDetailViewModel objfloordtl, string Action, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BranchID", objfloordtl.BranchID);
                parameters.Add("@ParentID", objfloordtl.BuildingID);
                parameters.Add("@NodeID", objfloordtl.FloorID);
                parameters.Add("@NodeName", objfloordtl.FloorName);
                parameters.Add("@UserID", objfloordtl.UserID);

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

        public bool DeleteFloorDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            try
            {
                var objfloors = objBuildingRepository.DeleteMasterDoorDetailByNodeID(NodeID, BranchID, ParentID);
                return objfloors;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool CheckZoneLimit(long BranchID, long FloorID)
        {
            try
            {
                List<FloorDetailViewModel> objResponse = GetZoneDetailByFloorID(BranchID, FloorID);

                if (objResponse == null && objResponse.Count == 0)
                {
                    return false;
                }
                else if (objResponse.Count >= 9)
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

        public MasterDoorViewModel AddZone(MasterDoorViewModel objMstDoor, string Action, int NodeLevel)
        {
            try
            {
                var objReturnvalue = objBuildingRepository.AddMasterDoorDetails(objMstDoor, Action, NodeLevel);
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<FloorDetailViewModel> GetFloorDetailByBuildingID(long BranchID, long BuildingID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<FloorDetailViewModel>("[dbo].[USP_GET_FloorDetailsByBuildingID]", new { @BranchID = BranchID, @BuildingID = BuildingID },
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objFloors;
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

        public List<FloorDetailViewModel> GetZoneDetailByFloorID(long BranchID, long FloorID,long NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<FloorDetailViewModel>("[dbo].[USP_GET_MasterDoorDetails]", new { @BranchID = BranchID, @ParentID = FloorID, @NodeLevel = NodeLevel },
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objFloors;
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

        public bool CheckZonerDetailsByName(string ZoneName, long FloorID, long BranchID)
        {
            try
            {
                List<MasterDoorViewModel> objResponse = GetFloorDetailsByName(ZoneName, FloorID, BranchID);

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

        public List<MasterDoorViewModel> UpdateZone(List<UpdateZoneDetailModel> objZonedtl, string Action, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                XMLOperation objXMLOperation = new XMLOperation();
                string strXml = objXMLOperation.ObjectToXmlConverter(objZonedtl, "utf-8");

                parameters.Add("@strXml", strXml);

                var objReturnvalue = objDBConnection.connection.Query<MasterDoorViewModel>("[dbo].[USP_UPDATE_ZoneDetails]", parameters,
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<FloorDetailViewModel> GetZoneDetailByFloorID(long BranchID, long FloorID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<FloorDetailViewModel>("[dbo].[USP_GET_ZoneDetailsByFloorID]", new { @BranchID = BranchID, @FloorID = FloorID },
                commandType: CommandType.StoredProcedure).ToList();

                objDBConnection.CloseConnection();
                return objFloors;
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

        public bool DeleteZoneDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            try
            {
                var objfloors = objBuildingRepository.DeleteMasterDoorDetailByNodeID(NodeID, BranchID, ParentID);
                return objfloors;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
