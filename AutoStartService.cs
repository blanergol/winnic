using Microsoft.Win32;

namespace Winnic
{
    internal sealed class AutoStartService
    {
        private const string RunKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string AppName = "Winnic";

        public void Apply(bool enable)
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKey, writable: true) ?? Registry.CurrentUser.CreateSubKey(RunKey);
            if (key == null) return;

            if (enable)
            {
                var exePath = GetExecutablePath();
                key.SetValue(AppName, exePath);
            }
            else
            {
                key.DeleteValue(AppName, false);
            }
        }

        public bool IsEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(RunKey, writable: false);
            if (key == null) return false;
            return key.GetValue(AppName) is string s && !string.IsNullOrWhiteSpace(s);
        }

        private static string GetExecutablePath()
        {
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
            // If the path contains spaces — wrap it in quotes
            if (!string.IsNullOrEmpty(path) && path.Contains(' '))
            {
                return "\"" + path + "\"";
            }
            return path;
        }
    }
}


