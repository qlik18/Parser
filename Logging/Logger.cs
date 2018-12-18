using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security;
using System.Reflection;

namespace Logging
{
    public class Logger
    {
        private const string LogName = "Application";
        private DateTime logStartTime;

        /// <summary>
        /// Gets or sets the global instance of the logger that can be used across application. Null by default.
        /// </summary>        
        public static Logger Instance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only warnings and exceptions should be logged.
        /// </summary>
        /// <value><c>true</c> if only warnings and exceptions should be logged; otherwise, <c>false</c>.</value>
        public bool LogWarningAndExceptionsOnly { get; set; }

        /// <summary>
        /// Gets the path to directory where log files are stored.
        /// </summary>        
        public string LogsDirectoryPath { get; private set; }

        /// <summary>
        /// Gets the name of the event log source used to logging errors in system Event Log.
        /// </summary>        
        public string EventLogSource { get; private set; }

        /// <summary>
        /// Gets a value indicating whether logs are being written to the Event Log.
        /// </summary>
        /// <value><c>true</c> if logs are being written to the Event Log; otherwise, <c>false</c>.</value>
        public bool WritesToEventLog { get; private set; }

        /// <summary>
        /// Gets a value indicating whether logs are being written to files.
        /// </summary>
        /// <value><c>true</c> if logs are being written to files; otherwise, <c>false</c>.</value>
        public bool WritesToFile { get; private set; }

        /// <summary>
        /// Gets a value indicating whether log files are being grouped in directories.
        /// </summary>
        /// <value><c>true</c> if files are being grouped in directories; otherwise, <c>false</c>.</value>
        public bool GroupsFiles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class. IMPORTANT NOTE: To use EventLog log, application must be started as Administrator at least once in order to create required EventLog source.
        /// </summary>
        /// <param name="logsDirectoryPath">The path to directory where log files are stored. Can be absolute or relative to application's path. Should be null or empty to prevent logging to files.</param>
        /// <param name="eventSourceName">The name of the event log source used to logging errors in system Event Log. Usually the application's name. Should be null or empty to prevent logging to Event Log.</param>
        public Logger(string logsDirectoryPath, string eventSourceName)
            : this(logsDirectoryPath, eventSourceName, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class. IMPORTANT NOTE: To use EventLog log, application must be started as Administrator at least once in order to create required EventLog source.
        /// </summary>
        /// <param name="logsDirectoryPath">The path to directory where log files are stored. Can be absolute or relative to application's path. Should be null or empty to prevent logging to files.</param>
        /// <param name="eventSourceName">The name of the event log source used to logging errors in system Event Log. Usually the application's name. Should be null or empty to prevent logging to Event Log.</param>
        /// <param name="groupFilesInDirectories">if set to <c>true</c>, log files will be grouped in separate directories for every single application run.</param>
        public Logger(string logsDirectoryPath, string eventSourceName, bool groupFilesInDirectories)
        {
            //if (string.IsNullOrEmpty(logsDirectoryPath) && string.IsNullOrEmpty(eventSourceName))
            //    throw new ArgumentException("At least one of the parameters must be a non-empty string.", "logsDirectoryPath, eventSourceName");

            GroupsFiles = groupFilesInDirectories;
            logStartTime = DateTime.Now;

            if (!string.IsNullOrEmpty(logsDirectoryPath))
                InitializeFileLogger(logsDirectoryPath);

            if (!string.IsNullOrEmpty(eventSourceName))
                InitializeEventLogLogger(eventSourceName);
        }

        private void InitializeEventLogLogger(string eventSourceName)
        {
            EventLogSource = eventSourceName;

            try
            {
                if (!EventLog.SourceExists(eventSourceName))
                    EventLog.CreateEventSource(eventSourceName, LogName);

                WritesToEventLog = true;
            }
            catch (SecurityException ex)
            {
                //if (WritesToFile)
                //    LogException(ex);

                WritesToEventLog = false;
            }
        }

        private void InitializeFileLogger(string logsDirectoryPath)
        {
            char[] invalidChars = Path.GetInvalidPathChars();
            if (logsDirectoryPath.Intersect(invalidChars).Count() > 0)
                throw new ArgumentException("Logs direcotry path contains illegal characters.", "logsDirectoryPath");

            if (Path.IsPathRooted(logsDirectoryPath))
                LogsDirectoryPath = logsDirectoryPath;
            else
            {
                string u = new @Uri(Assembly.GetCallingAssembly().CodeBase).AbsolutePath;
                LogsDirectoryPath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).AbsolutePath), logsDirectoryPath);
            }
            if (GroupsFiles)
                LogsDirectoryPath = Path.Combine(LogsDirectoryPath, logStartTime.Ticks.ToString());

