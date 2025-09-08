using System.Text.Json.Serialization;

namespace Winnic
{
    internal sealed class AppSettings
    {
        // Common modifiers for all hotkeys
        public HotkeyModifiers CommonModifiers { get; set; } = HotkeyModifiers.Control | HotkeyModifiers.Alt;

        // Primary keys per command
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys Key { get; set; } = Keys.C; // Center — key
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys MaximizeKey { get; set; } = Keys.Enter; // Maximize — key
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys RestoreKey { get; set; } = Keys.Back; // Restore — key

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys LeftKey { get; set; } = Keys.Left; // Left half — key
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys RightKey { get; set; } = Keys.Right; // Right half — key

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys TopKey { get; set; } = Keys.Up; // Top half — key
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys BottomKey { get; set; } = Keys.Down; // Bottom half — key

        // Autostart
        public bool AutoStart { get; set; } = false;

        // Additional window commands
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys MinimizeKey { get; set; } = Keys.W; // Minimize active window — key (Ctrl+Alt+W)
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys RestoreMinimizedKey { get; set; } = Keys.E; // Restore last minimized window — key (Ctrl+Alt+E)
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys CloseKey { get; set; } = Keys.Q; // Close active window — key (Ctrl+Alt+Q)
    }
}


