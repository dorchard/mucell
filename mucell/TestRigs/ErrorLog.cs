using System;
using System.IO;

namespace MuCell.TestRigs
{
    /// <summary>
    /// Classic providing basic debugging capabilities by writing to an error log text file
    /// </summary>
    public class ErrorLog
    {
        public static TextWriter tw;

        /// <summary>
        /// Clears the error log
        /// </summary>
        public static void InitErrorLog()
        {
            tw = new StreamWriter("ErrorLog.txt");
            tw.Close();
        }


        /// <summary>
        /// Appends a line to the txt error log file
        /// </summary>
        /// <param name="str"></param>
        public static void LogError(String str)
        {
            tw = new StreamWriter("ErrorLog.txt",true);
            tw.WriteLine(str);
            tw.Close();
        }

    }
}
