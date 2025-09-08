using System.Drawing.Drawing2D;

namespace Winnic
{
    internal static class IconFactory
    {
        public static Icon CreateAppIcon()
        {
            using var bmp = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Simple icon: square with a centering cross
                using var bg = new SolidBrush(Color.FromArgb(30, 144, 255));
                g.FillRectangle(bg, 0, 0, 32, 32);
                using var pen = new Pen(Color.White, 3);
                g.DrawLine(pen, 16, 4, 16, 28);
                g.DrawLine(pen, 4, 16, 28, 16);
                g.DrawEllipse(Pens.White, 10, 10, 12, 12);
            }

            // Create an icon and immediately clone it to release the original handle
            var hIcon = bmp.GetHicon();
            try
            {
                using var tmpIcon = Icon.FromHandle(hIcon);
                return (Icon)tmpIcon.Clone();
            }
            finally
            {
                // Destroy the handle to avoid a leak
                NativeMethods.DestroyIcon(hIcon);
            }
        }
    }

    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        internal static extern bool DestroyIcon(IntPtr hIcon);
    }
}


