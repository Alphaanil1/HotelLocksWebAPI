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
    public class RoomService
    {
        private RoomRepository objRoomRepository;
        IEmailService objEmailService = null;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Encrypt _encrypt = new Encrypt("alphaict2019");

        public RoomService(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objRoomRepository = new RoomRepository();
            objEmailService = emailService;
            this._jWTManager = jWTManager;
        }


        public ResultViewModel<List<RoomsViewModel>> CreateRoomList(CreateRoomList objRooms)
        {
            try
            {
                if (objRooms == null)
                {
                    return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(objRooms.BranchID) == null || objRooms.BranchID <= 0)
                {
                    return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidBranchID };
                }
                else if (Convert.ToString(objRooms.BuildingID) == null || objRooms.BuildingID <= 0)
                {
                    return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidBuildingID };
                }
                else if (Convert.ToString(objRooms.FloorID) == null || objRooms.FloorID <= 0)
                {
                    return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidFloorID };
                }
                else if (Convert.ToString(objRooms.TotalRooms) == null || objRooms.TotalRooms <= 0)
                {
                    return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide total room count" };
                }
                else if (!objRoomRepository.CheckRoomLimit(objRooms.BranchID, objRooms.FloorID, 1))
                {
                    return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.Room99LimitOver };
                }
                else
                {
                    if (objRooms.IsInnerRoom == true)
                    {
                        if (Convert.ToString(objRooms.ParentRoomNo) == null)
                        {
                            return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide parent room no." };
                        }
                        else if (Convert.ToString(objRooms.InnerRoomCount) == null || objRooms.InnerRoomCount <= 0)
                        {
                            return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Please provide total inner rooms count" };
                        }
                        else if (!objRoomRepository.CheckInnerRoomLimit(objRooms.BranchID, objRooms.FloorID, objRooms.ParentRoomNo, 2))
                        {
                            return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.Room99LimitOver };
                        }
                    }

                    List<RoomsViewModel> objResult = objRoomRepository.CreateRoomList(objRooms);

                    if (objResult == null)
                    {
                        return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        return new ResultViewModel<List<RoomsViewModel>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<RoomsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<RoomTypes>> GetRoomTypeListByBranchID(long BranchID)
        {
            try
            {
                if (Convert.ToString(BranchID) == null)
                {
                    return new ResultViewModel<List<RoomTypes>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }

                else
                {
                    List<RoomTypes> objResult = objRoomRepository.GetRoomTypeListByBranchID(BranchID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<List<RoomTypes>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        return new ResultViewModel<List<RoomTypes>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<RoomTypes>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<RoomTypes> AddUpdateRoomType(RoomTypes objRooms, string Action)
        {
            try
            {
                if (objRooms == null)
                {
                    return new ResultViewModel<RoomTypes> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<RoomTypes> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(objRooms.BranchID) == null || objRooms.BranchID <= 0)
                {
                    return new ResultViewModel<RoomTypes> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(objRooms.UserID) == null)
                {
                    return new ResultViewModel<RoomTypes> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (!objRoomRepository.CheckRoomType(objRooms.BranchID, objRooms.RoomType, Action, objRooms.RoomTypeID))
                {
                    return new ResultViewModel<RoomTypes> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RoomTypeExists };
                }
                else
                {

                    RoomTypes objResult = objRoomRepository.AddUpdateRoomType(objRooms, Action);
                    if (objResult != null)
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.RoomTypeAddSuccessMessage : UserMessage.RoomTypeEditSuccessMessage);
                        return new ResultViewModel<RoomTypes> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                    }
                    else
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.RoomTypeAddFailureMessage : UserMessage.RoomTypeEditFailureMessage);
                        return new ResultViewModel<RoomTypes> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<RoomTypes> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<string> AddRooms(List<RoomsViewModel> objRooms, string Action, long UserID)
        {
            try
            {
                if (objRooms == null)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(UserID) == null)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    long recordUpdatedCount = 0;
                    string returnUserMessage = "";

                    foreach (var rooms in objRooms)
                    {
                        if (Convert.ToString(rooms.NodeID) == null || rooms.NodeID <= 0)
                        {
                            returnUserMessage = UserMessage.provideValidFloorID;
                            break;
                        }
                        else if (Convert.ToString(rooms.BranchID) == null || rooms.BranchID <= 0)
                        {
                            returnUserMessage = UserMessage.provideValidBranchID; break;
                        }
                        else if (rooms.DoorNo == null || string.IsNullOrWhiteSpace(rooms.DoorNo))
                        {
                            returnUserMessage = "Please provide valid doorno"; break;
                        }
                        else if (rooms.DoorName == null || string.IsNullOrWhiteSpace(rooms.DoorName))
                        {
                            returnUserMessage = "Please provide valid door name"; break;
                        }
                        else
                        {
                            recordUpdatedCount++;
                        }
                    }

                    if (objRooms.Count == recordUpdatedCount)
                    {
                        string objResult = objRoomRepository.AddRooms(objRooms, Action, ((int)MasterDoorModule.Floor));
                        if (objResult != null && objResult == "Success")
                        {
                            string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.RoomAddSuccessMessage : UserMessage.RoomEditSuccessMessage);
                            return new ResultViewModel<string> { Result = returnMessage, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                        }
                        else
                        {
                            string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.RoomAddFailureMessage : UserMessage.RoomEditFailureMessage);
                            return new ResultViewModel<string> { Result = returnMessage, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<string> { Result = returnUserMessage, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnUserMessage };
                    }

                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<RoomsDetailsViewModel>> GetRoomDetailListByBranchID(long BranchID)
        {
            try
            {
                if (Convert.ToString(BranchID) == null)
                {
                    return new ResultViewModel<List<RoomsDetailsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }

                else
                {
                    List<RoomsDetailsViewModel> objResult = objRoomRepository.GetRoomDetailListByBranchID(BranchID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<List<RoomsDetailsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        return new ResultViewModel<List<RoomsDetailsViewModel>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<RoomsDetailsViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<string> UpdateRoomsName(long DoorID, long BranchID, long ParentID, string DoorName)
        {
            try
            {
                if (Convert.ToString(DoorID) == null || DoorID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(BranchID) == null || BranchID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.provideValidBranchID };
                }
                else if (Convert.ToString(ParentID) == null || ParentID <= 0)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Convert.ToString(DoorName) == null)
                {
                    return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = "Room Name is required." };
                }
                else
                {
                    string objResult = objRoomRepository.UpdateRoomsName(DoorID,BranchID,ParentID,DoorName);
                    if (objResult != null && objResult == "Success")
                    {
                        string returnMessage =  UserMessage.RoomEditSuccessMessage;
                        return new ResultViewModel<string> { Result = returnMessage, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                    }
                    else
                    {
                        string returnMessage =UserMessage.RoomEditFailureMessage;
                        return new ResultViewModel<string> { Result = returnMessage, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                    }

                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }




    }
}
