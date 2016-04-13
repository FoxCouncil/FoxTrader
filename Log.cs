using System;
using System.Diagnostics;

namespace FoxTrader
{
    class Log
    {
        public static void Initialize()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("game.log"));
            Trace.AutoFlush = true;

            Info("====================================================================================================", "LOGLOG");
        }

        public static void Error(string c_message, string c_module)
        {
            WriteEntry(c_message, "ERROR", c_module);
        }

        public static void Error(Exception c_exception, string c_module)
        {
            WriteEntry(c_exception.Message, "ERROR", c_module);
        }

        public static void Warning(string c_message, string c_module)
        {
            WriteEntry(c_message, "WARNING", c_module);
        }

        public static void Info(string c_message, string c_module)
        {
            WriteEntry(c_message, "INFO", c_module);
        }

        private static void WriteEntry(string c_message, string c_type, string c_module)
        {
            Trace.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]-[{c_type}]-({c_module}): {c_message}");
        }
    }
}