            WritesToFile = true;
        }

        /// <summary>
        /// Logs the exception to file, event log or both (depending on configuration).
        /// </summary>
        /// <param name="exception">The exception to log.</param>        
        public string LogException(Exception exception)
        {
            string dummy;
            return LogException(exception, out dummy);
        }

        /// <summary>
        /// Logs the exception to file, event log or both (depending on configuration).
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="combinedInnerExceptionsMessage">Messages from inner exceptions (if any), one per line.</param>
        public string LogException(Exception exception, out string combinedInnerExceptionsMessage)
        {
            combinedInnerExceptionsMessage = CreateCombinedInnerExceptionsMessage(exception);
            string logContent = CreateLogContent(exception);

            string crashLogFilePath = string.Empty;
            if (WritesToFile)
                crashLogFilePath = WriteFile(logContent);

            if (WritesToEventLog)
                WriteEventLog(EventLogEntryType.Error, logContent);

            return crashLogFilePath;
        }

        /// <summary>
        /// Logs the information to file, event log or both (depending on configuration).
        /// </summary>
        /// <param name="message">The information to log.</param>
        public string LogInformation(string message)
        {
            Debug.WriteLine(message);

            string filePath = string.Empty;

            if (!LogWarningAndExceptionsOnly)
            {
                if (WritesToFile)
                    filePath = WriteFile(message);

                if (WritesToEventLog)
                    WriteEventLog(EventLogEntryType.Information, message);
            }

            return filePath;
        }

        /// <summary>
        /// Logs the warning to file, event log or both (depending on configuration).
        /// </summary>
        /// <param name="message">The warning to log.</param>
        public string LogWarning(string message)
        {
            Debug.WriteLine(message);

            string filePath = string.Empty;
            if (WritesToFile)
                filePath = WriteFile(message);

            if (WritesToEventLog)
                WriteEventLog(EventLogEntryType.Warning, message);

            return filePath;
        }

        /// <summary>
        /// Creates the string containing messages from inner exceptions (if any), one per line.
        /// </summary>
        /// <param name="exception">The exception for which to create a message.</param>
        public string CreateCombinedInnerExceptionsMessage(Exception exception)
        {
            StringBuilder sb = new StringBuilder();

            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                sb.AppendLine(innerException.Message);
                innerException = innerException.InnerException;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the string which will be logged when LogException will be called.
        /// </summary>
        /// <param name="exception">The exception for which to create the log content.</param>
        public string CreateLogContent(Exception exception)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("--- Main exception: Message ---");
            sb.AppendLine(exception.Message);
            sb.AppendLine();

            int index = 1;
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                sb.AppendFormat("--- Inner exception {0}: Message ---{1}", index, Environment.NewLine);
                sb.AppendLine(innerException.Message);
                sb.AppendLine();

                sb.AppendFormat("--- Inner exception {0}: StackTrace ---{1}", index, Environment.NewLine);
                sb.AppendLine(innerException.StackTrace);
                sb.AppendLine();

                sb.AppendFormat("--- Inner exception {0}: Source ---{1}", index++, Environment.NewLine);
                sb.AppendLine(innerException.Source);
                sb.AppendLine();

                innerException = innerException.InnerException;
            }

            sb.AppendLine("--- Main exception: StackTrace ---");
            sb.AppendLine(exception.StackTrace);
            sb.AppendLine();

            sb.AppendLine("--- Main exception: Source ---");
            sb.AppendLine(exception.Source);
            sb.AppendLine();

            return sb.ToString();
        }

        private string WriteFile(string message)
        {
            lock (this)
            {
                if (!Directory.Exists(LogsDirectoryPath))
                {
                    try { Directory.CreateDirectory(LogsDirectoryPath); }
                    catch (Exception ex)
                    {
                        Exception exception = new InvalidOperationException(string.Format("Could not create logs directory at path: {0}. {1}. See inner exception for details.", LogsDirectoryPath, ex.Message), ex);
                        if (WritesToEventLog)
                            WriteEventLog(EventLogEntryType.Error, CreateLogContent(exception));

                        Debug.WriteLine(CreateLogContent(exception));
                        return string.Empty;
                    }
                }

                string crashLogPath = Path.Combine(LogsDirectoryPath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));
                StreamWriter sw = new StreamWriter(crashLogPath, true);
                sw.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                sw.WriteLine();
                sw.Write(message);
                sw.Close();
                return crashLogPath;
            }

        }

        private void WriteEventLog(EventLogEntryType entryType, string message)
        {
            EventLog.WriteEntry(EventLogSource, message, entryType);
        }
    }
}

