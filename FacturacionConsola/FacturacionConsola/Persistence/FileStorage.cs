using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FacturacionConsola.Persistence
{
    public static class FileStorage
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true
        };

        public static List<T> LoadList<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    // Crear archivo vacío con []
                    SaveList(path, new List<T>());
                    return new List<T>();
                }
                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json)) return new List<T>();
                var list = JsonSerializer.Deserialize<List<T>>(json, Options);
                return list ?? new List<T>();
            }
            catch
            {
                // Si el archivo está corrupto, no matar la app
                return new List<T>();
            }
        }

        public static void SaveList<T>(string path, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, Options);
            File.WriteAllText(path, json);
        }

        public static T LoadObject<T>(string path, T defaultValue)
        {
            try
            {
                if (!File.Exists(path))
                {
                    SaveObject(path, defaultValue);
                    return defaultValue;
                }
                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json)) return defaultValue;
                var obj = JsonSerializer.Deserialize<T>(json, Options);
                return obj == null ? defaultValue : obj;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void SaveObject<T>(string path, T obj)
        {
            var json = JsonSerializer.Serialize(obj, Options);
            File.WriteAllText(path, json);
        }
    }

}
