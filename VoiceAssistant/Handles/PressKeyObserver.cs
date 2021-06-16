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
    class PressKeyObserver
    {
        public static event Action<Keys> onKeyPress;

        public static void Start()
        {
            StartHook();
        }

        public static void Stop()
        {
            Unhook();
        }







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
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                onKeyPress?.Invoke(key);
                Debug.Log("press key " + key);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
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
