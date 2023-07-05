using HotelLock.BusinessObjects.Models;
using HotelLock.BusinessObjects.Models.Utility;
using HotelLock.DAL.Repositories;
using HotelLock.DAL.Repositories.InterfaceRepositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotelLock.BLL
{

    public class GuestInformationService
    {
        private GuestInformationRepository objGuestInformationRepository;
        IEmailService objEmailService = null;
        private readonly IJWTManagerRepository _jWTManager;
        private readonly Encrypt _encrypt = new Encrypt("alphaict2019");

        public GuestInformationService(IEmailService emailService, IJWTManagerRepository jWTManager)
        {
            objGuestInformationRepository = new GuestInformationRepository();
            objEmailService = emailService;
            this._jWTManager = jWTManager;
        }
        #region "Guest Card"      
        public bool CheckCardNumberPresent(string CardNo, int BranchID)
        {
            return objGuestInformationRepository.CheckCardNumber(CardNo, BranchID);
        }
        public string IncreCardNumber(int BranchID)
        {

            string NewCardNo = string.Empty;
            string CrdNum = "101";

            try
            {
                //CHECK CARD NUMBER IS ALREADY PRESENT
                do
                {
                    string strGuestID = objGuestInformationRepository.GenerateCardno();

                    NewCardNo = Convert.ToString(strGuestID, CultureInfo.CurrentCulture);

                    if (NewCardNo == "0" || NewCardNo == "0000000000")
                        NewCardNo = Convert.ToString(CrdNum, CultureInfo.CurrentCulture);
                    else
                        NewCardNo = Convert.ToString(Convert.ToInt32(NewCardNo, CultureInfo.CurrentCulture) + 1, CultureInfo.CurrentCulture);

                    for (int i = NewCardNo.Length; i < 10; i++)
                        NewCardNo = "0" + NewCardNo;

                }
                while (CheckCardNumberPresent(NewCardNo, BranchID));

            }
            catch
            {
                throw;
            }

            return NewCardNo;
        }
        public ResultViewModel<GuestInformationViewModel> InsertGuestCard(GuestInformationViewModel objGuestInformationViewModel)
        {
            try
            {
                if (objGuestInformationViewModel == null)
                {
                    return new ResultViewModel<GuestInformationViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    string cardNo = IncreCardNumber(objGuestInformationViewModel.BranchID);
                    objGuestInformationViewModel.GuestID = cardNo;


                    string  validationMessage = InsertGuestCardValidation(objGuestInformationViewModel);

                    if (validationMessage.ToUpper() == "VALID")
                    {
                        GuestInformationViewModel objResult = objGuestInformationRepository.InsertGuestCard(objGuestInformationViewModel);
                        if (objResult != null)
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationAddSuccessMessage);
                            return new ResultViewModel<GuestInformationViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                        }
                        else
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationAddFailureMessage);
                            return new ResultViewModel<GuestInformationViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<GuestInformationViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = validationMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<GuestInformationViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }
        private string InsertGuestCardValidation(GuestInformationViewModel objGuestInformationViewModel)
        {
            string errorMSG = "";
            bool isvalid = true;

            try
            {


                if (objGuestInformationViewModel.GuestName.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "Guest Name is required, ";

                }
                //else if (!(objGuestInformationViewModel.ModelType >= 0 && objReceivedData.ModelType <= 1))
                //{
                //    isvalid = false;
                //    errorMSG += "Model Type is invalid, ";

                //}
                if (objGuestInformationViewModel.Nationality.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "Nationality is required, ";

                }
                if (objGuestInformationViewModel.BirthDate.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "Date of birth is required, ";

                }
                if (objGuestInformationViewModel.DocumentID.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "Document Name is required, ";

                }
                if (objGuestInformationViewModel.DocumentNumber.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "Document Number is required";

                }

                if (isvalid)
                {
                    return "Valid";
                }
                else
                {
                    return errorMSG;
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();

            }
        }      
        public ResultViewModel<GuestInformationViewModel> ModifyGuestCard(GuestInformationViewModel objGuestInformationViewModel)
        {
            try
            {
                if (objGuestInformationViewModel == null)
                {
                    return new ResultViewModel<GuestInformationViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                  string validationMessage = ModifyGuestCardValidation(objGuestInformationViewModel);

                    if (validationMessage.ToUpper() == "VALID")
                    {
                        GuestInformationViewModel objResult = objGuestInformationRepository.ModifyGuestCard(objGuestInformationViewModel);
                        if (objResult != null)
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationEditSuccessMessage);
                            return new ResultViewModel<GuestInformationViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                        }
                        else
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationEditFailureMessage);
                            return new ResultViewModel<GuestInformationViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<GuestInformationViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = validationMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<GuestInformationViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }
        private string ModifyGuestCardValidation(GuestInformationViewModel objGuestInformationViewModel)
        {
            string errorMSG = "";
            bool isvalid = true;

            try
            {
                errorMSG = InsertGuestCardValidation(objGuestInformationViewModel);

                if (errorMSG == "Valid")
                {
                    errorMSG = "";
                }
                else
                {
                    isvalid = false;
                }
                if (objGuestInformationViewModel.GuestID.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += ",Guest ID is required";
                }
                if (isvalid)
                {
                    return "Valid";
                }
                else
                {
                    return errorMSG;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public ResultViewModel<ModifyGuestCardStatusViewModel> ModifyGuestCardStatus(ModifyGuestCardStatusViewModel objModifyGuestCardStatusViewModel)
        {
            try
            {
                if (objModifyGuestCardStatusViewModel == null)
                {
                    return new ResultViewModel<ModifyGuestCardStatusViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {

                    string validationMessage = ModifyGuestCardStatusValidation(objModifyGuestCardStatusViewModel);

                    if (validationMessage.ToUpper() == "VALID")
                    {
                        ModifyGuestCardStatusViewModel objResult = objGuestInformationRepository.ModifyGuestCardStatus(objModifyGuestCardStatusViewModel);
                        if (objResult != null)
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationAddSuccessMessage);
                            return new ResultViewModel<ModifyGuestCardStatusViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                        }
                        else
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationAddFailureMessage);
                            return new ResultViewModel<ModifyGuestCardStatusViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<ModifyGuestCardStatusViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = validationMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<ModifyGuestCardStatusViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }
        private string ModifyGuestCardStatusValidation(ModifyGuestCardStatusViewModel objModifyGuestCardStatusViewModel)
        {
            string errorMSG = "";
            bool isvalid = true;

            try
            {
                if (objModifyGuestCardStatusViewModel.GuestID.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "GuestID is required, ";
                }
                
                if (objModifyGuestCardStatusViewModel.CheckOutDate == null)
                {
                    isvalid = false;
                    errorMSG += "Checkout Date is required, ";
                }
                if (objModifyGuestCardStatusViewModel.CheckOutStatus == null)
                {
                    isvalid = false;
                    errorMSG += "Checkout Status is required";
                }               

                if (isvalid)
                {
                    return "Valid";
                }
                else
                {
                    return errorMSG;
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();

            }
        }
        #endregion
        #region "Issue Card"      
        public ResultViewModel<InsertIssueCardResponseViewModel> InsertIssueCard(InsertIssueCardViewModel objInsertIssueCardViewModel)
        {
            try
            {
                if (objInsertIssueCardViewModel == null)
                {
                   return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
                }
                else
                {
                    //string cardNo = IncreCardNumber(objInsertIssueCardViewModel.BranchID);
                    //objInsertIssueCardViewModel.GuestID = cardNo;

                    string validationMessage = InsertIssueCardValidation(objInsertIssueCardViewModel);

                    if (validationMessage.ToUpper() == "VALID")
                    {
                        InsertIssueCardResponseViewModel objResult = objGuestInformationRepository.InsertIssueCards(objInsertIssueCardViewModel);
                        if (objResult != null)
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationAddSuccessMessage);
                            return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
                        }
                        else
                        {
                            string returnMessage = Convert.ToString(UserMessage.GuestInformationAddFailureMessage);
                            return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
                        }
                    }
                    else
                    {
                        return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = validationMessage };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
            }
        }
        private string InsertIssueCardValidation(InsertIssueCardViewModel objInsertIssueCardViewModel)
        {
            string errorMSG = "";
            bool isvalid = true;
            try
            {
                if (objInsertIssueCardViewModel.DoorID.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "DoorID is required, ";

                }               
                if (objInsertIssueCardViewModel.CardHolderID.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "CardHolderID is required ";

                }   
                if (isvalid)
                {
                    return "Valid";
                }
                else
                {
                    return errorMSG;
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();

            }
        }
        //public ResultViewModel<InsertIssueCardResponseViewModel> InsertIssueCard(InsertIssueCardViewModel objInsertIssueCardViewModel)
        //{
        //    try
        //    {
        //        if (objInsertIssueCardViewModel == null)
        //        {
        //            return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = UserMessage.InvalidData };
        //        }
        //        else
        //        {
        //            //string cardNo = IncreCardNumber(objInsertIssueCardViewModel.BranchID);
        //            //objInsertIssueCardViewModel.GuestID = cardNo;

        //            string validationMessage = InsertIssueCardValidation(objInsertIssueCardViewModel);

        //            if (validationMessage.ToUpper() == "VALID")
        //            {
        //                InsertIssueCardResponseViewModel objResult = objGuestInformationRepository.InsertIssueCards(objInsertIssueCardViewModel);
        //                if (objResult != null)
        //                {
        //                    string returnMessage = Convert.ToString(UserMessage.GuestInformationAddSuccessMessage);
        //                    return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Success.ToString(), UserMessage = returnMessage };
        //                }
        //                else
        //                {
        //                    string returnMessage = Convert.ToString(UserMessage.GuestInformationAddFailureMessage);
        //                    return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = objResult, ResponseCode = System.Net.HttpStatusCode.BadRequest, Message = Message.Failure.ToString(), UserMessage = returnMessage };
        //                }
        //            }
        //            else
        //            {
        //                return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.OK, Message = Message.Failure.ToString(), UserMessage = validationMessage };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultViewModel<InsertIssueCardResponseViewModel> { Result = null, ResponseCode = System.Net.HttpStatusCode.InternalServerError, Message = Message.Exception.ToString(), UserMessage = ex.ToString() };
        //    }
        //}
        private string UpdateIssueCardValidation(ModifyIssueCardViewModel objInsertIssueCardViewModel)
        {
            string errorMSG = "";
            bool isvalid = true;
            try
            {
                if (objInsertIssueCardViewModel.DoorID.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "DoorID is required, ";

                }
                if (objInsertIssueCardViewModel.CardHolderID.ToString() == null)
                {
                    isvalid = false;
                    errorMSG += "CardHolderID is required ";

                }
                if (isvalid)
                {
                    return "Valid";
                }
                else
                {
                    return errorMSG;
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();

            }
        }
        #endregion




    }
}
