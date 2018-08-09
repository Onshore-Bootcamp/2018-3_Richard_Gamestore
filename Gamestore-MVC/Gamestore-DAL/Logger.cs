using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;


namespace Gamestore_DAL
{
    public class Logger
    {
        private readonly string _LogPath;

        public Logger( string logpath)
        {
            _LogPath = logpath;
        }
        
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
