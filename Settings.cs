using System.Text.Json.Serialization;

namespace Winnic
{
    internal sealed class AppSettings
    {
        // Общие модификаторы для всех хоткеев
        public HotkeyModifiers CommonModifiers { get; set; } = HotkeyModifiers.Control | HotkeyModifiers.Alt;

        // Основные клавиши по командам
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys Key { get; set; } = Keys.C; // Центрирование — клавиша
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys MaximizeKey { get; set; } = Keys.Enter; // Разворот — клавиша
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys RestoreKey { get; set; } = Keys.Back; // Восстановление — клавиша

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys LeftKey { get; set; } = Keys.Left; // Левая половина — клавиша
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys RightKey { get; set; } = Keys.Right; // Правая половина — клавиша

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys TopKey { get; set; } = Keys.Up; // Верхняя половина — клавиша
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys BottomKey { get; set; } = Keys.Down; // Нижняя половина — клавиша

        // Автозапуск
        public bool AutoStart { get; set; } = false;
    }
}


