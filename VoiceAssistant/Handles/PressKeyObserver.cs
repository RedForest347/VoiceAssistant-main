using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static VoiceAssistant.Handles.User32;

namespace VoiceAssistant.Handles
{
    static class PressKeyObserver
    {
        public static event Action<Keys> onKeyDown;
        public static event Action<Keys> onKeyPress;
        public static event Action<Keys> onKeyUp;

        private static Dictionary<Keys, bool> pressedKeys;

        public static void Start()
        {
            InitPressedKessDictionary();
            StartHook();
        }

        public static void Stop()
        {
            Unhook();
        }

        private static void InitPressedKessDictionary()
        {
            pressedKeys = new Dictionary<Keys, bool>();
        }


        #region Low level keyboard functional


        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static bool wasStarted = false;

        private static void StartHook()
        {
            _hookID = SetHook(_proc);
            wasStarted = true;
        }

        private static void Unhook()
        {
            if (wasStarted)
            {
                UnhookWindowsHookEx(_hookID);
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (wParam == (IntPtr)WM_KEYDOWN && keyWasDown(key))
                {
                    KeyPress(key);
                }
                else if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    KeyDown(key);
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    KeyUp(key);
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        #endregion Low level keyboard functional


        public static bool KeyPressed(Keys key)
        {
            return keyWasDown(key);
        }


        //была ли нажата клавиша, заодно, эта функция инициализирует недостающие Keys
        static bool keyWasDown(Keys key)
        {
            if (!pressedKeys.ContainsKey(key)) 
                pressedKeys.Add(key, false);

            return pressedKeys[key];

        }

        static void KeyDown(Keys key)
        {
            pressedKeys[key] = true;
            onKeyDown?.Invoke(key);
        }

        static void KeyPress(Keys key)
        {
            onKeyPress?.Invoke(key);
        }

        static void KeyUp(Keys key)
        {
            pressedKeys[key] = false;
            onKeyUp?.Invoke(key);
        }


        #region Others


        internal enum HookType : int
        {
            ///
            WH_JOURNALRECORD = 0,
            ///
            WH_JOURNALPLAYBACK = 1,
            ///
            WH_KEYBOARD = 2,
            ///
            WH_GETMESSAGE = 3,
            ///
            WH_CALLWNDPROC = 4,
            ///
            WH_CBT = 5,
            ///
            WH_SYSMSGFILTER = 6,
            ///
            WH_MOUSE = 7,
            ///
            WH_HARDWARE = 8,
            ///
            WH_DEBUG = 9,
            ///
            WH_SHELL = 10,
            ///
            WH_FOREGROUNDIDLE = 11,
            ///
            WH_CALLWNDPROCRET = 12,
            ///
            WH_KEYBOARD_LL = 13,
            ///
            WH_MOUSE_LL = 14
        }



        #endregion



    }
}
