using System;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace TelephoneServiceProvider.Equipment
{
    internal class Logger
    {
        public static string LogFilesPath = ConfigurationManager.AppSettings["LogFilesPath"];

        internal static void WriteLine(string message)
        {
            using (var streamWriter = new StreamWriter(LogFilesPath, true))
            {
                streamWriter.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture) + ":",-21} {message}");
            }
        }
    }
}