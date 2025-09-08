using System.Runtime.InteropServices;

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
                throw new InvalidOperationException("No active window");

            if (!GetWindowRect(hwnd, out var rect))
                throw new InvalidOperationException("Failed to get window rectangle");

            var hmon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            var mi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            bool gotMonitor = hmon != IntPtr.Zero && GetMonitorInfo(hmon, ref mi);

            // Fallback: if GetMonitorInfo failed, use Screen.FromHandle
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
                throw new InvalidOperationException("Failed to move window");
        }

        public void SnapForegroundWindowLeftHalf()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("No active window");

            if (!GetWindowRect(hwnd, out var rect))
                throw new InvalidOperationException("Failed to get window rectangle");

            var hmon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            var mi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            bool gotMonitor = hmon != IntPtr.Zero && GetMonitorInfo(hmon, ref mi);

            if (!gotMonitor)
            {
                var screen = Screen.FromHandle(hwnd);
                var wa = screen.WorkingArea;
                mi.rcWork = new RECT { Left = wa.Left, Top = wa.Top, Right = wa.Right, Bottom = wa.Bottom };
            }

            int workWidth = mi.rcWork.Right - mi.rcWork.Left;
            int workHeight = mi.rcWork.Bottom - mi.rcWork.Top;
            int targetWidth = workWidth / 2;
            int targetHeight = workHeight;
            int targetX = mi.rcWork.Left;
            int targetY = mi.rcWork.Top;

            // First restore the window to normal state
            ShowWindow(hwnd, SW_SHOWNORMAL);
            if (!MoveWindow(hwnd, targetX, targetY, targetWidth, targetHeight, true))
                throw new InvalidOperationException("Failed to move window");
        }

        public void SnapForegroundWindowRightHalf()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("No active window");

            if (!GetWindowRect(hwnd, out var rect))
                throw new InvalidOperationException("Failed to get window rectangle");

            var hmon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            var mi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            bool gotMonitor = hmon != IntPtr.Zero && GetMonitorInfo(hmon, ref mi);

            if (!gotMonitor)
            {
                var screen = Screen.FromHandle(hwnd);
                var wa = screen.WorkingArea;
                mi.rcWork = new RECT { Left = wa.Left, Top = wa.Top, Right = wa.Right, Bottom = wa.Bottom };
            }

            int workWidth = mi.rcWork.Right - mi.rcWork.Left;
            int workHeight = mi.rcWork.Bottom - mi.rcWork.Top;
            int targetWidth = workWidth / 2;
            int targetHeight = workHeight;
            int targetX = mi.rcWork.Left + workWidth - targetWidth;
            int targetY = mi.rcWork.Top;

            ShowWindow(hwnd, SW_SHOWNORMAL);
            if (!MoveWindow(hwnd, targetX, targetY, targetWidth, targetHeight, true))
                throw new InvalidOperationException("Failed to move window");
        }

        public void SnapForegroundWindowTopHalf()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("No active window");

            if (!GetWindowRect(hwnd, out var rect))
                throw new InvalidOperationException("Failed to get window rectangle");

            var hmon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            var mi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            bool gotMonitor = hmon != IntPtr.Zero && GetMonitorInfo(hmon, ref mi);

            if (!gotMonitor)
            {
                var screen = Screen.FromHandle(hwnd);
                var wa = screen.WorkingArea;
                mi.rcWork = new RECT { Left = wa.Left, Top = wa.Top, Right = wa.Right, Bottom = wa.Bottom };
            }

            int workWidth = mi.rcWork.Right - mi.rcWork.Left;
            int workHeight = mi.rcWork.Bottom - mi.rcWork.Top;
            int targetWidth = workWidth;
            int targetHeight = workHeight / 2;
            int targetX = mi.rcWork.Left;
            int targetY = mi.rcWork.Top;

            ShowWindow(hwnd, SW_SHOWNORMAL);
            if (!MoveWindow(hwnd, targetX, targetY, targetWidth, targetHeight, true))
                throw new InvalidOperationException("Failed to move window");
        }

        public void SnapForegroundWindowBottomHalf()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("No active window");

            if (!GetWindowRect(hwnd, out var rect))
                throw new InvalidOperationException("Failed to get window rectangle");

            var hmon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            var mi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            bool gotMonitor = hmon != IntPtr.Zero && GetMonitorInfo(hmon, ref mi);

            if (!gotMonitor)
            {
                var screen = Screen.FromHandle(hwnd);
                var wa = screen.WorkingArea;
                mi.rcWork = new RECT { Left = wa.Left, Top = wa.Top, Right = wa.Right, Bottom = wa.Bottom };
            }

            int workWidth = mi.rcWork.Right - mi.rcWork.Left;
            int workHeight = mi.rcWork.Bottom - mi.rcWork.Top;
            int targetWidth = workWidth;
            int targetHeight = workHeight / 2;
            int targetX = mi.rcWork.Left;
            int targetY = mi.rcWork.Top + workHeight - targetHeight;

            ShowWindow(hwnd, SW_SHOWNORMAL);
            if (!MoveWindow(hwnd, targetX, targetY, targetWidth, targetHeight, true))
                throw new InvalidOperationException("Failed to move window");
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_MINIMIZE = 6;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint WM_CLOSE = 0x0010;

        public void MaximizeForegroundWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("No active window");
            ShowWindow(hwnd, SW_SHOWMAXIMIZED);
        }

        // --- Save/restore window placement ---
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
                throw new InvalidOperationException("No active window");
            if (_lastPlacement == null)
                return;
            var wp = _lastPlacement.Value;
            wp.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            if (!SetWindowPlacement(hwnd, ref wp))
            {
                // If failed, just show as normal
                ShowWindow(hwnd, SW_SHOWNORMAL);
            }
        }

        private IntPtr _lastMinimizedWindow;

        public void MinimizeForegroundWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("No active window");
            _lastMinimizedWindow = hwnd;
            ShowWindow(hwnd, SW_MINIMIZE);
        }

        public void RestoreLastMinimizedWindow()
        {
            if (_lastMinimizedWindow == IntPtr.Zero)
                throw new InvalidOperationException("No minimized window to restore");
            ShowWindow(_lastMinimizedWindow, SW_SHOWNORMAL);
            SetForegroundWindow(_lastMinimizedWindow);
        }

        public void CloseForegroundWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                throw new InvalidOperationException("No active window");
            SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
    }
}


