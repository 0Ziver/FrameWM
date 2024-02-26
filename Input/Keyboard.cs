using System.Runtime.InteropServices;
using System.Diagnostics;
using Frame.Exec;
using Frame.Configs;
using static Frame.Helpers.LibImports;
using static System.Diagnostics.Process;

namespace Frame.Input
{
    class Keyboard : IInit, IOnExit
    {
        public static event Action<Hotkey> OnSendHotkey;


        private LowLevelKeyboardProc? _proc;
        private IntPtr _hookID;


        public void Init()
        {
            _proc = HookCallback;
            _hookID = InstallHook(_proc);
            Console.WriteLine($@"{typeof(Keyboard)} is ready");

        }

        private bool IS_MOD_KEY_PRESSED()
        {
            return (GetAsyncKeyState((int)KeyCode.LWIN) & 0x8000) != 0;
        }
        private bool IS_SEC_KEY_PRESSED()
        {
            return (GetAsyncKeyState((int)KeyCode.LSHIFT) & 0x8000) != 0;
        }
        private IntPtr InstallHook(LowLevelKeyboardProc proc)
        {
            using ProcessModule? curModule = GetCurrentProcess().MainModule;
            return SetWindowsHookEx((int)KEYBOARD_HOOKS.LL_REG, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
        
        List<KeyCode> keys = new(3);
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KeyCode VKC = (KeyCode)Marshal.ReadInt32(lParam);
            if (nCode >= 0)
            {
                if (CheckSystemKey(VKC))
                {
                    if (wParam == (IntPtr)KEYBOARD_HOOKS.KEYDOWN)
                    {
                        keys.Add(VKC);
                    }
                    else if (wParam == (IntPtr)KEYBOARD_HOOKS.KEYUP)
                    {

                        bool mod = IS_MOD_KEY_PRESSED();
                        bool sec = IS_SEC_KEY_PRESSED();

                        if (mod || sec)
                        {
                            ProcessKeys(mod, sec, VKC);
                        }
                        else
                        {
                            
                            ProcessKeys(false, false, VKC);
                        }
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void ProcessKeys(bool m = false, bool s = false, KeyCode VKC = KeyCode.EMPTY)
        {
            if (CheckSystemKey(VKC))
            {
                for (int i = 1; i <= keys.Count; i++)
                {
                    Send(m, s, keys.ToArray());
                    keys.Clear();
                    return;
                }
            }
            keys.Clear();
        }
        private void Send(bool m = false, bool s = false, params KeyCode[] keys)
        {
            
            /* Console.WriteLine($" MOD: {m} \n SEC: {s} " +
                 $"\n K1: {(keys.Length > 0 ? keys[0] : KeyCode.EMPTY)} " +
                 $"\n K2: {(keys.Length > 1 ? keys[1] : KeyCode.EMPTY)} " +
                 $"\n K3: {(keys.Length > 2 ? keys[2] : KeyCode.EMPTY)} \n");*/

        }


        private bool CheckSystemKey(KeyCode inkey)
        {
            KeyCode[] keys = {
                KeyCode.LSHIFT,
                KeyCode.LWIN,
                KeyCode.LMENU,
                KeyCode.LCONTROL,
                KeyCode.RSHIFT,
                KeyCode.RWIN,
                KeyCode.RMENU,
                KeyCode.RCONTROL,
            };
            return !keys.Contains(inkey);
        }


        public void OnExit()
        {
            UnhookWindowsHookEx(_hookID);
        }
    }
}