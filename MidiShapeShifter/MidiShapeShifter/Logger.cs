using Microsoft.VisualBasic.Logging;
using MidiShapeShifter.Mss;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MidiShapeShifter
{
    public static class Logger
    {
        private static readonly TraceSource traceSource;

        //keeps track of calls to StartLoggingHighVelocity;
        private static int loggingHighVelocity = 0;

        //Next log message id = 30

        static Logger() {
            traceSource = new TraceSource("mss");

            FileLogTraceListener logListener = new FileLogTraceListener();
            logListener.BaseFileName = "mss-log";
            logListener.CustomLocation = MssFileSystemLocations.SettingsFolder;
            logListener.AutoFlush = true;
            logListener.MaxFileSize = 5000000; //5MB
            logListener.Delimiter = " | ";
            logListener.LogFileCreationSchedule = LogFileCreationScheduleOption.Weekly;
            logListener.DiskSpaceExhaustedBehavior = DiskSpaceExhaustedOption.DiscardMessages;
            logListener.TraceOutputOptions = TraceOptions.ThreadId;
            
            TextWriterTraceListener logWriterListener = new TextWriterTraceListener(MssFileSystemLocations.SettingsFolder + "log.txt");
            traceSource.Listeners.Add(logListener);

            traceSource.Switch = new SourceSwitch("switch", "");

            //TODO: this should probably be loaded from the registry or a settings file.
            traceSource.Switch.Level = SourceLevels.Warning;


            //Remove log files from previous weeks
            foreach (string logFilePath in Directory.GetFiles(MssFileSystemLocations.SettingsFolder, logListener.BaseFileName + "*.log"))
            {
                if (logFilePath != logListener.FullLogFileName)
                {
                    File.Delete(logFilePath);
                }
            }
        }

        public static void StartLoggingHighVolume(int id = -1, string message = null) {
            if (message != null)
            {
                WriteEntry(id, message, TraceEventType.Verbose);
            }
            loggingHighVelocity++;
        }

        public static void StopLoggingHighVolume(int id = -1, string message = null)
        {
            loggingHighVelocity--;

            if (message != null)
            {
                WriteEntry(id, message, TraceEventType.Verbose);
            }
        }

        public static void Error(int id, string message)
        {
            WriteEntry(id, message, TraceEventType.Error);
        }

        public static void Error(int id, Exception ex)
        {
            WriteEntry(id, ex.Message, TraceEventType.Error);
        }

        public static void Warning(int id, string message)
        {
            WriteEntry(id, message, TraceEventType.Warning);
        }

        public static void Info(int id, string message)
        {
            WriteEntry(id, message, TraceEventType.Information);
        }

        public static void Verbose(int id, string message)
        {
            WriteEntry(id, message, TraceEventType.Verbose);
        }

        public static void HighVolume(int id, string message) { 
            if (loggingHighVelocity > 0) {
                WriteEntry(id, message, TraceEventType.Verbose);
            }
        }

        private static void WriteEntry(int id, string message, TraceEventType type)
        {
            string formattedMessage = string.Format("{0} | {1}",
                                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                    message);

            //TODO: could use logListener.WriteLine instead to get rid of all the useless info in TraceEvent.
            traceSource.TraceEvent(type, id, formattedMessage);

        }
    }
}
