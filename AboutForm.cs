using System.Diagnostics;
using System.Reflection;

namespace Winnic
{
    internal sealed class AboutForm : Form
    {
        public AboutForm()
        {
            Text = "О программе Winnic";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 260);

            var productName = GetAttribute<AssemblyProductAttribute, string?>(a => a.Product) ?? "Winnic";
            var version = GetVersionString();
            var company = GetAttribute<AssemblyCompanyAttribute, string?>(a => a.Company) ?? "";
            var copyright = GetAttribute<AssemblyCopyrightAttribute, string?>(a => a.Copyright)
                            ?? $"© {DateTime.Now:yyyy} {company}";
            var description = GetAttribute<AssemblyDescriptionAttribute, string?>(a => a.Description) ?? "Утилита для управления расположением активного окна в Windows.";

            var title = new Label { Text = productName, Font = new Font(Font, FontStyle.Bold), AutoSize = true, Padding = new Padding(0, 8, 0, 4), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            var ver = new Label { Text = $"Версия: {version}", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            var dev = new Label { Text = string.IsNullOrWhiteSpace(company) ? "Разработчик: —" : $"Разработчик: blanergol", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            var copy = new Label { Text = copyright, AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            var desc = new Label { Text = description, AutoSize = true, MaximumSize = new Size(420, 0), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };

            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12, 6, 12, 6) };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                Padding = new Padding(16)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // title
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // desc
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // spacer
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // version
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // developer
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // copyright

            var spacer = new Panel { Dock = DockStyle.Fill };

            layout.Controls.Add(title, 0, 0);
            layout.Controls.Add(desc, 0, 1);
            layout.Controls.Add(spacer, 0, 2);
            layout.Controls.Add(ver, 0, 3);
            layout.Controls.Add(dev, 0, 4);
            layout.Controls.Add(copy, 0, 5);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(16) };
            buttons.Controls.Add(ok);

            Controls.Add(layout);
            Controls.Add(buttons);

            AcceptButton = ok;
        }

        private static string GetVersionString()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return fvi.ProductVersion ?? fvi.FileVersion ?? asm.GetName().Version?.ToString() ?? "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        private static TOut? GetAttribute<TAttr, TOut>(Func<TAttr, TOut> getter) where TAttr : Attribute
        {
            var asm = Assembly.GetExecutingAssembly();
            var attr = asm.GetCustomAttribute<TAttr>();
            return attr == null ? default : getter(attr);
        }
    }
}


