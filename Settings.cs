using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace Winnic
{
    internal sealed class AppSettings
    {
        // Хоткей центрирования
        public HotkeyModifiers Modifiers { get; set; } = HotkeyModifiers.Control | HotkeyModifiers.Alt;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys Key { get; set; } = Keys.C; // Ctrl+Alt+C по умолчанию

        // Хоткей разворота
        public HotkeyModifiers MaximizeModifiers { get; set; } = HotkeyModifiers.Control | HotkeyModifiers.Alt;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys MaximizeKey { get; set; } = Keys.Enter; // Ctrl+Alt+Enter по умолчанию

        // Хоткей восстановления предыдущего состояния
        public HotkeyModifiers RestoreModifiers { get; set; } = HotkeyModifiers.Control | HotkeyModifiers.Alt;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys RestoreKey { get; set; } = Keys.Back; // Ctrl+Alt+Backspace по умолчанию

        // Автозапуск
        public bool AutoStart { get; set; } = false;
    }
}


