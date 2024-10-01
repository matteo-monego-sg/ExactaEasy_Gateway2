using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

using SPAMI.Util;
using SPAMI.Util.Native;

namespace SPAMI.Util.Logger
{
    public enum LogLevels
    {
        Error,
        Warning,
        Pass,
        Debug
    }

    public enum FileModeEnum
    {
        Append,
        CreateNewAtStartup,
        CreateDaily,
        Overwrite
    }

    public struct LogMessage
    {
        public LogLevels level;
        public string message;
        public LogMessage(LogLevels _level, string _message)
        {
            level = _level;
            message = _message;
        }
    }

    public class Logger : Component, ICommon
    {
        [Browsable(false)]
        public string ClassName { get; set; }
        private bool consoleAllocated;
        private string sLogFolder;
        private const string sDefaultLogFolder = @"..\var\log";
        private const string sDefaultLogFile = @"Log";
        private const string sFilenameExt = ".txt";
        private string sLogFile;
        private StreamWriter streamLogFile;
        private int initialDay = DateTime.Now.Day;
        public delegate void NewMessageEventHandler(object sender, MessageEventArgs args);
        public event NewMessageEventHandler OnNewMessage;

        public Logger()
        {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            this.Disposed += new EventHandler(Logger_Disposed);
        }
        [Browsable(true), Category("Log"), Description("Specifies if console logging is enabled.")]
        public bool WriteToConsole { get; set; }
        [Browsable(true), Category("Log"), Description("Specifies console minimum logging level to write.")]
        public LogLevels WriteToConsoleLevel { get; set; }
        [Browsable(true), Category("Log"), Description("Specifies if file logging is enabled.")]
        public bool WriteToFile { get; set; }
        [Browsable(true), Category("Log"), Description("Specifies file minimum logging level to write.")]
        public LogLevels WriteToFileLevel { get; set; }
        [Browsable(true), Category("Log"), Description("Specifies how to manage log file(s).")]
        public FileModeEnum FileMode { get; set; }
        [Browsable(true), Category("Log"), Description("Specifies log file(s) folder.")]
        public string LogFolder
        {
            get
            {
                if (sLogFolder == null) sLogFolder = sDefaultLogFolder;
                return sLogFolder;
            }
            set
            {
                sLogFolder = value;
            }
        }
        [Browsable(true), Category("Log"), Description("Specifies how many days store log file(s).")]
        public int StorageDays { get; set; }


        public bool Initialized { get; private set; }
        public void Init()
        {
            Destroy();
            if (WriteToConsole)
                CreateConsole();
            if (WriteToFile)
            {
                DeleteOldLogs();
                CreateLogFilename();
                if (FileMode == FileModeEnum.Overwrite)
                    streamLogFile = new StreamWriter(sLogFile, false);
                else
                    streamLogFile = new StreamWriter(sLogFile, true);
                streamLogFile.AutoFlush = true;
            }
            Log.Init(this);
            Initialized = true;
        }

        public void Logger_Disposed(object sender, EventArgs args)
        {
            Initialized = false;
            Destroy();
        }

        private void Destroy()
        {
            Log.Destroy();
            DestroyConsole();
            if (streamLogFile != null)
            {
                streamLogFile.Flush();
                streamLogFile.Close();
            }
        }

        public void WriteFile(string line)
        {
            if ((FileMode == FileModeEnum.CreateDaily) &&
                DateTime.Now.Day != initialDay)
            {
                initialDay = DateTime.Now.Day;
                if (streamLogFile != null)
                {
                    streamLogFile.Flush();
                    streamLogFile.Close();
                }
                if (CreateLogFilename())
                {
                    streamLogFile = new StreamWriter(sLogFile, true);
                    streamLogFile.AutoFlush = true;
                }
            }
            if (streamLogFile != null) streamLogFile.WriteLine(line);
        }

        public void DispatchMessage(LogMessage logMsg)
        {
            if (OnNewMessage != null) OnNewMessage(this, new MessageEventArgs(logMsg));
        }

        private void DeleteOldLogs()
        {
            string sFolder = Path.GetFullPath(LogFolder);
            if (Directory.Exists(sFolder) == false)
            {
                return;
            }
            string[] logFilesPath = Directory.GetFiles(sFolder);
            DateTime currentTime = DateTime.Now;
            foreach (string filePath in logFilesPath)
            {
                DateTime creationTime = File.GetCreationTime(filePath);
                if ((currentTime - creationTime).TotalDays > StorageDays)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteToConsole(ConsoleColor.Red, ClassName + ".DeleteOldLogs - Error! Can't delete log file: " + filePath + "\r\n" + ex.Message);
                    }
                }
            }
        }

        private bool CreateLogFilename()
        {
            DateTime creationTime = DateTime.Now;
            string sFolder = Path.GetFullPath(LogFolder);
            try
            {
                Directory.CreateDirectory(sFolder);
            }
            catch (System.Exception ex)
            {
                Utilities.WriteToConsole(ConsoleColor.Red, ClassName + ".CreateLogFilename - Error! Can't create log folder: " + LogFolder + "\r\n" + ex.Message);
                return false;
            }

            switch (FileMode)
            {
                case FileModeEnum.Append:
                    sLogFile = sFolder + @"\" + sDefaultLogFile + sFilenameExt;
                    break;
                case FileModeEnum.CreateDaily:
                    string dateFilename = sFolder + @"\" + Utilities.DateString();
                    sLogFile = dateFilename + sFilenameExt;
                    break;
                case FileModeEnum.CreateNewAtStartup:
                    string datetimeFilename = sFolder + @"\" + Utilities.DateTimeString();
                    sLogFile = datetimeFilename + sFilenameExt;
                    break;
                case FileModeEnum.Overwrite:
                    sLogFile = "Log" + sFilenameExt;
                    break;
            }
            return true;
        }

        public void CreateConsole()
        {
            NativeMethods.FreeConsole();
            consoleAllocated = NativeMethods.AllocConsole();
            try
            {
                Console.BufferWidth = 512;
                Console.BufferHeight = 1024;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, ClassName + ".CreateConsole", "Errore: ", ex.Message);
            }
            if (consoleAllocated)
                Log.Line(LogLevels.Debug, ClassName + ".CreateConsole", "Console allocata!");
            else
                Log.Line(LogLevels.Error, ClassName + ".CreateConsole", "Errore: Console non allocata!");
        }

        public void DestroyConsole()
        {
            consoleAllocated = !NativeMethods.FreeConsole();
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public LogMessage Message { get; set; }

        public MessageEventArgs(LogMessage message)
        {
            Message = message;
        }
    }
}
