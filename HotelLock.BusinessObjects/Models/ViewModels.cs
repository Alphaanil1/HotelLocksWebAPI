using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
    public class EmailData
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }

    public class EmailSendData
    {
        public List<EmailSenderDetails> EmailTo { get; set; }
        public List<EmailSenderDetails> EmailCc { get; set; }
        public List<EmailSenderDetails> EmailBcc { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }

    }

    public class EmailSenderDetails
    {
        public string UserName { get; set; }
        public string MailAddress { get; set; }
    }
}
