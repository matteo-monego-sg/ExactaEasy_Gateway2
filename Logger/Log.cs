using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

using SPAMI.Util.Multilanguage;
using System.Globalization;


namespace SPAMI.Util.Logger
{
    public class Log
    {
        //private DateTime LogDateTime = new DateTime();
        private static Logger logger;
        private static object log = new object();
        private static ConcurrentQueue<LogMessage> MessageQueue = new ConcurrentQueue<LogMessage>();
        private static AutoResetEvent NewLineEvt = new AutoResetEvent(false);
        private static AutoResetEvent KillWriteThreadEvt = new AutoResetEvent(false);
        private static WaitHandle[] EventsList = new WaitHandle[2];
        private static Thread WriteTh;

        public Log()
        {
        }

        public static void Init(Logger _logger)
        {
            KillWriteThreadEvt.Reset();
            logger = _logger;
            EventsList[0] = NewLineEvt;
            EventsList[1] = KillWriteThreadEvt;
            WriteTh = new Thread(new ThreadStart(Log.WriterThread));
            WriteTh.Name = "Log writer thread";
            WriteTh.Priority = ThreadPriority.BelowNormal;
            WriteTh.IsBackground = true;
            WriteTh.Start();
            Thread.Sleep(15);
        }

        public static void Destroy()
        {
            if (WriteTh != null)
            {
                KillWriteThreadEvt.Set();
                WriteTh.Join();
            }
        }

        //per log non in lingua usare questa
        public static void Line(LogLevels level, string classFunctionName, string sFormatMsg, params object[] args)
        {
            DateTime lineTime = DateTime.Now;
            lock (log)
            {
                //string line = lineTime.ToShortDateString() + "-" + lineTime.ToLongTimeString() + "." + lineTime.Millisecond.ToString("d3") + ": ";
                string line = lineTime.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + ": ";
                line += "\"" + classFunctionName + "\" - ";
                line += string.Format(sFormatMsg, args);
                MessageQueue.Enqueue(new LogMessage(level, line));
                NewLineEvt.Set();
            }
        }

        // per log in lingua dell'applicazione usare questa
        public static void Line(LogLevels level, string classFunctionName, string sMultiLangAssembly, string sMultiLangLabel, params object[] args)
        {
            string sFormatMsg = Multilanguage.Multilang.GetString(sMultiLangAssembly, sMultiLangLabel);
            Line(level, classFunctionName, sFormatMsg, args);
        }

        private static void WriterThread()
        {
            while (logger!=null)
            {
                int ret = WaitHandle.WaitAny(EventsList);
                if (ret==1) //kill event
                    break;
                while (MessageQueue.Count > 0)
                {
                    LogMessage logMessage;
                    if (MessageQueue.TryDequeue(out logMessage))
                    {
                        if (logger.WriteToConsole &&
                            (logMessage.level <= logger.WriteToConsoleLevel))
                        {
                            SetColor(logMessage.level);
                            Console.WriteLine(logMessage.message);
                        }
                        if (logger.WriteToFile &&
                            (logMessage.level <= logger.WriteToFileLevel))
                        {
                            logger.WriteFile(logMessage.message);
                        }
                        logger.DispatchMessage(logMessage);
                        //if (logger.OnNewMessage != null) logger.OnNewMessage(null, new NewMessageEventArgs(logMessage));
                    }
                }
            }
        }

        private static void SetColor(LogLevels level)
        {
            switch (level)
            {
                case LogLevels.Debug:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevels.Pass:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevels.Warning:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevels.Error:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
        }
    }

    
}
