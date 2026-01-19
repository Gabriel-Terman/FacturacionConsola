using System;
using System.Collections.Generic;
using System.Text;
using FacturacionConsola.Persistence;

namespace FacturacionConsola.Config
{
    public class ConfigRepository
    {
        private readonly string _path;
        public ConfigRepository(string path)
        {
            _path = path;
        }

        public Config Load()
        {
            return FileStorage.LoadObject(_path, new Config());
        }

        public void Save(Config config)
        {
            FileStorage.SaveObject(_path, config);
        }
    }
}
