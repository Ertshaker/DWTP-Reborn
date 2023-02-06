using DWTP_Reborn.Main;
using System.Windows;

namespace DWTP_Reborn.ViewModels
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand WallpaperChangerViewCommand { get; set; }
        public RelayCommand FileSorterViewCommand { get; set; }
        public RelayCommand ExitApplicationCommand { get; set; }
        public RelayCommand CollapseApplicationCommand { get; set; }

        public WallpaperChangerViewModel WallpaperChangerVM { get; set; }
        public FileSorterViewModel FileSorterVM { get; set; }

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
            FileSorterVM = new FileSorterViewModel();

            CurrentView = null;

            WallpaperChangerViewCommand = new RelayCommand(o => 
            {
                CurrentView = WallpaperChangerVM;
            });

            FileSorterViewCommand = new RelayCommand(o =>
            {
                CurrentView = FileSorterVM;
            });

            ExitApplicationCommand = new RelayCommand(o =>
            {
                Application.Current.Shutdown();
            });

            CollapseApplicationCommand = new RelayCommand(o =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }
    }
}
