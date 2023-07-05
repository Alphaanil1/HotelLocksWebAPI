using HotelLock.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.DAL.Repositories.InterfaceRepositories
{
    public interface IEmailService
    {
        bool SendEmail(EmailData emailData);
        bool SendUserPasswordEmail(UserAccessRights userData, string mailsubject);
        bool SendForgotPasswordEmail(UserLoginViewModel userData, string mailsubject);
        bool Email(EmailSendData emailData);
    }
}
