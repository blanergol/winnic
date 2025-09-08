namespace Winnic
{
    internal sealed class SettingsForm : Form
    {
        private readonly CheckBox _chkCtrl = new CheckBox { Text = "Ctrl" };
        private readonly CheckBox _chkAlt = new CheckBox { Text = "Alt", Checked = true, Enabled = false };
        private readonly CheckBox _chkShift = new CheckBox { Text = "Shift" };
        private readonly CheckBox _chkWin = new CheckBox { Text = "Win" };
        private readonly ComboBox _cmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

        private readonly ComboBox _mCmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox _lCmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox _rHalfCmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox _tHalfCmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox _bHalfCmbKey = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

        private readonly CheckBox _chkAutostart = new CheckBox { Text = "Автозапуск при старте Windows", AutoSize = true };
        private readonly Button _btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12, 6, 12, 6) };
        private readonly Button _btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(12, 6, 12, 6) };

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
            ClientSize = new Size(540, 540);
            MinimumSize = new Size(540, 540);

            var modsPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            var modsLabel = new Label { Text = "Общие модификаторы:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            modsPanel.Controls.Add(modsLabel);
            modsPanel.SetFlowBreak(modsLabel, true);
            modsPanel.Controls.AddRange(new Control[] { _chkCtrl, _chkAlt, _chkShift, _chkWin });

            var keysTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10),
                ColumnCount = 2
            };
            keysTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            keysTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));

            int rowIndex = 0;

            var lblCenter = new Label { Text = "Центрирование:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            keysTable.Controls.Add(lblCenter, 0, rowIndex);
            _cmbKey.Width = 200;
            _cmbKey.Anchor = AnchorStyles.Left;
            keysTable.Controls.Add(_cmbKey, 1, rowIndex++);

            var lblMax = new Label { Text = "На весь экран:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            keysTable.Controls.Add(lblMax, 0, rowIndex);
            _mCmbKey.Width = 200;
            _mCmbKey.Anchor = AnchorStyles.Left;
            keysTable.Controls.Add(_mCmbKey, 1, rowIndex++);

            var lblLeft = new Label { Text = "Левая половина:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            keysTable.Controls.Add(lblLeft, 0, rowIndex);
            _lCmbKey.Width = 200;
            _lCmbKey.Anchor = AnchorStyles.Left;
            keysTable.Controls.Add(_lCmbKey, 1, rowIndex++);

            var lblRightHalf = new Label { Text = "Правая половина:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            keysTable.Controls.Add(lblRightHalf, 0, rowIndex);
            _rHalfCmbKey.Width = 200;
            _rHalfCmbKey.Anchor = AnchorStyles.Left;
            keysTable.Controls.Add(_rHalfCmbKey, 1, rowIndex++);

            var lblTopHalf = new Label { Text = "Верхняя половина:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            keysTable.Controls.Add(lblTopHalf, 0, rowIndex);
            _tHalfCmbKey.Width = 200;
            _tHalfCmbKey.Anchor = AnchorStyles.Left;
            keysTable.Controls.Add(_tHalfCmbKey, 1, rowIndex++);

            var lblBottomHalf = new Label { Text = "Нижняя половина:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            keysTable.Controls.Add(lblBottomHalf, 0, rowIndex);
            _bHalfCmbKey.Width = 200;
            _bHalfCmbKey.Anchor = AnchorStyles.Left;
            keysTable.Controls.Add(_bHalfCmbKey, 1, rowIndex++);

            var lblRestore = new Label { Text = "Восстановление:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 6, 6, 0) };
            keysTable.Controls.Add(lblRestore, 0, rowIndex);
            _rCmbKey.Width = 200;
            _rCmbKey.Anchor = AnchorStyles.Left;
            keysTable.Controls.Add(_rCmbKey, 1, rowIndex++);

            var autostartPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10, 10, 10, 12) };
            autostartPanel.Controls.Add(_chkAutostart);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(10) };
            buttons.Controls.AddRange(new Control[] { _btnOk, _btnCancel });

            var content = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            // Порядок добавления важен для DockStyle.Top: последний добавленный будет сверху
            content.Controls.Add(autostartPanel);
            content.Controls.Add(keysTable);
            content.Controls.Add(modsPanel);

            Controls.Add(content);
            Controls.Add(buttons);

            Load += OnLoad;
            _btnOk.Click += OnOk;
            AcceptButton = _btnOk;
            CancelButton = _btnCancel;

            _cmbKey.FormattingEnabled = true;
            _mCmbKey.FormattingEnabled = true;
            _lCmbKey.FormattingEnabled = true;
            _rHalfCmbKey.FormattingEnabled = true;
            _tHalfCmbKey.FormattingEnabled = true;
            _bHalfCmbKey.FormattingEnabled = true;
            _rCmbKey.FormattingEnabled = true;
            _cmbKey.Format += FormatKeyName;
            _mCmbKey.Format += FormatKeyName;
            _lCmbKey.Format += FormatKeyName;
            _rHalfCmbKey.Format += FormatKeyName;
            _tHalfCmbKey.Format += FormatKeyName;
            _bHalfCmbKey.Format += FormatKeyName;
            _rCmbKey.Format += FormatKeyName;
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
            // Заполняем списки доступных основных клавиш
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                if ((int)k >= (int)Keys.A && (int)k <= (int)Keys.Z || (int)k >= (int)Keys.F1 && (int)k <= (int)Keys.F24)
                {
                    _cmbKey.Items.Add(k);
                    _mCmbKey.Items.Add(k);
                    _lCmbKey.Items.Add(k);
                    _rHalfCmbKey.Items.Add(k);
                    _tHalfCmbKey.Items.Add(k);
                    _bHalfCmbKey.Items.Add(k);
                    _rCmbKey.Items.Add(k);
                }
            }
            // Добавим популярные дополнительные клавиши
            if (!_cmbKey.Items.Contains(Keys.Enter)) { _cmbKey.Items.Add(Keys.Enter); }
            if (!_mCmbKey.Items.Contains(Keys.Enter)) { _mCmbKey.Items.Add(Keys.Enter); }
            if (!_lCmbKey.Items.Contains(Keys.Left)) { _lCmbKey.Items.Add(Keys.Left); }
            if (!_rHalfCmbKey.Items.Contains(Keys.Right)) { _rHalfCmbKey.Items.Add(Keys.Right); }
            if (!_tHalfCmbKey.Items.Contains(Keys.Up)) { _tHalfCmbKey.Items.Add(Keys.Up); }
            if (!_bHalfCmbKey.Items.Contains(Keys.Down)) { _bHalfCmbKey.Items.Add(Keys.Down); }
            if (!_rCmbKey.Items.Contains(Keys.Back)) { _rCmbKey.Items.Add(Keys.Back); }

            _chkCtrl.Checked = CurrentSettings.CommonModifiers.HasFlag(HotkeyModifiers.Control);
            // Alt обязателен: принудительно включаем и делаем недоступным для снятия
            _chkAlt.Checked = true;
            _chkShift.Checked = CurrentSettings.CommonModifiers.HasFlag(HotkeyModifiers.Shift);
            _chkWin.Checked = CurrentSettings.CommonModifiers.HasFlag(HotkeyModifiers.Win);
            if (!_cmbKey.Items.Contains(CurrentSettings.Key)) _cmbKey.Items.Add(CurrentSettings.Key);
            _cmbKey.SelectedItem = CurrentSettings.Key;
            if (_cmbKey.SelectedIndex < 0 && _cmbKey.Items.Count > 0)
                _cmbKey.SelectedIndex = 0;

            if (!_mCmbKey.Items.Contains(CurrentSettings.MaximizeKey)) _mCmbKey.Items.Add(CurrentSettings.MaximizeKey);
            _mCmbKey.SelectedItem = CurrentSettings.MaximizeKey;
            if (_mCmbKey.SelectedIndex < 0 && _mCmbKey.Items.Count > 0)
                _mCmbKey.SelectedIndex = 0;

            if (!_lCmbKey.Items.Contains(CurrentSettings.LeftKey)) _lCmbKey.Items.Add(CurrentSettings.LeftKey);
            _lCmbKey.SelectedItem = CurrentSettings.LeftKey;
            if (_lCmbKey.SelectedIndex < 0 && _lCmbKey.Items.Count > 0)
                _lCmbKey.SelectedIndex = 0;

            if (!_rHalfCmbKey.Items.Contains(CurrentSettings.RightKey)) _rHalfCmbKey.Items.Add(CurrentSettings.RightKey);
            _rHalfCmbKey.SelectedItem = CurrentSettings.RightKey;
            if (_rHalfCmbKey.SelectedIndex < 0 && _rHalfCmbKey.Items.Count > 0)
                _rHalfCmbKey.SelectedIndex = 0;

            if (!_tHalfCmbKey.Items.Contains(CurrentSettings.TopKey)) _tHalfCmbKey.Items.Add(CurrentSettings.TopKey);
            _tHalfCmbKey.SelectedItem = CurrentSettings.TopKey;
            if (_tHalfCmbKey.SelectedIndex < 0 && _tHalfCmbKey.Items.Count > 0)
                _tHalfCmbKey.SelectedIndex = 0;

            if (!_bHalfCmbKey.Items.Contains(CurrentSettings.BottomKey)) _bHalfCmbKey.Items.Add(CurrentSettings.BottomKey);
            _bHalfCmbKey.SelectedItem = CurrentSettings.BottomKey;
            if (_bHalfCmbKey.SelectedIndex < 0 && _bHalfCmbKey.Items.Count > 0)
                _bHalfCmbKey.SelectedIndex = 0;

            _chkAutostart.Checked = CurrentSettings.AutoStart;

            if (!_rCmbKey.Items.Contains(CurrentSettings.RestoreKey)) _rCmbKey.Items.Add(CurrentSettings.RestoreKey);
            _rCmbKey.SelectedItem = CurrentSettings.RestoreKey;
            if (_rCmbKey.SelectedIndex < 0 && _rCmbKey.Items.Count > 0)
                _rCmbKey.SelectedIndex = 0;
        }

        private void OnOk(object? sender, EventArgs e)
        {
            var commonMods = HotkeyModifiers.None;
            if (_chkCtrl.Checked) commonMods |= HotkeyModifiers.Control;
            // Alt обязателен
            commonMods |= HotkeyModifiers.Alt;
            if (_chkShift.Checked) commonMods |= HotkeyModifiers.Shift;
            if (_chkWin.Checked) commonMods |= HotkeyModifiers.Win;

            var key = _cmbKey.SelectedItem is Keys k ? k : Keys.C;
            var mkey = _mCmbKey.SelectedItem is Keys mk ? mk : Keys.Enter;
            var leftKey = _lCmbKey.SelectedItem is Keys lk ? lk : Keys.Left;
            var rightKey = _rHalfCmbKey.SelectedItem is Keys rk2 ? rk2 : Keys.Right;
            var topKey = _tHalfCmbKey.SelectedItem is Keys tk ? tk : Keys.Up;
            var bottomKey = _bHalfCmbKey.SelectedItem is Keys bk ? bk : Keys.Down;
            var rkey = _rCmbKey.SelectedItem is Keys rk ? rk : Keys.Back;

            CurrentSettings = new AppSettings
            {
                CommonModifiers = commonMods,
                Key = key,
                MaximizeKey = mkey,
                LeftKey = leftKey,
                RightKey = rightKey,
                TopKey = topKey,
                BottomKey = bottomKey,
                RestoreKey = rkey,
                AutoStart = _chkAutostart.Checked
            };
        }
    }
}


