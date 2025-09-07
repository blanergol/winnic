using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Winnic
{
    internal sealed class WindowCenterService
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }

        public void CenterForegroundWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("Нет активного окна");

            if (!GetWindowRect(hwnd, out var rect))
                throw new InvalidOperationException("Не удалось получить размер окна");

            var hmon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            var mi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            bool gotMonitor = hmon != IntPtr.Zero && GetMonitorInfo(hmon, ref mi);

            // Фолбэк: если GetMonitorInfo не сработал, используем Screen.FromHandle
            if (!gotMonitor)
            {
                var screen = Screen.FromHandle(hwnd);
                var wa = screen.WorkingArea;
                mi.rcWork = new RECT { Left = wa.Left, Top = wa.Top, Right = wa.Right, Bottom = wa.Bottom };
            }

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            int targetX = mi.rcWork.Left + (mi.rcWork.Right - mi.rcWork.Left - width) / 2;
            int targetY = mi.rcWork.Top + (mi.rcWork.Bottom - mi.rcWork.Top - height) / 2;

            if (!MoveWindow(hwnd, targetX, targetY, width, height, true))
                throw new InvalidOperationException("Не удалось переместить окно");
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWNORMAL = 1;

        public void MaximizeForegroundWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("Нет активного окна");
            ShowWindow(hwnd, SW_SHOWMAXIMIZED);
        }

        // --- Сохранение/восстановление положения окна ---
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        private WINDOWPLACEMENT? _lastPlacement;

        public void SaveForegroundPlacement()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero) return;
            if (GetWindowPlacement(hwnd, out var wp))
            {
                wp.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                _lastPlacement = wp;
            }
        }

        public void RestoreLastPlacement()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("Нет активного окна");
            if (_lastPlacement == null)
                return;
            var wp = _lastPlacement.Value;
            wp.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            if (!SetWindowPlacement(hwnd, ref wp))
            {
                // если не удалось, просто показать как обычное
                ShowWindow(hwnd, SW_SHOWNORMAL);
            }
        }
    }
}


