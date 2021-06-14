using System;

namespace VoiceAssistant
{
    class Debug
    {
        public static Form1 form1;

        public static void Log(string message)
        {
            form1.WriteMassage(message);
        }

        public static void LogWarning(string message)
        {
            form1.WriteMassage("[WARNING] " + message);
        }

        public static void LogError(string message)
        {
            form1.WriteMassage("[ERROR] " + message);
        }

        public static void ClearLog()
        {
            form1.ClearLog();
        }
    }
}
