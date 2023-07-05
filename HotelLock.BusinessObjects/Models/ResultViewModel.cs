using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace HotelLock.BusinessObjects.Models
{

    public class ResultViewModel<T>
    {
        public T Result { get; set; }
        public HttpStatusCode ResponseCode { get; set; }
        public string Message { get; set; }
        public string UserMessage { get; set; }

        public ResultViewModel()
        {
        }
        public ResultViewModel(T result, string message, string userMessage)
        {
            this.Result = result;
            this.Message = message;
            this.UserMessage = userMessage;
        }
        public ResultViewModel(T result, HttpStatusCode responseCode, string message, string userMessage)
        {
            this.Result = result;
            this.ResponseCode = responseCode;
            this.Message = message;
            this.UserMessage = userMessage;
        }

    }

    public enum Message
    {
        Success,
        NotFound,
        AlreadyExists,
        Exception,
        Failure,
        GatewayConnectionFailed,
        DataBaseError,
        NoData,
        unauthorized,
        TimeOut
    }

    public enum MasterDoorModule
    {
        Hotel = -1,
        Building = 1,
        Floor = 2,
        Zone = 3,
        Room = 4,
        InnerRoom = 5
    }
    public class UserMessage
    {
        public static string RecordSavedMessage { get => "Record saved successfully"; }
        public static string RecordUpdatedMessage { get => "Record updated successfully"; }
        public static string RecordDeletedMessage { get => "Record deleted successfully"; }
        public static string RecordNotFoundMessage { get => "Record not found"; }
        public static string BranchdetailNotFoundMessage { get => "Branch details not found"; }
        public static string RecordAlreadyExistsMessage { get => "Record already exists..!!"; }
        public static string Success { get => "Query successfuly executed..!!"; }
        public static string InvalidData { get => "Data is not valid..!!"; }
        public static string UserNotExists { get => "UserName does not exists"; }
        public static string UserExists { get => "UserName already exists"; }
        public static string EMAIL_LeaveApplication_SUBJECT { get => " Leave Application"; }
        public static string ContentLocation { get => "https://nextgenapi.alpha-ict.in/HRMSContent/"; }
        public static string BaseUrl { get => "https://nextgenapi.alpha-ict.in/HRMSContent/"; }
        public static string LeaveAppliedSuccess { get => "Leave applied successfully but manager not assigned yet, hence mail not sent"; }
        public static string LeaveApprovedSuccess { get => "Leave approved successfully but employee not assigned yet, hence mail not sent"; }
        public static string EmployeeNotExists { get => "Employee does not exists"; }
        public static string EmployeeExists { get => "Employee already exists"; }
        public static string RecordNotDeletedMessage { get => "Record not deleted."; }
        public static string LeaveDeletedSuccess { get => "Leave deleted successfully but manager not assigned yet, hence mail not sent"; }
        public static string NewsEventSavedMessage { get => "News/Event saved successfully"; }
        public static string NewsEventUpdatedMessage { get => "News/Event updated successfully"; }
        public static string NewsEventDeletedMessage { get => "News/Event deleted successfully"; }
        public static string BranchDetailsRequireMessage { get => "Branch detail required"; }
        public static string UserCodeRequireMessage { get => "User code is required"; }
        public static string UserdetailNotFoundMessage { get => "User details not found"; }
        public static string UserdoesNotExistsMessage { get => "User does not exists"; }
        public static string LoginSuccessMessage { get => "Login successfull"; }
        public static string BlankUserNamePasswordMessage { get => "Blank UserName or Password not allowed"; }
        public static string UnauthorizedUserMessage { get => "Unauthorized user"; }
        public static string ConfirmPasswordRequireMessage { get => "Confirm Password required"; }
        public static string PasswordRequireMessage { get => "Password is required"; }
        public static string OldPasswordNotMatchMessage { get => "Old Password does not match"; }
        public static string PasswordConfirmPasswordNotMatchMessage { get => "Password and confirm password doesn't match"; }
        public static string PasswordFormatChangeMessage { get => "Required atleast one lowercase, one uppercase letter, one special character, one digit and minimum length should 8 characters "; }
        public static string PasswordChangeSuccessMessage { get => "Password changed successfully"; }
        public static string PasswordChangeFailureMessage { get => "Password not changed"; }
        public static string BranchdoesNotExistsMessage { get => "Branch does not exists"; }
        public static string ForgotPasswordEmailSuccessMessage { get => "Temporary password has been sent to registered email Id."; }
        public static string ForgotPasswordSetFailureMessage { get => "Error occured to set temporary password."; }
        public static string ForgotPasswordEmailFailureMessage { get => "Error occured while sending email"; }
        public static string UserAddSuccessMessage { get => "User created successfully. and Password has been sent on registered email Id."; }
        public static string UserEditSuccessMessage { get => "User updated successfully."; }
        public static string UserDeletedMessage { get => "User deleted successfully"; }
        public static string UserMailSendFailMessage { get => "User create successfull but Email not send."; }

        public static string CompanyAddSuccessMessage { get => "Company created successfully."; }
        public static string CompanyEditSuccessMessage { get => "Company updated successfully."; }
        public static string CompanyAddFailureMessage { get => "Error occured while adding company details. try again.!"; }
        public static string CompanyEditFailureMessage { get => "Error occured while updating company details. try again.!"; }
        public static string CompanyDeletedMessage { get => "Company deleted successfully"; }
        public static string CompanyDeleteFailureMessage { get => "Error occured while deleting company details"; }

        public static string BranchExists { get => "BranchName already exists"; }
        public static string BranchAddSuccessMessage { get => "Branch created successfully."; }
        public static string BranchEditSuccessMessage { get => "Branch updated successfully."; }
        public static string BranchAddFailureMessage { get => "Error occured while adding branch details. try again.!"; }
        public static string BranchEditFailureMessage { get => "Error occured while updating branch details. try again.!"; }
        public static string BranchDeletedMessage { get => "Branch deleted successfully"; }
        public static string BranchDeleteFailureMessage { get => "Error occured while deleting branch details"; }

        public static string Building26LimitOver { get => "You can not create more than 26 building."; }
        public static string BuildingExists { get => "Building name already exists"; }
        public static string BuildingAddSuccessMessage { get => "Building created successfully."; }
        public static string BuildingEditSuccessMessage { get => "Building updated successfully."; }
        public static string BuildingAddFailureMessage { get => "Error occured while adding building details. try again.!"; }
        public static string BuildingEditFailureMessage { get => "Error occured while updating building details. try again.!"; }
        public static string BuildingDeletedMessage { get => "Building deleted successfully"; }
        public static string BuildingDeleteFailureMessage { get => "Error occured while deleting Building details"; }

        public static string Floor99LimitOver { get => "You can not create more than 99 floor."; }
        public static string FloorExists { get => "Floor name already exists"; }
        public static string FloorAddSuccessMessage { get => "Floor created successfully."; }
        public static string FloorEditSuccessMessage { get => "Floor updated successfully."; }
        public static string FloorAddFailureMessage { get => "Error occured while adding floor details. try again.!"; }
        public static string FloorEditFailureMessage { get => "Error occured while updating floor details. try again.!"; }
        public static string FloorDeletedMessage { get => "Floor deleted successfully"; }
        public static string FloorDeleteFailureMessage { get => "Error occured while deleting floor details"; }

        public static string provideValidUserID { get => "Please provide a valid user id"; }
        public static string provideValidFloorID { get => "Please provide a valid floor id"; }
        public static string provideValidBranchID { get => "Please provide a valid branch id"; }
        public static string provideValidBuildingID { get => "Please provide a valid building id"; }
        public static string provideValidZoneID { get => "Please provide a valid zone id"; }

        public static string Zone9LimitOver { get => "You can not create more than 9 zone."; }
        public static string ZoneExists { get => "Zone name already exists"; }
        public static string ZoneAddSuccessMessage { get => "Zone created successfully."; }
        public static string ZoneEditSuccessMessage { get => "Zone updated successfully."; }
        public static string ZoneAddFailureMessage { get => "Error occured while adding zone details. try again.!"; }
        public static string ZoneEditFailureMessage { get => "Error occured while updating zone details. try again.!"; }
        public static string ZoneDeletedMessage { get => "Zone deleted successfully"; }
        public static string ZoneDeleteFailureMessage { get => "Error occured while deleting zone details"; }

        public static string Room99LimitOver { get => "You can not create more than 99 rooms."; }
        public static string RoomTypeExists { get => "Room type name already exists"; }
        public static string RoomTypeAddSuccessMessage { get => "Room type created successfully."; }
        public static string RoomTypeEditSuccessMessage { get => "Room type updated successfully."; }
        public static string RoomTypeAddFailureMessage { get => "Error occured while adding room type details. try again.!"; }
        public static string RoomTypeEditFailureMessage { get => "Error occured while updating room type details. try again.!"; }
        public static string RoomTypeDeletedMessage { get => "Room type deleted successfully"; }
        public static string RoomTypeDeleteFailureMessage { get => "Error occured while deleting room type details"; }


        public static string RoomAddSuccessMessage { get => "Rooms created successfully."; }
        public static string RoomEditSuccessMessage { get => "Rooms updated successfully."; }
        public static string RoomAddFailureMessage { get => "Error occured while adding rooms details. try again.!"; }
        public static string RoomEditFailureMessage { get => "Error occured while updating rooms details. try again.!"; }
        public static string RoomDeletedMessage { get => "Rooms deleted successfully"; }
        public static string RoomDeleteFailureMessage { get => "Error occured while deleting rooms details"; }


        //public static string BranchExists { get => "BranchName already exists"; }
        public static string GuestInformationAddSuccessMessage { get => "Guest information added successfully."; }
        public static string GuestInformationEditSuccessMessage { get => "Guest information updated successfully."; }
        public static string GuestInformationAddFailureMessage { get => "Error occured while adding guest information. try again.!"; }
        public static string GuestInformationEditFailureMessage { get => "Error occured while updating guest information. try again.!"; }
        //public static string BranchDeletedMessage { get => "Branch deleted successfully"; }
        //public static string BranchDeleteFailureMessage { get => "Error occured while deleting branch details"; }
    }




}
