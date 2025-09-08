namespace Winnic
{
    internal class TrayApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _menu;
        private readonly ToolStripMenuItem _settingsItem;
        private readonly ToolStripMenuItem _aboutItem;
        private readonly ToolStripMenuItem _exitItem;
        private readonly Icon _icon;

        private readonly WindowCenterService _windowCenterService;
        private readonly HotkeyManager _hotkeyManager;
        private readonly SettingsService _settingsService;
        private readonly AutoStartService _autoStartService;

        public TrayApplicationContext()
        {
            _settingsService = new SettingsService();
            _windowCenterService = new WindowCenterService();
            _hotkeyManager = new HotkeyManager();
            _autoStartService = new AutoStartService();

            _menu = new ContextMenuStrip();
            _settingsItem = new ToolStripMenuItem("Настройки…", null, OnOpenSettings);
            _aboutItem = new ToolStripMenuItem("О программе…", null, OnOpenAbout);
            _exitItem = new ToolStripMenuItem("Выход", null, OnExit);
            _menu.Items.AddRange(new ToolStripItem[] { _settingsItem, new ToolStripSeparator(), _aboutItem, new ToolStripSeparator(), _exitItem });

            _icon = IconFactory.CreateAppIcon();
            _notifyIcon = new NotifyIcon
            {
                Text = "Winnic",
                Icon = _icon,
                Visible = true,
                ContextMenuStrip = _menu
            };

            RegisterHotkeys();

            // Применить автозапуск по настройкам при старте
            var cfg = _settingsService.Load();
            _autoStartService.Apply(cfg.AutoStart);
        }

        private void RegisterHotkeys()
        {
            var cfg = _settingsService.Load();
            _hotkeyManager.Unregister();
            try
            {
                _hotkeyManager.Register(cfg.CommonModifiers, cfg.Key, OnHotkeyPressed);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей центрирования: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.CommonModifiers, cfg.MaximizeKey, OnMaximizeHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей разворота: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.CommonModifiers, cfg.RestoreKey, OnRestoreHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей восстановления: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.CommonModifiers, cfg.LeftKey, OnSnapLeftHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей левой половины: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.CommonModifiers, cfg.RightKey, OnSnapRightHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей правой половины: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.CommonModifiers, cfg.TopKey, OnSnapTopHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей верхней половины: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.CommonModifiers, cfg.BottomKey, OnSnapBottomHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей нижней половины: {ex.Message}");
            }
        }

        private void OnHotkeyPressed()
        {
            try
            {
                _windowCenterService.CenterForegroundWindow();
            }
            catch (Exception ex)
            {
                ShowBalloon($"Ошибка: {ex.Message}");
            }
        }

        
        private void OnMaximizeHotkey()
        {
            try
            {
                _windowCenterService.SaveForegroundPlacement();
                _windowCenterService.MaximizeForegroundWindow();
            }
            catch (Exception ex)
            {
                ShowBalloon($"Ошибка: {ex.Message}");
            }
        }

        private void OnRestoreHotkey()
        {
            try
            {
                _windowCenterService.RestoreLastPlacement();
            }
            catch (Exception ex)
            {
                ShowBalloon($"Ошибка: {ex.Message}");
            }
        }

        private void OnSnapLeftHotkey()
        {
            try
            {
                _windowCenterService.SnapForegroundWindowLeftHalf();
            }
            catch (Exception ex)
            {
                ShowBalloon($"Ошибка: {ex.Message}");
            }
        }

        private void OnSnapRightHotkey()
        {
            try
            {
                _windowCenterService.SnapForegroundWindowRightHalf();
            }
            catch (Exception ex)
            {
                ShowBalloon($"Ошибка: {ex.Message}");
            }
        }

        private void OnSnapTopHotkey()
        {
            try
            {
                _windowCenterService.SnapForegroundWindowTopHalf();
            }
            catch (Exception ex)
            {
                ShowBalloon($"Ошибка: {ex.Message}");
            }
        }

        private void OnSnapBottomHotkey()
        {
            try
            {
                _windowCenterService.SnapForegroundWindowBottomHalf();
            }
            catch (Exception ex)
            {
                ShowBalloon($"Ошибка: {ex.Message}");
            }
        }

        private void OnOpenSettings(object? sender, EventArgs e)
        {
            using var form = new SettingsForm(_settingsService.Load());
            if (form.ShowDialog() == DialogResult.OK)
            {
                _settingsService.Save(form.CurrentSettings);
                RegisterHotkeys();
                ShowBalloon("Горячие клавиши обновлены");
                _autoStartService.Apply(form.CurrentSettings.AutoStart);
            }
        }

        private void OnExit(object? sender, EventArgs e)
        {
            ExitThread();
        }

        private void OnOpenAbout(object? sender, EventArgs e)
        {
            using var form = new AboutForm();
            form.ShowDialog();
        }

        private void ShowBalloon(string text)
        {
            _notifyIcon.BalloonTipTitle = "Winnic";
            _notifyIcon.BalloonTipText = text;
            _notifyIcon.ShowBalloonTip(2000);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { _hotkeyManager.Unregister(); } catch { }
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _menu.Dispose();
                _hotkeyManager.Dispose();
                _icon.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


