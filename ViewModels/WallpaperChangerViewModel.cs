using DWTP_Reborn.Main;
using DWTP_Reborn.Models.WallpaperChanger;

namespace DWTP_Reborn.ViewModels
{
    class WallpaperChangerViewModel : ObservableObject
    {
        public void Start()
        {
            var changeWallpaper = new ChangeWallpaper(1000, "");
            changeWallpaper.Start();
        }
    }
}
