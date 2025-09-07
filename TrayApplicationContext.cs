using System;
using System.Drawing;
using System.Windows.Forms;

namespace Winnic
{
    internal class TrayApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _menu;
        private readonly ToolStripMenuItem _settingsItem;
        private readonly ToolStripMenuItem _aboutItem;
        private readonly ToolStripMenuItem _exitItem;

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

            _notifyIcon = new NotifyIcon
            {
                Text = "Winnic",
                Icon = IconFactory.CreateAppIcon(),
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
                _hotkeyManager.Register(cfg.Modifiers, cfg.Key, OnHotkeyPressed);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей центрирования: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.MaximizeModifiers, cfg.MaximizeKey, OnMaximizeHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей разворота: {ex.Message}");
            }
            try
            {
                _hotkeyManager.Register(cfg.RestoreModifiers, cfg.RestoreKey, OnRestoreHotkey);
            }
            catch (Exception ex)
            {
                ShowBalloon($"Не удалось зарегистрировать хоткей восстановления: {ex.Message}");
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

        private void OnCenterNow(object? sender, EventArgs e)
        {
            OnHotkeyPressed();
        }

        private void OnMaximizeNow(object? sender, EventArgs e)
        {
            OnMaximizeHotkey();
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

        private void OnRestoreNow(object? sender, EventArgs e)
        {
            OnRestoreHotkey();
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
            _hotkeyManager.Unregister();
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
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
    }
}


