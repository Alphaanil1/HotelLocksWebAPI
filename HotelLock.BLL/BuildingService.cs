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
    public class BuildingService
    {
        private BuildingRepository objBuildingRepository;
        IEmailService objEmailService = null;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Encrypt _encrypt = new Encrypt("alphaict2019");

        public BuildingService(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objBuildingRepository = new BuildingRepository();
            objEmailService = emailService;
            this._jWTManager = jWTManager;
        }

        public ResultViewModel<MasterDoorViewModel> AddBuilding(long BranchID, string Action, int UserID)
        {
            try
            {
                if (BranchID == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (UserID == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (!objBuildingRepository.CheckBuildingLimit(BranchID, 1, ((int)MasterDoorModule.Building)))
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.Building26LimitOver };
                }
                else
                {
                    MasterDoorViewModel ObjmasterDoor = new MasterDoorViewModel
                    {
                        BranchID = BranchID,
                        UserID = UserID,
                    };
                    MasterDoorViewModel objResult = objBuildingRepository.AddMasterDoorDetails(ObjmasterDoor, Action, ((int)MasterDoorModule.Building));
                    if (objResult != null)
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.BuildingAddSuccessMessage : UserMessage.BuildingEditSuccessMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                    }
                    else
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.BuildingAddFailureMessage : UserMessage.BuildingEditFailureMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<MasterDoorViewModel> UpdateBuildingName(MasterDoorViewModel objmasterDoor, string Action)
        {
            try
            {
                if (objmasterDoor == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (objmasterDoor.NodeID == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (objmasterDoor.NodeName == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (Action == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (objmasterDoor.UserID == null)
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else if (objBuildingRepository.CheckBuildingDetailsByName(objmasterDoor.NodeName, objmasterDoor.BranchID))
                {
                    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.BuildingExists };
                }
                //else if (!objBuildingRepository.CheckBuildingLimit(objmasterDoor.BranchID, 1, ((int)MasterDoorModule.Building)))
                //{
                //    return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.Building26LimitOver };
                //}
                else
                {
                    //MasterDoorViewModel ObjmasterDoor = new MasterDoorViewModel
                    //{
                    //    NodeID=NodeID,
                    //    NodeName=BuildingName,
                    //    BranchID = BranchID,
                    //    UserID = UserID,
                    //};

                    MasterDoorViewModel objResult = objBuildingRepository.UpdateBuilding(objmasterDoor, Action, ((int)MasterDoorModule.Building));

                    if (objResult != null)
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.BuildingAddSuccessMessage : UserMessage.BuildingEditSuccessMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                    }
                    else
                    {
                        string returnMessage = Convert.ToString(Action == "Add" ? UserMessage.BuildingAddFailureMessage : UserMessage.BuildingEditFailureMessage);
                        return new ResultViewModel<MasterDoorViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<MasterDoorViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<List<MasterDoorViewModel>> GetBuildingDetailList(long BranchID)
        {
            try
            {
                if (BranchID == null)
                {
                    return new ResultViewModel<List<MasterDoorViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    List<MasterDoorViewModel> objResult = objBuildingRepository.GetAllBuildingDetailsByBranchID(BranchID, 0, ((int)MasterDoorModule.Building));

                    if (objResult == null)
                    {
                        return new ResultViewModel<List<MasterDoorViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        return new ResultViewModel<List<MasterDoorViewModel>> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = Message.Success.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<List<MasterDoorViewModel>> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }

        public ResultViewModel<MasterDoorViewModel> GetBuildingDetailByNodeID(long NodeID, long BranchID, long ParentID)
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

                    MasterDoorViewModel objResult = objBuildingRepository.GetBuildingDetailByNodeID(NodeID, BranchID, ParentID);
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

        public ResultViewModel<string> DeleteBuildingDetailByNodeID(long NodeID, long BranchID, long ParentID)
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
                    MasterDoorViewModel objResult = objBuildingRepository.GetBuildingDetailByNodeID(NodeID, BranchID, ParentID);

                    if (objResult == null)
                    {
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.RecordNotFoundMessage };
                    }
                    else
                    {
                        objBuildingRepository.DeleteMasterDoorDetailByNodeID(NodeID, BranchID, ParentID);
                        return new ResultViewModel<string> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = UserMessage.BuildingDeletedMessage };
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
