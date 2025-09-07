using System.Text.Json;

namespace Winnic
{
    internal sealed class SettingsService
    {
        private readonly string _path;

        public SettingsService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dir = Path.Combine(appData, "Winnic");
            Directory.CreateDirectory(dir);
            _path = Path.Combine(dir, "settings.json");
        }

        public AppSettings Load()
        {
            try
            {
                if (!File.Exists(_path))
                    return new AppSettings();
                var json = File.ReadAllText(_path);
                var settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                });
                return settings ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public void Save(AppSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_path, json);
        }
    }
}


