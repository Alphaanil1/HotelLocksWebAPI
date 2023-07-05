using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.BusinessObjects.Models.Utility
{
    public static class ErrorLogs
    {

        public static void ErrorLogAndNotification(string INFO)
        {
            FileStream fs = null;
            try
            {
                string path = "D:\\HotelLockLogs\\SystemWebLog.txt";

                string result = Path.GetFileNameWithoutExtension(path);
                var date = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;

                path = path.Replace(result, date);

                string addtext = "";
                string Errormessage = String.Empty;
                fs = new FileStream(path, FileMode.OpenOrCreate);

                using (StreamWriter str = new StreamWriter(fs))
                {
                    str.BaseStream.Seek(0, SeekOrigin.End);
                    str.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
                    str.WriteLine(DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());
                    str.Write(INFO);

                    str.Flush();
                }

                File.AppendAllText(path, addtext);  // // //Exception occurrs ??????????

                StringBuilder strbuild = new StringBuilder();
                strbuild.Append("----------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
                strbuild.Append(DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());
                strbuild.Append(INFO + Environment.NewLine);

                //Log.Logger.Information(INFO);

                strbuild.Clear();
            }
            catch (FieldAccessException EX)
            {
                EX.ToString();
            }
            catch (FileNotFoundException EX)
            {
                EX.ToString();
            }
            catch (Exception EX)
            {
                EX.ToString();
                // throw;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }


        public static void ErrorLogAndNotification(ExceptionLog objExceptionLog, Exception ex)
        {
            FileStream fs = null;
            try
            {
                objExceptionLog.SystemMessage = ex.Message.ToString();
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                objExceptionLog.LineNumber = lineNumber.ToString(); ;
                objExceptionLog.SystemMessage = ex.Message.ToString();

                string path = "D:\\HotelLockLogs\\SystemWebLog.txt";

                string result = Path.GetFileNameWithoutExtension(path);
                var date = "WebAPI" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;

                path = path.Replace(result, date);

                string addtext = "";
                fs = new FileStream(path, FileMode.OpenOrCreate);
                using (StreamWriter str = new StreamWriter(fs))
                {
                    str.BaseStream.Seek(0, SeekOrigin.End);
                    str.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
                    str.WriteLine(DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());
                    str.Write("[" + objExceptionLog.ErrorCode + "]" + "[" + objExceptionLog.ErrorType + "]" + "[" + objExceptionLog.ErrorText + "]" + "[" + objExceptionLog.ProcessName + "]" + "[" + objExceptionLog.MethodName + "]" + "[Message:" + objExceptionLog.Message + "]" + "[ Line Number:" + objExceptionLog.LineNumber + ", Source:" + objExceptionLog.Source + ", Procedure:" + objExceptionLog.Procedure + "]" + Environment.NewLine);

                    addtext = "[System error:" + objExceptionLog.SystemMessage + " Additional Data:" + objExceptionLog.AddtionalData + Environment.NewLine;

                    str.Flush();
                }

                File.AppendAllText(path, addtext);  //Exception occurrs ??????????


                StringBuilder strbuild = new StringBuilder();
                strbuild.Append("----------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
                strbuild.Append(DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());
                strbuild.Append("[" + objExceptionLog.ErrorCode + "]" + "[" + objExceptionLog.ErrorType + "]" + "[" + objExceptionLog.ErrorText + "]" + "[" + objExceptionLog.ProcessName + "]" + "[" + objExceptionLog.MethodName + "]" + "[Message:" + objExceptionLog.Message + "]" + "[ Line Number:" + objExceptionLog.LineNumber + ", Source:" + objExceptionLog.Source + ", Procedure:" + objExceptionLog.Procedure + "]" + Environment.NewLine);
                strbuild.Append("[System error:" + objExceptionLog.SystemMessage + " Additional Data:" + objExceptionLog.AddtionalData + Environment.NewLine);

                //PostStringData(strbuild, "ErrorLog");
                strbuild.Clear();
            }
            catch (FieldAccessException EX)
            {
                EX.ToString();
            }
            catch (FileNotFoundException EX)
            {
                EX.ToString();
            }
            catch (Exception EX)
            {
                EX.ToString();
                //throw;
            }
            finally
            {
                //if (fs != null)
                //{
                //    fs.Dispose();
                //}
            }
        }
    }
}
