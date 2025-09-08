using System.Drawing.Drawing2D;

namespace Winnic
{
    internal static class IconFactory
    {
        public static Icon CreateAppIcon()
        {
            var svgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "favicon.svg");
            if (!File.Exists(svgPath))
            {
                // Fallback to simple generated icon if SVG not found
                using var bmpFallback = new Bitmap(32, 32);
                using (var g = Graphics.FromImage(bmpFallback))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);
                    using var bg = new SolidBrush(Color.FromArgb(30, 144, 255));
                    g.FillRectangle(bg, 0, 0, 32, 32);
                    using var pen = new Pen(Color.White, 3);
                    g.DrawLine(pen, 16, 4, 16, 28);
                    g.DrawLine(pen, 4, 16, 28, 16);
                    g.DrawEllipse(Pens.White, 10, 10, 12, 12);
                }
                var hIconFallback = bmpFallback.GetHicon();
                try
                {
                    using var tmpIcon = Icon.FromHandle(hIconFallback);
                    return (Icon)tmpIcon.Clone();
                }
                finally
                {
                    NativeMethods.DestroyIcon(hIconFallback);
                }
            }

            // Render SVG to bitmap and convert to icon
            var svgDoc = Svg.SvgDocument.Open(svgPath);
            using var bmp = svgDoc.Draw(32, 32);
            var hIcon = bmp.GetHicon();
            try
            {
                using var tmpIcon = Icon.FromHandle(hIcon);
                return (Icon)tmpIcon.Clone();
            }
            finally
            {
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


