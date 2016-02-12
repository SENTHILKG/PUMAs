using System;
using System.Configuration;
using System.IO;

namespace P2PBusinessLayer
{
    public class   WriteLogToFile 
    {
        public static void WriteLog(string message)
        {
            string filePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            File.AppendAllText(filePath, DateTime.Now.ToString() + Environment.NewLine);
            File.AppendAllText(filePath, message + Environment.NewLine);
            File.AppendAllText(filePath, "***********************" + Environment.NewLine);
           
        }
    }
}
