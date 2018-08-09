using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace Gamestore_MVC
{
    public class MVC_Logger
    {
        private static string _LogPath = ConfigurationManager.AppSettings["ErrorLogPath"];
        public void ErrorLog(string className, string methodName, Exception sqlEx, string level = "Error")
        {
            string stackTrace = sqlEx.StackTrace;

            using (StreamWriter errorWriter = new StreamWriter(_LogPath, true))
            {
                errorWriter.WriteLine(new string('-', 40));
                errorWriter.WriteLine($"Class: {className} Method: {methodName} / {DateTime.Now.ToString()} {level}\n{sqlEx.Message}\n{stackTrace}");

            }
        }
    }
}