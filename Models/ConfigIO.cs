using DWTP_Reborn.Models.WallpaperChanger;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace DWTP_Reborn.Models
{
    public class ConfigIO
    {
        private string PATH;
        public ConfigIO(string path)
        {
            PATH = path;
        }
        public Config LoadData()
        {
            if (!File.Exists(PATH))
            {
                File.CreateText(PATH);
                return new Config();
            }
            using (var reader = File.OpenText(PATH))
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
            using (var writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(ConfigList);
                writer.Write(output);
            }
        }
    }
}