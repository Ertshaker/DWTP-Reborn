using DWTP_Reborn.Main;
using DWTP_Reborn.Models;
using DWTP_Reborn.Models.WallpaperChanger;
using System;
using System.IO;
using System.Windows;

namespace DWTP_Reborn.ViewModels
{
    class WallpaperChangerViewModel : ObservableObject
    {
        private readonly string _path = Environment.CurrentDirectory + "Config.json";

        public RelayCommand ChangeWallpaper { get; set; }
        public RelayCommand SaveData { get; set; }

        public Config CurrentConfig { get; set; }
        private ConfigIO _configIO { get; set; }

        private ChangeWallpaper changeWallpaper;

        public WallpaperChangerViewModel()
        {
            _configIO = new ConfigIO(_path);
            CurrentConfig = _configIO.LoadData();

            ChangeWallpaper = new RelayCommand(o =>
            {
                Start();
            });

            SaveData = new RelayCommand(o =>
            {
                _configIO.SaveData(CurrentConfig);
                MessageBox.Show("Успех");
            });
        }

        public void Start()
        {
            _configIO.SaveData(CurrentConfig);
            switch (CurrentConfig.NormalState)
            {
                case true:
                    changeWallpaper = new ChangeWallpaper(
                        frequency: CurrentConfig.Frequency, 
                        normalFolderPath: CurrentConfig.NormalFolderPath,
                        extensionList: CurrentConfig.ExtensionsList.Split()
                        );
                    break;
                default:
                    changeWallpaper = new ChangeWallpaper(
                        frequency: CurrentConfig.Frequency, 
                        dayFolderPath: CurrentConfig.DayFolderPath, 
                        nightFolderPath: CurrentConfig.NightFolderPath, 
                        dayTime: CurrentConfig.DayTime, 
                        nightTime: CurrentConfig.NightTime, 
                        extensionList: CurrentConfig.ExtensionsList.Split()
                        );
                    break;
            }
            changeWallpaper?.Start();
        }
    }
}
