using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static VoiceAssistant.Handles.Keyboard;

namespace VoiceAssistant.Handles
{
    class PressKeyHandles
    {
        static int sleepTime = 40;

        static Dictionary<string, ScanCodeShort> keyDictionary = CreateKeyDictionary();


        public static void PressKey(string button)
        {
            ScanCodeShort scanCodeShort = GetScanCodeShort(button);

            Keyboard.SendPress(scanCodeShort);

            Action<object> unpress = (code) => WaitAndUnpress((ScanCodeShort)code);

            Task task = new Task(unpress, scanCodeShort);
        }

        static void WaitAndUnpress(ScanCodeShort scanCodeShort)
        {
            Thread.Sleep(sleepTime);
            Keyboard.SendUnpress(scanCodeShort);
        }

        public static ScanCodeShort GetScanCodeShort(string key)
        {
            key = key.ToUpper();

            if (!keyDictionary.ContainsKey(key))
            {
                Debug.LogError("Клавиши \"" + key + "\" не существует");
                return 0;
            }

            return keyDictionary[key];

        }


        public static Dictionary<string, ScanCodeShort> CreateKeyDictionary()
        {
            Dictionary<string, ScanCodeShort> keyDictionary = new Dictionary<string, ScanCodeShort>
            {
                { "LBUTTON", (ScanCodeShort)0 },
                { "RBUTTON", (ScanCodeShort)0 },
                { "CANCEL", (ScanCodeShort)70 },
                { "MBUTTON", (ScanCodeShort)0 },
                { "XBUTTON1", (ScanCodeShort)0 },
                { "XBUTTON2", (ScanCodeShort)0 },
                { "BACK", (ScanCodeShort)14 },
                { "TAB", (ScanCodeShort)15 },
                { "CLEAR", (ScanCodeShort)76 },
                { "RETURN", (ScanCodeShort)28 },
                { "SHIFT", (ScanCodeShort)42 },
                { "CONTROL", (ScanCodeShort)29 },
                { "MENU", (ScanCodeShort)56 },
                { "PAUSE", (ScanCodeShort)0 },
                { "CAPITAL", (ScanCodeShort)58 },
                { "KANA", (ScanCodeShort)0 },
                { "HANGUL", (ScanCodeShort)0 },
                { "JUNJA", (ScanCodeShort)0 },
                { "FINAL", (ScanCodeShort)0 },
                { "HANJA", (ScanCodeShort)0 },
                { "KANJI", (ScanCodeShort)0 },
                { "ESCAPE", (ScanCodeShort)1 },
                { "CONVERT", (ScanCodeShort)0 },
                { "NONCONVERT", (ScanCodeShort)0 },
                { "ACCEPT", (ScanCodeShort)0 },
                { "MODECHANGE", (ScanCodeShort)0 },
                { "SPACE", (ScanCodeShort)57 },
                { "PRIOR", (ScanCodeShort)73 },
                { "NEXT", (ScanCodeShort)81 },
                { "END", (ScanCodeShort)79 },
                { "HOME", (ScanCodeShort)71 },
                { "LEFT", (ScanCodeShort)75 },
                { "UP", (ScanCodeShort)72 },
                { "RIGHT", (ScanCodeShort)77 },
                { "DOWN", (ScanCodeShort)80 },
                { "SELECT", (ScanCodeShort)0 },
                { "PRINT", (ScanCodeShort)0 },
                { "EXECUTE", (ScanCodeShort)0 },
                { "SNAPSHOT", (ScanCodeShort)84 },
                { "INSERT", (ScanCodeShort)82 },
                { "DELETE", (ScanCodeShort)83 },
                { "HELP", (ScanCodeShort)99 },
                { "0", (ScanCodeShort)11 },
                { "1", (ScanCodeShort)2 },
                { "2", (ScanCodeShort)3 },
                { "3", (ScanCodeShort)4 },
                { "4", (ScanCodeShort)5 },
                { "5", (ScanCodeShort)6 },
                { "6", (ScanCodeShort)7 },
                { "7", (ScanCodeShort)8 },
                { "8", (ScanCodeShort)9 },
                { "9", (ScanCodeShort)10 },
                { "A", (ScanCodeShort)30 },
                { "B", (ScanCodeShort)48 },
                { "C", (ScanCodeShort)46 },
                { "D", (ScanCodeShort)32 },
                { "E", (ScanCodeShort)18 },
                { "F", (ScanCodeShort)33 },
                { "G", (ScanCodeShort)34 },
                { "H", (ScanCodeShort)35 },
                { "I", (ScanCodeShort)23 },
                { "J", (ScanCodeShort)36 },
                { "K", (ScanCodeShort)37 },
                { "L", (ScanCodeShort)38 },
                { "M", (ScanCodeShort)50 },
                { "N", (ScanCodeShort)49 },
                { "O", (ScanCodeShort)24 },
                { "P", (ScanCodeShort)25 },
                { "Q", (ScanCodeShort)16 },
                { "R", (ScanCodeShort)19 },
                { "S", (ScanCodeShort)31 },
                { "T", (ScanCodeShort)20 },
                { "U", (ScanCodeShort)22 },
                { "V", (ScanCodeShort)47 },
                { "W", (ScanCodeShort)17 },
                { "X", (ScanCodeShort)45 },
                { "Y", (ScanCodeShort)21 },
                { "Z", (ScanCodeShort)44 },
                { "А", (ScanCodeShort)33 },
                { "Б", (ScanCodeShort)51 },
                { "В", (ScanCodeShort)32 },
                { "Г", (ScanCodeShort)22 },
                { "Д", (ScanCodeShort)38 },
                { "Е", (ScanCodeShort)20 },
                { "Ж", (ScanCodeShort)39 },
                { "З", (ScanCodeShort)25 },
                { "И", (ScanCodeShort)48 },
                { "Й", (ScanCodeShort)16 },
                { "К", (ScanCodeShort)19 },
                { "Л", (ScanCodeShort)37 },
                { "М", (ScanCodeShort)47 },
                { "Н", (ScanCodeShort)21 },
                { "О", (ScanCodeShort)36 },
                { "П", (ScanCodeShort)34 },
                { "Р", (ScanCodeShort)35 },
                { "С", (ScanCodeShort)46 },
                { "Т", (ScanCodeShort)49 },
                { "У", (ScanCodeShort)18 },
                { "Х", (ScanCodeShort)26 },
                { "Ф", (ScanCodeShort)30 },
                { "Ц", (ScanCodeShort)17 },
                { "Ч", (ScanCodeShort)45 },
                { "Ш", (ScanCodeShort)23 },
                { "Щ", (ScanCodeShort)24 },
                { "Ъ", (ScanCodeShort)27 },
                { "Ы", (ScanCodeShort)31 },
                { "Ь", (ScanCodeShort)50 },
                { "Э", (ScanCodeShort)40 },
                { "Ю", (ScanCodeShort)52 },
                { "Я", (ScanCodeShort)44 },
                { "LWIN", (ScanCodeShort)91 },
                { "RWIN", (ScanCodeShort)92 },
                { "APPS", (ScanCodeShort)93 },
                { "SLEEP", (ScanCodeShort)95 },
                { "NUMPAD0", (ScanCodeShort)82 },
                { "NUMPAD1", (ScanCodeShort)79 },
                { "NUMPAD2", (ScanCodeShort)80 },
                { "NUMPAD3", (ScanCodeShort)81 },
                { "NUMPAD4", (ScanCodeShort)75 },
                { "NUMPAD5", (ScanCodeShort)76 },
                { "NUMPAD6", (ScanCodeShort)77 },
                { "NUMPAD7", (ScanCodeShort)71 },
                { "NUMPAD8", (ScanCodeShort)72 },
                { "NUMPAD9", (ScanCodeShort)73 },
                { "MULTIPLY", (ScanCodeShort)55 },
                { "ADD", (ScanCodeShort)78 },
                { "SEPARATOR", (ScanCodeShort)0 },
                { "SUBTRACT", (ScanCodeShort)74 },
                { "DECIMAL", (ScanCodeShort)83 },
                { "DIVIDE", (ScanCodeShort)53 },
                { "F1", (ScanCodeShort)59 },
                { "F2", (ScanCodeShort)60 },
                { "F3", (ScanCodeShort)61 },
                { "F4", (ScanCodeShort)62 },
                { "F5", (ScanCodeShort)63 },
                { "F6", (ScanCodeShort)64 },
                { "F7", (ScanCodeShort)65 },
                { "F8", (ScanCodeShort)66 },
                { "F9", (ScanCodeShort)67 },
                { "F10", (ScanCodeShort)68 },
                { "F11", (ScanCodeShort)87 },
                { "F12", (ScanCodeShort)88 },
                { "F13", (ScanCodeShort)100 },
                { "F14", (ScanCodeShort)101 },
                { "F15", (ScanCodeShort)102 },
                { "F16", (ScanCodeShort)103 },
                { "F17", (ScanCodeShort)104 },
                { "F18", (ScanCodeShort)105 },
                { "F19", (ScanCodeShort)106 },
                { "F20", (ScanCodeShort)107 },
                { "F21", (ScanCodeShort)108 },
                { "F22", (ScanCodeShort)109 },
                { "F23", (ScanCodeShort)110 },
                { "F24", (ScanCodeShort)118 },
                { "NUMLOCK", (ScanCodeShort)69 },
                { "SCROLL", (ScanCodeShort)70 },
                { "LSHIFT", (ScanCodeShort)42 },
                { "RSHIFT", (ScanCodeShort)54 },
                { "LCONTROL", (ScanCodeShort)29 },
                { "RCONTROL", (ScanCodeShort)29 },
                { "LMENU", (ScanCodeShort)56 },
                { "RMENU", (ScanCodeShort)56 },
                { "BROWSER_BACK", (ScanCodeShort)106 },
                { "BROWSER_FORWARD", (ScanCodeShort)105 },
                { "BROWSER_REFRESH", (ScanCodeShort)103 },
                { "BROWSER_STOP", (ScanCodeShort)104 },
                { "BROWSER_SEARCH", (ScanCodeShort)101 },
                { "BROWSER_FAVORITES", (ScanCodeShort)102 },
                { "BROWSER_HOME", (ScanCodeShort)50 },
                { "VOLUME_MUTE", (ScanCodeShort)32 },
                { "VOLUME_DOWN", (ScanCodeShort)46 },
                { "VOLUME_UP", (ScanCodeShort)48 },
                { "MEDIA_NEXT_TRACK", (ScanCodeShort)25 },
                { "MEDIA_PREV_TRACK", (ScanCodeShort)16 },
                { "MEDIA_STOP", (ScanCodeShort)36 },
                { "MEDIA_PLAY_PAUSE", (ScanCodeShort)34 },
                { "LAUNCH_MAIL", (ScanCodeShort)108 },
                { "LAUNCH_MEDIA_SELECT", (ScanCodeShort)109 },
                { "LAUNCH_APP1", (ScanCodeShort)107 },
                { "LAUNCH_APP2", (ScanCodeShort)33 },
                { "OEM_1", (ScanCodeShort)39 },
                { "OEM_PLUS", (ScanCodeShort)13 },
                { "OEM_COMMA", (ScanCodeShort)51 },
                { "OEM_MINUS", (ScanCodeShort)12 },
                { "OEM_PERIOD", (ScanCodeShort)52 },
                { "OEM_2", (ScanCodeShort)53 },
                { "OEM_3", (ScanCodeShort)41 },
                { "OEM_4", (ScanCodeShort)26 },
                { "OEM_5", (ScanCodeShort)43 },
                { "OEM_6", (ScanCodeShort)27 },
                { "OEM_7", (ScanCodeShort)40 },
                { "OEM_8", (ScanCodeShort)0 },
                { "OEM_102", (ScanCodeShort)86 },
                { "PROCESSKEY", (ScanCodeShort)0 },
                { "PACKET", (ScanCodeShort)0 },
                { "ATTN", (ScanCodeShort)0 },
                { "CRSEL", (ScanCodeShort)0 },
                { "EXSEL", (ScanCodeShort)0 },
                { "EREOF", (ScanCodeShort)93 },
                { "PLAY", (ScanCodeShort)0 },
                { "ZOOM", (ScanCodeShort)98 },
                { "NONAME", (ScanCodeShort)0 },
                { "PA1", (ScanCodeShort)0 },
                { "OEM_CLEAR", (ScanCodeShort)0 }
            };

            return keyDictionary;
        }
    }
}
