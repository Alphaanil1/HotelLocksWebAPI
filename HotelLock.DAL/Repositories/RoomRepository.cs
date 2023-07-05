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
    public class RoomRepository
    {
        public List<RoomsViewModel> GetRoomDetailByFloorID(long BranchID, long FloorID, string ParentRoomNo, long NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<RoomsViewModel>("[dbo].[USP_GET_RoomsDetails]", new { @BranchID = BranchID, @FloorID = FloorID, @ParentRoomNo= ParentRoomNo, @NodeLevel = NodeLevel },
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

        public bool CheckRoomLimit(long BranchID, long FloorID, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                List<RoomsViewModel> objResponse = GetRoomDetailByFloorID(BranchID, FloorID,string.Empty, NodeLevel);

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

        public bool CheckInnerRoomLimit(long BranchID, long FloorID,string ParentRoomNo, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                List<RoomsViewModel> objResponse = GetRoomDetailByFloorID(BranchID, FloorID, ParentRoomNo, NodeLevel);

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

        public List<RoomsViewModel> CreateRoomList(CreateRoomList objRooms)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BranchID", objRooms.BranchID);
                parameters.Add("@BuildingID", objRooms.BuildingID);
                parameters.Add("@FloorID", objRooms.FloorID);
                parameters.Add("@TotalRooms", objRooms.TotalRooms);
                parameters.Add("@IsInnerRoom", objRooms.IsInnerRoom);
                parameters.Add("@ParentRoomNo", objRooms.ParentRoomNo);
                parameters.Add("@InnerRoomCount", objRooms.InnerRoomCount);

                var objFloors = objDBConnection.connection.Query<RoomsViewModel>("[dbo].[USP_GET_CreateRoomsList]", parameters,
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

        public List<RoomTypes> GetRoomTypeListByBranchID(long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<RoomTypes>("[dbo].[USP_GET_RoomTypeDetailsByBranchID]", new { @BranchID = BranchID },
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

        public List<RoomTypes> GetRoomTypeListByRoomType(long BranchID, string RoomType,string Action, long RoomTypeID = 0)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<RoomTypes>("[dbo].[USP_GET_RoomTypeDetailsByRoomType]", new { @BranchID = BranchID, @RoomType = RoomType, @RoomTypeID=RoomTypeID, @Action = Action },
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

        public bool CheckRoomType(long BranchID, string RoomType, string Action, long RoomTypeID = 0)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                List<RoomTypes> objResponse = GetRoomTypeListByRoomType(BranchID, RoomType, Action, RoomTypeID);

                if (objResponse == null && objResponse.Count == 0)
                {
                    return false;
                }
                else if (objResponse.Count > 0)
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

        public RoomTypes AddUpdateRoomType(RoomTypes objRooms, string Action)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@BranchID", objRooms.BranchID);
                parameters.Add("@RoomTypeID", objRooms.RoomTypeID);
                parameters.Add("@RoomType", objRooms.RoomType);
                parameters.Add("@UserID", objRooms.UserID);
                parameters.Add("@Action", Action);

                var objReturnvalue = objDBConnection.connection.Query<RoomTypes>("[dbo].[USP_INSERTUPDATE_RoomType]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public string AddRooms(List<RoomsViewModel> objRooms, string Action, int NodeLevel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                XMLOperation objXMLOperation = new XMLOperation();
                string strXml = objXMLOperation.ObjectToXmlConverter(objRooms, "utf-8");

                parameters.Add("@strXml", strXml);

                var objReturnvalue = objDBConnection.connection.Query<string>("[dbo].[USP_Insert_RoomsDetails]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<RoomsDetailsViewModel> GetRoomDetailListByBranchID(long BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objFloors = objDBConnection.connection.Query<RoomsDetailsViewModel>("[dbo].[USP_GET_RoomDetailsByBranchID]", new { @BranchID = BranchID },
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

        public string UpdateRoomsName(long DoorID,long BranchID,long ParentID, string DoorName)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                
                parameters.Add("@DoorID", DoorID);
                parameters.Add("@BranchID", BranchID);
                parameters.Add("@ParentID", ParentID);
                parameters.Add("@DoorName", DoorName);

                var objReturnvalue = objDBConnection.connection.Query<string>("[dbo].[USP_UPDATE_RoomName]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

    }
}
