using System;

namespace VoiceAssistant
{
    class Debug
    {
        public static Form1 form1;
        static bool duplicateToConsole = true;

        public static void Log(string message)
        {
            if (duplicateToConsole)
                Console.WriteLine(message);

            form1.WriteMassage(message);
        }

        public static void LogWarning(string message)
        {
            if (duplicateToConsole)
                Console.WriteLine("[WARNING] " + message);

            form1.WriteMassage("[WARNING] " + message);
        }

        public static void LogError(string message)
        {
            if (duplicateToConsole)
                Console.WriteLine("[ERROR] " + message);

            form1.WriteMassage("[ERROR] " + message);
        }

        public static void ClearLog()
        {
            form1.ClearLog();
        }
    }
}
