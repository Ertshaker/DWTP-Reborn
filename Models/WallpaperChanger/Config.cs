using DWTP_Reborn.Main;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DWTP_Reborn.Models.WallpaperChanger
{
    public class Config : INotifyPropertyChanged
    {
        private int _frequency;
        private string _dayTime;
        private string _nightTime;
        private string _dayFolderPath;
        private string _nightFolderPath;
        private string _normalFolderPath;
        private string _extensionsList;
        private bool _normalState;


        public int Frequency { get { return _frequency; } set { _frequency = value; OnPropertyChanged(); } }
        public string DayTime { get { return _dayTime; } set { _dayTime = value; OnPropertyChanged(); } }
        public string NightTime { get { return _nightTime; } set { _nightTime = value; OnPropertyChanged(); } }
        public string DayFolderPath { get { return _dayFolderPath; } set { _dayFolderPath = value; OnPropertyChanged(); } }
        public string NightFolderPath { get { return _nightFolderPath; } set { _nightFolderPath = value; OnPropertyChanged(); } }
        public string NormalFolderPath { get { return _normalFolderPath; } set { _normalFolderPath= value; OnPropertyChanged(); } }
        public string ExtensionsList { get { return _extensionsList; } set { _extensionsList = value; OnPropertyChanged(); } }
        public bool NormalState { get { return _normalState; } set { _normalState = value; OnPropertyChanged(); } }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
