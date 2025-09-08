using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

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
            ClientSize = new Size(460, 410);

            var version = GetVersionString();
            var versionDisplay = ShortenHashInVersion(version);
            var company = GetAttribute<AssemblyCompanyAttribute, string?>(a => a.Company) ?? "";
            var copyright = GetAttribute<AssemblyCopyrightAttribute, string?>(a => a.Copyright)
                            ?? $"© {DateTime.Now:yyyy} {company}";
            var description = GetAttribute<AssemblyDescriptionAttribute, string?>(a => a.Description) ?? "Утилита для управления расположением активного окна в Windows.";

            var ver = new Label { Text = $"Версия: {versionDisplay}", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Margin = Padding.Empty };
            var dev = new Label { Text = string.IsNullOrWhiteSpace(company) ? "Разработчик: —" : $"Разработчик: blanergol", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            var copy = new Label { Text = copyright, AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            var desc = new Label { Text = description, AutoSize = true, MaximumSize = new Size(420, 0), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Margin = Padding.Empty };

            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12, 6, 12, 6) };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                Padding = new Padding(16)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // desc
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // spacer
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // version
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // developer
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // copyright

            var spacer = new Panel { Dock = DockStyle.Fill };

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

        private static string ShortenHashInVersion(string version)
        {
            try
            {
                // Find any long hex hash (9+ chars) and shorten it to 7 chars
                var match = Regex.Match(version, @"(?<![0-9a-fA-F])[0-9a-fA-F]{9,}(?![0-9a-fA-F])");
                if (match.Success)
                {
                    var shortHash = match.Value.Substring(0, 7);
                    return version.Replace(match.Value, shortHash);
                }
                return version;
            }
            catch
            {
                return version;
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


