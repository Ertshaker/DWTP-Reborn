using DWTP_Reborn.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWTP_Reborn.ViewModels
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand WallpaperChangerViewCommand { get; set; }

        public RelayCommand WallpaperChangerChangeWallpaper { get; set; }

        public WallpaperChangerViewModel WallpaperChangerVM { get; set; }

        private object? _currentView;

        public object? CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            WallpaperChangerVM = new WallpaperChangerViewModel();
            CurrentView = null;

            WallpaperChangerViewCommand = new RelayCommand(o => 
            {
                CurrentView = WallpaperChangerVM;
            });

            WallpaperChangerChangeWallpaper = new RelayCommand(o => 
            {
                WallpaperChangerVM.Start();
            });
        }
    }
}
