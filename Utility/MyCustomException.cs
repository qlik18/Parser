using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Security;
using System.Configuration;
using System.Diagnostics;

namespace Utility
{
    public class MyCustomException : Exception
    {
        public MyCustomException(string errorMessage, Exception innerEx, Object ob)
            : base(errorMessage, innerEx)
        {            
            //Console.WriteLine(errorMessage);
            //exMessage = errorMessage;
            
        }

        public MyCustomException(string errorMessage, Exception innerEx)
        {
            //EventLog.WriteEntry("BillenniumParser", errorMessage + "\n" + innerEx.Message + "\n" + innerEx.StackTrace);

            errorMessage = " -> " + errorMessage;

            if (innerEx is XmlException)
                throw new MyCustomException("Błąd XML" + errorMessage, innerEx);

            else if (innerEx is InvalidOperationException)
                throw new MyCustomException("Wywołanie metody jest błędne dla obiektu" + errorMessage, innerEx, null);

            else if (innerEx is ArgumentException)
                throw new MyCustomException("Błędny argument w metodzie" + errorMessage, innerEx, null);

            else if (innerEx is ArgumentNullException)
                throw new MyCustomException("Argument w tej metodzie nie może być pustą referencją" + errorMessage, innerEx, null);

            else if (innerEx is PathTooLongException)
                throw new MyCustomException("Ścieżka do pliku jest za długa (max 248)" + errorMessage, innerEx, null);

            else if (innerEx is DirectoryNotFoundException)
                throw new MyCustomException("Nie można znaleźć części pliku bądź folderu" + errorMessage, innerEx, null);

            else if (innerEx is ObjectDisposedException)
                throw new MyCustomException("Próba wykonania operacji na nieistniejącym obiekcie" + errorMessage, innerEx, null);

            else if (innerEx is IOException)
                throw new MyCustomException("Błąd I/O" + errorMessage, innerEx, null);

            else if (innerEx is UnauthorizedAccessException)
                throw new MyCustomException("Błąd dostępu z powodu błędu I/O bądź błędu bezpieczeństwa" + errorMessage, innerEx, null);

            else if (innerEx is FileNotFoundException)
                throw new MyCustomException("Plik nie został znaleziony" + errorMessage, innerEx, null);

            else if (innerEx is NotSupportedException)
                throw new MyCustomException("Użyta metoda nie jest wspierana / błąd strumienia danych." + errorMessage, innerEx, null);

            else if (innerEx is SecurityException)
                throw new MyCustomException("Błąd bezpieczeństwa" + errorMessage, innerEx, null);

            else if (innerEx is ConfigurationErrorsException)
                throw new MyCustomException("Aktualna wartość nie jest elementem EnableSessionState - sprawdź App.config" + errorMessage, innerEx, null);


            else if (innerEx is EncoderFallbackException)
                throw new MyCustomException("Niektóre znaki nie mogły zostać zapisane - błąd kodowania" + errorMessage, innerEx, null);

            else if (innerEx is AccessViolationException)
                throw new MyCustomException("Błąd w ścieżce XPath" + errorMessage, innerEx, null);

            else if (innerEx is XPathException)
                throw new MyCustomException("Błąd w ścieżce XPath" + errorMessage, innerEx, null);

            else if (innerEx is Exception)
                throw new MyCustomException("Wystąpił błąd, dzwoń po admina." + errorMessage, innerEx, null);

            
            //try
            //{
            //    throw innerEx;
            //}
            //catch (XPathException ex)
            //{
            //    throw new MyCustomException("Błąd w ścieżce XPath", ex);
            //}
            //catch (XmlException ex)
            //{
            //    throw new MyCustomException("Błąd w pliku XML", ex);
            //}
            //catch (PathTooLongException ex)
            //{
            //    throw new MyCustomException("Ściażka do pliku jest za długa", ex);
            //}
            //catch (FileNotFoundException ex)
            //{
            //    throw new MyCustomException("Plik nie został znaleziony", ex);
            //}



            //Console.WriteLine(errorMessage+" w "+ innerEx.Message);
            //exMessage = errorMessage;
        }

        public string GetMessage()
        {
            return GetMessage(this);
        }

        public string GetMessage(Exception ex)
        {
            if (ex.InnerException == null) return ex.Message;
            else return GetMessage(ex.InnerException);
        }

        public string GetMessageWithStackTrace()
        {
            return GetMessageWithStackTrace(this);
        }

        public string GetMessageWithStackTrace(Exception ex)
        {
            if (ex.InnerException == null) return ex.Message + "\n" + ex.StackTrace;
            else return GetMessageWithStackTrace(ex.InnerException);
        }
    }
}
