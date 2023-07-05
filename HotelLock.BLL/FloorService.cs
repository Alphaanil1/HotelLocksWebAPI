using HotelLock.BusinessObjects.Models;
using HotelLock.BusinessObjects.Models.Utility;
using HotelLock.DAL.Repositories;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BLL
{
    public class FloorService
    {
        private FloorRepository objFloorRepository;
        IEmailService objEmailService = null;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Encrypt _encrypt = new Encrypt("alphaict2019");

        public FloorService(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objFloorRepository = new FloorRepository();
            objEmailService = emailService;
            this._jWTManager = jWTManager;
        }

        public ResultViewModel<MasterDoorViewModel> AddFloor(FloorDetailViewModel objmasterDoor, string Action)
        {
            try
            {
                if (objmasterDoor == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(objmasterDoor.UserID) == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (!objFloorRepository.CheckFloorLimit(objmasterDoor.BranchID, objmasterDoor.BuildingID, ((int)MasterDoorModule.Floor)))
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.Floor99LimitOver };
                }
                else
                {
                    MasterDoorViewModel ObjmasterDoor = new MasterDoorViewModel
                    {
                        BranchID = objmasterDoor.BranchID,
                        ParentID = objmasterDoor.BuildingID,
                        UserID = objmasterDoor.UserID,
                    };
                    MasterDoorViewModel objResult = objFloorRepository.AddFloor(ObjmasterDoor, Action, ((int)MasterDoorModule.Floor));
                    if (objResult != null)
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.FloorAddSuccessMessage : UserMessage.FloorEditSuccessMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                    }
                    else
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.FloorAddFailureMessage : UserMessage.FloorEditFailureMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<FloorZoneDetailViewModel>> GetFloorDetailList(long BranchID, long ParentID)
        {
            try
            {
                if (Convert.ToString(BranchID) == null)
                {
                    return new ResultViewModel<List<FloorZoneDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(ParentID) == null)
                {
                    return new ResultViewModel<List<FloorZoneDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    List<FloorDetailViewModel> objResult = objFloorRepository.GetAllFloorDetailsByBranchID(BranchID, ParentID, ((int)MasterDoorModule.Floor));

                    List<FloorZoneDetailViewModel> objFloorZoneDetail = new List<FloorZoneDetailViewModel>();

                    if (objResult == null)
                    {
                        return new ResultViewModel<List<FloorZoneDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        objFloorZoneDetail = ConvertFloorZoneDetail(objResult);
                        return new ResultViewModel<List<FloorZoneDetailViewModel>> { Result = objFloorZoneDetail, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<FloorZoneDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<MasterDoorViewModel> GetFloorDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            try
            {
                if (NodeID <= 0)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid Node ID" };
                }
                else if (BranchID <= 0)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid branch detail" };
                }
                else if (ParentID <= 0)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid parent ID" };
                }
                else
                {

                    MasterDoorViewModel objResult = objFloorRepository.GetFloorDetailByNodeID(NodeID, BranchID, ParentID);
                    if (objResult == null)
                    {
                        return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {

                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<MasterDoorViewModel> UpdateFloorName(FloorDetailViewModel objfloordtl, string Action)
        {
            try
            {
                if (objfloordtl == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(objfloordtl.FloorID) == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidFloorID };
                }
                else if (Convert.ToString(objfloordtl.BranchID) == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidBranchID };
                }
                else if (Convert.ToString(objfloordtl.BuildingID) == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidBuildingID };
                }
                else if (objfloordtl.FloorName == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid floor name" };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(objfloordtl.UserID) == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidUserID };
                }
                else if (objFloorRepository.CheckFloorDetailsByName(objfloordtl.FloorName, objfloordtl.BuildingID, objfloordtl.BranchID))
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.FloorExists };
                }
                else
                {

                    MasterDoorViewModel objResult = objFloorRepository.UpdateFloor(objfloordtl, Action, ((int)MasterDoorModule.Floor));

                    if (objResult != null)
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.FloorAddSuccessMessage : UserMessage.FloorEditSuccessMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                    }
                    else
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.FloorAddFailureMessage : UserMessage.FloorEditFailureMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<string> DeleteFloorDetailByNodeID(long NodeID, long BranchID, long ParentID)
        {
            try
            {
                if (NodeID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid Node ID" };
                }
                else if (BranchID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid branch detail" };
                }
                else if (ParentID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide a valid parent ID" };
                }
                else
                {
                    MasterDoorViewModel objResult = objFloorRepository.GetFloorDetailByNodeID(NodeID, BranchID, ParentID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        objFloorRepository.DeleteFloorDetailByNodeID(NodeID, BranchID, ParentID);
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.FloorDeletedMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<MasterDoorViewModel> AddZone(FloorDetailViewModel objmasterDoor, string Action)
        {
            try
            {
                if (objmasterDoor == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(objmasterDoor.UserID) == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidUserID };
                }
                else if (Convert.ToString(objmasterDoor.BranchID) == null || objmasterDoor.BranchID <= 0)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidBranchID };
                }
                else if (Convert.ToString(objmasterDoor.FloorID) == null || objmasterDoor.FloorID <= 0)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidFloorID };
                }
                else if (!objFloorRepository.CheckZoneLimit(objmasterDoor.BranchID, objmasterDoor.FloorID))
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.Floor99LimitOver };
                }
                else
                {
                    MasterDoorViewModel ObjmasterDoor = new MasterDoorViewModel
                    {
                        BranchID = objmasterDoor.BranchID,
                        ParentID = objmasterDoor.FloorID,
                        UserID = objmasterDoor.UserID,
                    };
                    MasterDoorViewModel objResult = objFloorRepository.AddZone(ObjmasterDoor, Action, ((int)MasterDoorModule.Zone));
                    if (objResult != null)
                    {
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.ZoneAddSuccessMessage };
                    }
                    else
                    {
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = UserMessage.ZoneAddFailureMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<FloorDetailViewModel>> GetZoneDetailByFloorID(long BranchID, long FloorID)
        {
            try
            {
                if (BranchID <= 0)
                {
                    return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidBranchID };
                }
                else if (FloorID <= 0)
                {
                    return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidFloorID };
                }
                else
                {
                    List<FloorDetailViewModel> objResult = objFloorRepository.GetZoneDetailByFloorID(BranchID, FloorID, (int)MasterDoorModule.Zone);
                    if (objResult == null)
                    {
                        return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {

                        return new ResultViewModel<List<FloorDetailViewModel>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<string> UpdateZoneName(List<UpdateZoneDetailModel> objZonedtl, string Action)
        {
            try
            {
                if (objZonedtl == null)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    long recordUpdatedCount = 0;
                    string returnUserMessage = "";

                    foreach (var floors in objZonedtl)
                    {
                        if (Convert.ToString(floors.FloorID) == null || floors.FloorID <= 0)
                        {
                            returnUserMessage = UserMessage.provideValidFloorID;
                            break;
                        }
                        else if (Convert.ToString(floors.BranchID) == null || floors.BranchID <= 0)
                        {
                            returnUserMessage = UserMessage.provideValidBranchID; break;
                        }
                        else if (Convert.ToString(floors.ZoneID) == null || floors.ZoneID <= 0)
                        {
                            returnUserMessage = UserMessage.provideValidZoneID; break;
                        }
                        else if (floors.ZoneName == null)
                        {
                            returnUserMessage = "Please provide a valid Zone name"; break;
                        }
                        else if (Convert.ToString(floors.UserID) == null || floors.UserID <= 0)
                        {
                            returnUserMessage = UserMessage.provideValidUserID; break;
                        }
                        else if (objFloorRepository.CheckZonerDetailsByName(floors.ZoneName, floors.FloorID, floors.BranchID))
                        {
                            returnUserMessage = UserMessage.FloorExists; break;
                        }
                        else
                        {
                            recordUpdatedCount++;
                        }
                    }

                    if (objZonedtl.Count == recordUpdatedCount)
                    {
                        List<MasterDoorViewModel> objResult = objFloorRepository.UpdateZone(objZonedtl, Action, ((int)MasterDoorModule.Floor));
                        if (objResult != null && objResult.Count > 0)
                        { returnUserMessage = UserMessage.ZoneEditSuccessMessage; }
                        else
                        { returnUserMessage = UserMessage.ZoneEditFailureMessage; }

                        return new ResultViewModel<string> { Result = returnUserMessage, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = string.IsNullOrWhiteSpace(returnUserMessage) ? UserMessage.ZoneEditSuccessMessage : returnUserMessage };
                    }
                    else
                    {
                        return new ResultViewModel<string> { Result = returnUserMessage, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = string.IsNullOrWhiteSpace(returnUserMessage) ? UserMessage.ZoneEditFailureMessage : returnUserMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<FloorDetailViewModel>> GetFloorDetailListByBuildingID(long BranchID, long ParentID)
        {
            try
            {
                if (Convert.ToString(BranchID) == null)
                {
                    return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(ParentID) == null)
                {
                    return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    List<FloorDetailViewModel> objResult = objFloorRepository.GetFloorDetailByBuildingID(BranchID, ParentID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        return new ResultViewModel<List<FloorDetailViewModel>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<FloorDetailViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }


        public List<FloorZoneDetailViewModel> ConvertFloorZoneDetail(List<FloorDetailViewModel> floorZoneViewModelResult)
        {
            try
            {

                //----------------------------------------------------------------------------
                var floorslist = from floors in floorZoneViewModelResult
                                 select new
                                 {
                                     floors.BranchID,
                                     floors.BranchName,
                                     floors.BuildingID,
                                     floors.BuildingParentID,
                                     floors.BuildingNo,
                                     floors.BuildingName,
                                     floors.FloorID,
                                     floors.FloorParentID,
                                     floors.FloorNo,
                                     floors.FloorName
                                 };

                //--------------------------------------------------------------------------------
                var distinctFloorslist = floorslist.Distinct().ToList();

                List<FloorZoneDetailViewModel> ObjFloorZoneDetail = new List<FloorZoneDetailViewModel>();
                foreach (var item in distinctFloorslist)
                {
                    FloorZoneDetailViewModel objFloorZone = new FloorZoneDetailViewModel();
                    objFloorZone.BranchID = item.BranchID;
                    objFloorZone.BranchName = item.BranchName;
                    objFloorZone.BuildingID = item.BuildingID;
                    objFloorZone.BuildingParentID = item.BuildingParentID;
                    objFloorZone.BuildingNo = item.BuildingNo;
                    objFloorZone.BuildingName = item.BuildingName;
                    objFloorZone.FloorID = item.FloorID;
                    objFloorZone.FloorParentID = item.FloorParentID;
                    objFloorZone.FloorNo = item.FloorNo;
                    objFloorZone.FloorName = item.FloorName;



                    List<ZoneDetailsViewModel> zoneDetailsList = new List<ZoneDetailsViewModel>();

                    var query3 = floorZoneViewModelResult.Where(e => e.FloorID == item.FloorID && e.BranchID == item.BranchID).ToList();
                    foreach (var val in query3)
                    {
                        ZoneDetailsViewModel objZoneDetails = new ZoneDetailsViewModel()
                        {
                            ZoneID          = val.ZoneID,
                            ZoneParentID    = val.ZoneParentID,
                            ZoneNo          = val.ZoneNo,    
                            ZoneName        = val.ZoneName    
                        };
                        zoneDetailsList.Add(objZoneDetails);
                    }
                    objFloorZone.zoneDetails = zoneDetailsList;

                    ObjFloorZoneDetail.Add(objFloorZone);
                }

                return ObjFloorZoneDetail;
            }
            catch
            {
                throw;
            }
        }
    }
}
