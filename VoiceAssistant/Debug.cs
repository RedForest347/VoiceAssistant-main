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

        public static void SetMaxValueProgressBar(int value)
        {
            form1.ProgressMaxValue(value);
        }

        public static void SetCurrentValueProgressBar(int value)
        {
            form1.ProgressCurrentValue(value);
        }

        public static void ZeroingProgressBar()
        {
            form1.ProgressCurrentValue(0);
        }

        public static void PlusProgressBar(int value)
        {

        }

        public static void ClearLog()
        {
            form1.ClearLog();
        }
    }
}
