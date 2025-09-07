using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

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
            var description = GetAttribute<AssemblyDescriptionAttribute, string?>(a => a.Description) ?? "Утилита для центрирования и управления активным окном Windows.";

            var title = new Label { Text = productName, Font = new Font(Font, FontStyle.Bold), AutoSize = true, Padding = new Padding(0, 8, 0, 4) };
            var ver = new Label { Text = $"Версия: {version}", AutoSize = true };
            var dev = new Label { Text = string.IsNullOrWhiteSpace(company) ? "Разработчик: —" : $"Разработчик: {company}", AutoSize = true };
            var copy = new Label { Text = copyright, AutoSize = true };
            var desc = new Label { Text = description, AutoSize = true, MaximumSize = new Size(420, 0) };

            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12, 6, 12, 6) };

            var content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(16)
            };
            content.Controls.Add(title);
            content.Controls.Add(ver);
            content.Controls.Add(dev);
            content.Controls.Add(copy);
            content.Controls.Add(new Label { Text = "", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
            content.Controls.Add(desc);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(16) };
            buttons.Controls.Add(ok);

            Controls.Add(content);
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


