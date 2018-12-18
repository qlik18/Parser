using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Utility
{
    public class Log
    {

        public String getCurrentDate()
        {
            DateTime CurrTime = DateTime.Now;
            String month = (CurrTime.Month < 10) ? "0" + CurrTime.Month : ""+CurrTime.Month;
            String day = (CurrTime.Day < 10) ? "0" + CurrTime.Day : "" + CurrTime.Day;
            return "" + CurrTime.Year + "-" + month + "-" + day;
        }


        public String getFilePath()
        {         
            return "c:\\HParser_LogFile\\" + getCurrentDate() + ".log";
        }


        public void saveError(String message)
        {
            StreamWriter fileStream = new StreamWriter(getFilePath(), true, Encoding.UTF8);  
     
            message = Regex.Replace(message, @"\n+", " -> ");
            try
            {
                fileStream.WriteLine(message);
                fileStream.WriteLine();
            }
            catch(Exception ex)
            {
                throw new MyCustomException("Błąd przy zapisie wyjątku do pliku logu"+ getFilePath(), ex);
            }
            finally
            {
                fileStream.Close();
            }

        }

        public void readLog()
        {         
            try
            {
                System.Diagnostics.Process.Start("notepad.exe", getFilePath());   
            }
            catch(Exception ex)
            {
                throw new MyCustomException("Błąd przy odczycie pliku logu", ex);
            }
        }




    }
}
