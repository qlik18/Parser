using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Windows.Forms;
using System.Configuration;

namespace Utility
{
    public class ExceptionManager {
        private static readonly string MESSAGE = "Wystąpił błąd w działaniu aplikacji. Szczegóły zostały zapisane do pliku logów. !";
        private static readonly string TITLE = "Błąd";
        private Object _sync = new Object();
        private Logger logger;
        public ExceptionManager() 
        {
            logger = new Logger(ConfigurationManager.AppSettings["DirectoryLogs"], null);
        }

        public ExceptionManager(Logger logger)
        {
            this.logger = logger;
        }

        public static void LogExceptionWithMessage(Exception ex)
        {
            MessageBox.Show(MESSAGE + "\r\n" + ex.Message, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void LogException(Exception ex)
        {
            logger.LogException(ex);
        }

        public static void LogError(Exception ex, Logger logger)
        {
            LogError(ex, logger, false);
        }
        public static void LogError(Exception ex, Logger logger, bool showMessageBox)
        {
            logger.LogException(ex);
            if (showMessageBox)
                MessageBox.Show(string.Concat(ex.Message, "\n\n", MESSAGE) , TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);        
        }
        public static void LogWarning(string message, Logger logger)
        {
            logger.LogWarning(string.Concat(message, "\n\n"));
        }
    }
}

