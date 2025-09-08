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
    }
}


