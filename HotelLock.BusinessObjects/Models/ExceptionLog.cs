using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models
{
    public class ExceptionLog
    {
        public string ProcessName { get; set; }
        public string MethodName { get; set; }
        public string SystemMessage { get; set; }
        public string ErrorText { get; set; }
        public string Message { get; set; }
        public string AddtionalData { get; set; }
        public string LineNumber { get; set; }
        public string Source { get; set; }
        public string Procedure { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorType { get; set; }
    }
}
