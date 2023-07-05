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
    public class GuestInformationRepository
    {

        public string GenerateCardno()
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {

                var objReturnvalue = objDBConnection.connection.Query<string>("SELECT ISNULL(MAX(CardNo), 0) As [CardNo] FROM IssueCards",
                commandType: CommandType.Text).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }
            catch
            {
                throw;
            }
        }
        public bool CheckCardNumber(string CardNumber, int BranchID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objReturnvalue = objDBConnection.connection.Query<int>("SELECT count(*) FROM IssueCards WHERE CardNo = '" + CardNumber + "' AND BranchID = " + BranchID,
                commandType: CommandType.Text).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue > 0 ? true : false;
            }
            catch
            {
                throw;
            }
        }


        public bool ChangeReservationStatus(int ReservationID)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                var objReturnvalue = objDBConnection.connection.Query<int>("UPDATE ReservationDoor SET ReservationComplete = 1 where ReservationDoorID = "+ ReservationID,
                commandType: CommandType.Text).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue > 0 ? true : false;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //-------------Guest Card----------------------------------
        public GuestInformationViewModel InsertGuestCard(GuestInformationViewModel objGuestInformationViewModel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@BranchId", objGuestInformationViewModel.BranchID);
                parameters.Add("@GuestID", objGuestInformationViewModel.GuestID);
                parameters.Add("@GuestName", objGuestInformationViewModel.GuestName);
                parameters.Add("@Address", objGuestInformationViewModel.Address);
                parameters.Add("@Sex", objGuestInformationViewModel.Sex == "Male" ? 'M' : 'F');
                parameters.Add("@Married", objGuestInformationViewModel.Married);
                parameters.Add("@BirthDate", objGuestInformationViewModel.BirthDate);
                parameters.Add("@Nationality", objGuestInformationViewModel.Nationality);
                parameters.Add("@DocumentID", objGuestInformationViewModel.DocumentID);
                parameters.Add("@DocumentNumber", objGuestInformationViewModel.DocumentNumber);
                parameters.Add("@ZipCode", objGuestInformationViewModel.ZipCode);
                parameters.Add("@MobileNo", objGuestInformationViewModel.MobileNo);
                parameters.Add("@FaxNo", objGuestInformationViewModel.FaxNo);
                parameters.Add("@EmailID", objGuestInformationViewModel.EmailID);
                parameters.Add("@CheckOutStatus", objGuestInformationViewModel.CheckOutStatus);
                parameters.Add("@ModifiedBy", objGuestInformationViewModel.ModifiedBy);

                var objReturnvalue = objDBConnection.connection.Query<GuestInformationViewModel>("[dbo].[Usp_Insert_GuestInfo]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public GuestInformationViewModel ModifyGuestCard(GuestInformationViewModel objGuestInformationViewModel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@BranchId", objGuestInformationViewModel.BranchID);
                parameters.Add("@GuestID", objGuestInformationViewModel.GuestID);
                parameters.Add("@GuestName", objGuestInformationViewModel.GuestName);
                parameters.Add("@Address", objGuestInformationViewModel.Address);
                parameters.Add("@Sex", objGuestInformationViewModel.Sex == "Male" ? 'M' : 'F');
                parameters.Add("@Married", objGuestInformationViewModel.Married);
                parameters.Add("@BirthDate", objGuestInformationViewModel.BirthDate);
                parameters.Add("@Nationality", objGuestInformationViewModel.Nationality);
                parameters.Add("@DocumentID", objGuestInformationViewModel.DocumentID);
                parameters.Add("@DocumentNumber", objGuestInformationViewModel.DocumentNumber);
                parameters.Add("@ZipCode", objGuestInformationViewModel.ZipCode);
                parameters.Add("@MobileNo", objGuestInformationViewModel.MobileNo);
                parameters.Add("@FaxNo", objGuestInformationViewModel.FaxNo);
                parameters.Add("@EmailID", objGuestInformationViewModel.EmailID);
                parameters.Add("@CheckOutStatus", objGuestInformationViewModel.CheckOutStatus);
                parameters.Add("@ModifiedBy", objGuestInformationViewModel.ModifiedBy);


                var objReturnvalue = objDBConnection.connection.Query<GuestInformationViewModel>("Usp_Update_GuestInfo", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public ModifyGuestCardStatusViewModel ModifyGuestCardStatus(ModifyGuestCardStatusViewModel objUpdateGuestStatusViewModel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@GuestID", objUpdateGuestStatusViewModel.GuestID);
                parameters.Add("@CheckOutStatus", objUpdateGuestStatusViewModel.CheckOutStatus);
                parameters.Add("@CheckOutDate", objUpdateGuestStatusViewModel.CheckOutDate);
                parameters.Add("@ModifiedBy", objUpdateGuestStatusViewModel.ModifiedBy);

                var objReturnvalue = objDBConnection.connection.Query<ModifyGuestCardStatusViewModel>("[dbo].[Usp_Update_GuestCardStatus]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //-----------------------------------------------------------
        //-----------------Issue Card-------------------------------------------
        public InsertIssueCardResponseViewModel InsertIssueCards(InsertIssueCardViewModel objIssueCardsViewModel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();                  
                parameters.Add("@CardType", "G");
                parameters.Add("@DoorID", objIssueCardsViewModel.DoorID);
                parameters.Add("@CardNo", objIssueCardsViewModel.CardNo);
                parameters.Add("@CardHolderID", objIssueCardsViewModel.CardHolderID);
                parameters.Add("@BeginTime", objIssueCardsViewModel.BeginTime);
                parameters.Add("@EndTime", objIssueCardsViewModel.EndTime);
                parameters.Add("@CardStatus", 0);
                parameters.Add("@BranchId", objIssueCardsViewModel.BranchId);
                parameters.Add("@ComputerName", objIssueCardsViewModel.ComputerName);
                parameters.Add("@ModifiedBy", objIssueCardsViewModel.ModifiedBy);

                var objReturnvalue = objDBConnection.connection.Query<InsertIssueCardResponseViewModel>("[dbo].[Usp_Insert_IssueCard]", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public ModifyIssueCardViewModel UpdateIssueCards(ModifyIssueCardViewModel objIssueCardsViewModel)
        {
            DBConnection objDBConnection = new DBConnection();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@IssueCardId", objIssueCardsViewModel.IssueCardID);
                parameters.Add("@TimeSectionID", objIssueCardsViewModel.TimeSectionID);
                parameters.Add("@BeginTime", objIssueCardsViewModel.BeginTime);
                parameters.Add("@EndTime", objIssueCardsViewModel.EndTime);
                parameters.Add("@HHDAccess", objIssueCardsViewModel.HHDAccess);
                parameters.Add("@CardStatus", objIssueCardsViewModel.CardStatus);
                parameters.Add("@CardDesc", objIssueCardsViewModel.CardDesc);
                parameters.Add("@ComputerName", objIssueCardsViewModel.ComputerName);
                parameters.Add("@ModifiedBy", objIssueCardsViewModel.ModifiedBy);
                parameters.Add("@DoorID", objIssueCardsViewModel.DoorID);
                parameters.Add("@CardHolderID", objIssueCardsViewModel.CardHolderID);

                var objReturnvalue = objDBConnection.connection.Query<ModifyIssueCardViewModel>("[dbo].[Usp_Update_IssueCard] ", parameters,
                commandType: CommandType.StoredProcedure).SingleOrDefault();

                objDBConnection.CloseConnection();
                return objReturnvalue;
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //-----------------------------------------------------------------------
    }
}
