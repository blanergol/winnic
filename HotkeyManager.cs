using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Winnic
{
    [Flags]
    internal enum HotkeyModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }

    internal sealed class HotkeyManager : NativeWindow, IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private int _nextId = 0xA100;
        private readonly Dictionary<int, Action> _callbacks = new Dictionary<int, Action>();

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public int Register(HotkeyModifiers modifiers, Keys key, Action callback)
        {
            if (Handle == IntPtr.Zero)
            {
                CreateHandle(new CreateParams());
            }

            int id = _nextId++;
            if (!RegisterHotKey(Handle, id, (int)modifiers, (int)key))
            {
                throw new InvalidOperationException("Не удалось зарегистрировать горячую клавишу. Возможно, она уже занята.");
            }
            _callbacks[id] = callback;
            return id;
        }

        public void Unregister()
        {
            if (Handle != IntPtr.Zero)
            {
                foreach (var id in _callbacks.Keys.ToArray())
                {
                    UnregisterHotKey(Handle, id);
                }
                _callbacks.Clear();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (_callbacks.TryGetValue(id, out var cb))
                {
                    cb();
                }
            }
            base.WndProc(ref m);
        }

        public void Dispose()
        {
            Unregister();
            if (Handle != IntPtr.Zero)
            {
                DestroyHandle();
            }
        }
    }
}


