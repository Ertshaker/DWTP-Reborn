using DWTP_Reborn.Models.WallpaperChanger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace DWTP_Reborn.Models
{
    public class ConfigIO
    {
        private readonly string _path = $"{Environment.CurrentDirectory}\\Config.json";
        public Config LoadData()
        {
            if (!File.Exists(_path))
            {
                File.CreateText(_path);
                return new Config();
            }
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                var json = JsonConvert.DeserializeObject<Config>(fileText);
                if (json != null)
                    return json;
                else
                    return new Config();
            }
        }

        public void SaveData(object ConfigList)
        {
            using (var writer = File.CreateText(_path))
            {
                string output = JsonConvert.SerializeObject(ConfigList);
                writer.Write(output);
            }
        }
    }
}