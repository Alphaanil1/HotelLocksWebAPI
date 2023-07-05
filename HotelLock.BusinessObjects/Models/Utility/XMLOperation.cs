using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HotelLock.BusinessObjects.Models.Utility
{
    public class XMLOperation
    {

        public string ObjectToXmlConverter<T>(T dataToSerialize)
        {

            string strxml = null;
            try
            {

                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                strxml = stringwriter.ToString();
                strxml = strxml.Replace("utf-16", "utf-8");
            }
            catch (InvalidOperationException ex)

            {
                ExceptionLog objExceptionLog = new ExceptionLog()
                {
                    ErrorType = "Exception",
                    ProcessName = this.GetType().Name,
                    MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    SystemMessage = ex.Message.ToString(),
                    Message = "Error occured to serialize data",
                    AddtionalData = "data :" + dataToSerialize,
                    Procedure = "NA",

                };

            }
            catch (Exception ex)
            {
                ExceptionLog objExceptionLog = new ExceptionLog()
                {
                    ErrorType = "Exception",
                    ProcessName = this.GetType().Name,
                    MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    SystemMessage = ex.Message.ToString(),
                    Message = "Error occured to serialize data",
                    AddtionalData = "data :" + dataToSerialize,
                    Procedure = "NA",

                };
                Utility.ErrorLogs.ErrorLogAndNotification(objExceptionLog, ex);
            }
            return strxml;
        }


        public string ObjectToXmlConverter<T>(T dataToSerialize, string UTF)
        {
            string strxml = null;
            try
            {
                // // // warning disable CA1062 violation
                if (dataToSerialize == null)
                { throw new ArgumentNullException(nameof(dataToSerialize)); }
                // // // warning disable CA1062 violation
                if (UTF == null)
                { throw new ArgumentNullException(nameof(UTF)); }

                using (System.IO.StringWriter stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));

                    serializer.Serialize(stringwriter, dataToSerialize);
                    strxml = stringwriter.ToString();

                    //if (UTF.ToLower(new CultureInfo("en-US", false)) == "utf-8")
                    //{
                    //    strxml = strxml.Replace("utf-16", "utf-8");
                    //}                    
                }
            }
            catch (InvalidOperationException ex)

            {
                ExceptionLog objExceptionLog = new ExceptionLog()
                {
                    ErrorType = "Exception",
                    ProcessName = this.GetType().Name,
                    MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    SystemMessage = ex.Message.ToString(),
                    Message = "Error occurred to serialize data",
                    AddtionalData = "data :" + dataToSerialize,
                    Procedure = "NA",

                };

            }
            catch (Exception ex)
            {
                ExceptionLog objExceptionLog = new ExceptionLog()
                {
                    ErrorType = "Exception",
                    ProcessName = this.GetType().Name,
                    MethodName = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    SystemMessage = ex.Message.ToString(),
                    Message = "Error occurred to serialize data",
                    AddtionalData = "data :" + dataToSerialize,
                    Procedure = "NA",

                };
                Utility.ErrorLogs.ErrorLogAndNotification(objExceptionLog, ex);
            }
            return strxml;
        }



    }
}

