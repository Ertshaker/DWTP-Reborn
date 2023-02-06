using DWTP_Reborn.Main;
using DWTP_Reborn.Models;
using DWTP_Reborn.Models.WallpaperChanger;
using System.Globalization;
using System;
using System.Threading;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DWTP_Reborn.ViewModels
{
    class WallpaperChangerViewModel : ObservableObject
    {
        private bool _canStop;
        public bool CanStop { get { return _canStop; } set { _canStop = value; OnPropertyChanged(); } }



        public RelayCommand ChangeWallpaper { get; set; }
        public RelayCommand SaveData { get; set; }
        public RelayCommand Stop { get; set; }
        public RelayCommand ChooseNormalFolder { get; set; }
        public RelayCommand ChooseDayFolder { get; set; }
        public RelayCommand ChooseNightFolder { get; set; }
        public RelayCommand NextWallpaper { get; set; }
        public RelayCommand PreviousWallpaper { get; set; }

        public Config CurrentConfig { get; set; }
        private ConfigIO _configIO { get; set; }

        private ChangeWallpaper changeWallpaper;

        public WallpaperChangerViewModel()
        {
            _configIO = new ConfigIO();
            CurrentConfig = _configIO.LoadData();

            ChangeWallpaper = new RelayCommand(o =>
            {
                Start();
                // _timer = new System.Threading.Timer(ChangeCurrentImage, null, 0, CurrentConfig.Frequency * 1000);
            });

            SaveData = new RelayCommand(o =>
            {
                _configIO.SaveData(CurrentConfig);
                System.Windows.MessageBox.Show("Успех");
            });

            ChooseNormalFolder = new RelayCommand(o =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowDialog();
                CurrentConfig.NormalFolderPath = dialog.SelectedPath;
            });

            ChooseDayFolder = new RelayCommand(o =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowDialog();
                CurrentConfig.DayFolderPath = dialog.SelectedPath;
            });

            ChooseNightFolder = new RelayCommand(o =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowDialog();
                CurrentConfig.NightFolderPath = dialog.SelectedPath;
            });

            Stop = new RelayCommand(o =>
            {
                changeWallpaper?.Stop();
                CanStop = false;
            });

            NextWallpaper = new RelayCommand(o =>
            {
                changeWallpaper?.Next();
            });

            PreviousWallpaper = new RelayCommand(o =>
            {
                changeWallpaper?.Previous();
            });
        }

        private void Start()
        {
            _configIO.SaveData(CurrentConfig);
            switch (CurrentConfig.NormalState)
            {
                case true:
                    if (CurrentConfig.Frequency == 0 || CurrentConfig.NormalFolderPath is null or "" || CurrentConfig.ExtensionsList == null)
                    {
                        System.Windows.MessageBox.Show("Не все параметры введены");
                        return;
                    }
                    changeWallpaper = new ChangeWallpaper(
                        frequency: CurrentConfig.Frequency * 1000,
                        normalFolderPath: CurrentConfig.NormalFolderPath,
                        extensionList: CurrentConfig.ExtensionsList.Split()
                        );
                    CanStop = true;
                    break;

                default:
                    if (CurrentConfig.Frequency == 0
                        || CurrentConfig.DayFolderPath is null or "" || CurrentConfig.NightFolderPath is null or ""
                        || CurrentConfig.DayTime is null or "" || CurrentConfig.NightTime is null or ""
                        || CurrentConfig.ExtensionsList == null)
                    {
                        System.Windows.MessageBox.Show("Не все параметры введены");
                        return;
                    }
                    changeWallpaper = new ChangeWallpaper(
                        frequency: CurrentConfig.Frequency * 1000, 
                        dayFolderPath: CurrentConfig.DayFolderPath, 
                        nightFolderPath: CurrentConfig.NightFolderPath, 
                        dayTime: CurrentConfig.DayTime, 
                        nightTime: CurrentConfig.NightTime, 
                        extensionList: CurrentConfig.ExtensionsList.Split()
                        );
                    CanStop = true;
                    break;
            }
            changeWallpaper?.Start();
        }
    }
}

