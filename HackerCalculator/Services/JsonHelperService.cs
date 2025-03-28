using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using HackerCalculator.Model;

namespace HackerCalculator.Services
{
    public class JsonHelperService
    {
        private static readonly String filePath = "settings.json";

        public static void SaveSettings(AppSettings settings)
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static AppSettings LoadSettings()
        {
            if (!File.Exists(filePath))
                return new AppSettings();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<AppSettings>(json);
        }
    }
}
