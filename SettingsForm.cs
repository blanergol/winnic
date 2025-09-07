using System;
using System.Drawing;
using System.Windows.Forms;

namespace Winnic
{
    internal sealed class SettingsForm : Form
    {
        private readonly CheckBox _chkCtrl = new CheckBox { Text = "Ctrl" };
        private readonly CheckBox _chkAlt = new CheckBox { Text = "Alt", Checked = true, Enabled = false };
        private readonly CheckBox _chkShift = new CheckBox { Text = "Shift" };
        private readonly CheckBox _chkWin = new CheckBox { Text = "Win" };
        private readonly ComboBox _cmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

        private readonly CheckBox _mChkCtrl = new CheckBox { Text = "Ctrl" };
        private readonly CheckBox _mChkAlt = new CheckBox { Text = "Alt", Checked = true, Enabled = false };
        private readonly CheckBox _mChkShift = new CheckBox { Text = "Shift" };
        private readonly CheckBox _mChkWin = new CheckBox { Text = "Win" };
        private readonly ComboBox _mCmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

        private readonly CheckBox _chkAutostart = new CheckBox { Text = "Автозапуск при старте Windows", AutoSize = true };
        private readonly Button _btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12, 6, 12, 6) };
        private readonly Button _btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12, 6, 12, 6) };

        // Третий хоткей: восстановление
        private readonly CheckBox _rChkCtrl = new CheckBox { Text = "Ctrl" };
        private readonly CheckBox _rChkAlt = new CheckBox { Text = "Alt", Checked = true, Enabled = false };
        private readonly CheckBox _rChkShift = new CheckBox { Text = "Shift" };
        private readonly CheckBox _rChkWin = new CheckBox { Text = "Win" };
        private readonly ComboBox _rCmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

        public AppSettings CurrentSettings { get; private set; }

        public SettingsForm(AppSettings initial)
        {
            CurrentSettings = initial;
            Text = "Настройки Winnic";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(540, 480);
            MinimumSize = new Size(540, 360);

            var modsPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            modsPanel.Controls.AddRange(new Control[] { _chkCtrl, _chkAlt, _chkShift, _chkWin });

            var keyPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            keyPanel.Controls.Add(new Label { Text = "Центрирование — клавиша:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) });
            _cmbKey.Width = 200;
            keyPanel.Controls.Add(_cmbKey);

            var mModsPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            mModsPanel.Controls.AddRange(new Control[] { _mChkCtrl, _mChkAlt, _mChkShift, _mChkWin });

            var mKeyPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            mKeyPanel.Controls.Add(new Label { Text = "Разворот — клавиша:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) });
            _mCmbKey.Width = 200;
            mKeyPanel.Controls.Add(_mCmbKey);

            var rModsPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            rModsPanel.Controls.AddRange(new Control[] { _rChkCtrl, _rChkAlt, _rChkShift, _rChkWin });

            var rKeyPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            rKeyPanel.Controls.Add(new Label { Text = "Восстановление — клавиша:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) });
            _rCmbKey.Width = 200;
            rKeyPanel.Controls.Add(_rCmbKey);

            var autostartPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10, 10, 10, 12) };
            autostartPanel.Controls.Add(_chkAutostart);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            buttons.Controls.AddRange(new Control[] { _btnOk, _btnCancel });

            var content = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            content.Controls.Add(autostartPanel);
            content.Controls.Add(rKeyPanel);
            content.Controls.Add(rModsPanel);
            content.Controls.Add(mKeyPanel);
            content.Controls.Add(mModsPanel);
            content.Controls.Add(keyPanel);
            content.Controls.Add(modsPanel);

            Controls.Add(content);
            Controls.Add(buttons);

            Load += OnLoad;
            _btnOk.Click += OnOk;
            AcceptButton = _btnOk;
            CancelButton = _btnCancel;

            _cmbKey.FormattingEnabled = true;
            _mCmbKey.FormattingEnabled = true;
            _cmbKey.Format += FormatKeyName;
            _mCmbKey.Format += FormatKeyName;
        }

        private void FormatKeyName(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is Keys k)
            {
                e.Value = k == Keys.Return ? "Enter" : k.ToString();
            }
        }

        private void OnLoad(object? sender, EventArgs e)
        {
            // Alt обязателен и фиксирован
            _chkAlt.Checked = true; _chkAlt.Enabled = false;
            _mChkAlt.Checked = true; _mChkAlt.Enabled = false;
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                if ((int)k >= (int)Keys.A && (int)k <= (int)Keys.Z || (int)k >= (int)Keys.F1 && (int)k <= (int)Keys.F24)
                {
                    _cmbKey.Items.Add(k);
                    _mCmbKey.Items.Add(k);
                    _rCmbKey.Items.Add(k);
                }
            }
            // Добавим популярные дополнительные клавиши
            if (!_cmbKey.Items.Contains(Keys.Enter)) { _cmbKey.Items.Add(Keys.Enter); }
            if (!_mCmbKey.Items.Contains(Keys.Enter)) { _mCmbKey.Items.Add(Keys.Enter); }
            if (!_rCmbKey.Items.Contains(Keys.Back)) { _rCmbKey.Items.Add(Keys.Back); }

            _chkCtrl.Checked = CurrentSettings.Modifiers.HasFlag(HotkeyModifiers.Control);
            _chkAlt.Checked = CurrentSettings.Modifiers.HasFlag(HotkeyModifiers.Alt);
            _chkShift.Checked = CurrentSettings.Modifiers.HasFlag(HotkeyModifiers.Shift);
            _chkWin.Checked = CurrentSettings.Modifiers.HasFlag(HotkeyModifiers.Win);
            if (!_cmbKey.Items.Contains(CurrentSettings.Key)) _cmbKey.Items.Add(CurrentSettings.Key);
            _cmbKey.SelectedItem = CurrentSettings.Key;
            if (_cmbKey.SelectedIndex < 0 && _cmbKey.Items.Count > 0)
                _cmbKey.SelectedIndex = 0;

            _mChkCtrl.Checked = CurrentSettings.MaximizeModifiers.HasFlag(HotkeyModifiers.Control);
            _mChkAlt.Checked = CurrentSettings.MaximizeModifiers.HasFlag(HotkeyModifiers.Alt);
            _mChkShift.Checked = CurrentSettings.MaximizeModifiers.HasFlag(HotkeyModifiers.Shift);
            _mChkWin.Checked = CurrentSettings.MaximizeModifiers.HasFlag(HotkeyModifiers.Win);
            if (!_mCmbKey.Items.Contains(CurrentSettings.MaximizeKey)) _mCmbKey.Items.Add(CurrentSettings.MaximizeKey);
            _mCmbKey.SelectedItem = CurrentSettings.MaximizeKey;
            if (_mCmbKey.SelectedIndex < 0 && _mCmbKey.Items.Count > 0)
                _mCmbKey.SelectedIndex = 0;

            _chkAutostart.Checked = CurrentSettings.AutoStart;

            // Восстановление
            _rChkCtrl.Checked = CurrentSettings.RestoreModifiers.HasFlag(HotkeyModifiers.Control);
            _rChkShift.Checked = CurrentSettings.RestoreModifiers.HasFlag(HotkeyModifiers.Shift);
            _rChkWin.Checked = CurrentSettings.RestoreModifiers.HasFlag(HotkeyModifiers.Win);
            if (!_rCmbKey.Items.Contains(CurrentSettings.RestoreKey)) _rCmbKey.Items.Add(CurrentSettings.RestoreKey);
            _rCmbKey.SelectedItem = CurrentSettings.RestoreKey;
            if (_rCmbKey.SelectedIndex < 0 && _rCmbKey.Items.Count > 0)
                _rCmbKey.SelectedIndex = 0;
        }

        private void OnOk(object? sender, EventArgs e)
        {
            var mods = HotkeyModifiers.None;
            if (_chkCtrl.Checked) mods |= HotkeyModifiers.Control;
            if (_chkAlt.Checked) mods |= HotkeyModifiers.Alt;
            if (_chkShift.Checked) mods |= HotkeyModifiers.Shift;
            if (_chkWin.Checked) mods |= HotkeyModifiers.Win;

            var key = _cmbKey.SelectedItem is Keys k ? k : Keys.C;

            var mmods = HotkeyModifiers.None;
            if (_mChkCtrl.Checked) mmods |= HotkeyModifiers.Control;
            if (_mChkAlt.Checked) mmods |= HotkeyModifiers.Alt;
            if (_mChkShift.Checked) mmods |= HotkeyModifiers.Shift;
            if (_mChkWin.Checked) mmods |= HotkeyModifiers.Win;
            var mkey = _mCmbKey.SelectedItem is Keys mk ? mk : Keys.Enter;

            var rmods = HotkeyModifiers.Alt; // Alt обязателен
            if (_rChkCtrl.Checked) rmods |= HotkeyModifiers.Control;
            if (_rChkShift.Checked) rmods |= HotkeyModifiers.Shift;
            if (_rChkWin.Checked) rmods |= HotkeyModifiers.Win;
            var rkey = _rCmbKey.SelectedItem is Keys rk ? rk : Keys.Back;

            CurrentSettings = new AppSettings
            {
                Modifiers = mods,
                Key = key,
                MaximizeModifiers = mmods,
                MaximizeKey = mkey,
                RestoreModifiers = rmods,
                RestoreKey = rkey,
                AutoStart = _chkAutostart.Checked
            };
        }
    }
}


