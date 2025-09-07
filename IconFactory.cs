using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

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

                // Простая иконка: квадрат с крестом-центровкой
                using var bg = new SolidBrush(Color.FromArgb(30, 144, 255));
                g.FillRectangle(bg, 0, 0, 32, 32);
                using var pen = new Pen(Color.White, 3);
                g.DrawLine(pen, 16, 4, 16, 28);
                g.DrawLine(pen, 4, 16, 28, 16);
                g.DrawEllipse(Pens.White, 10, 10, 12, 12);
            }

            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            using var iconBmp = new Bitmap(bmp);
            return Icon.FromHandle(iconBmp.GetHicon());
        }
    }
}


