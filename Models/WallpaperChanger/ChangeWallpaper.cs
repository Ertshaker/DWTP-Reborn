using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DWTP_Reborn.Models.WallpaperChanger
{
    public class ChangeWallpaper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(
            int uAction,
            int uParam,
            string lpvParam,
            int fuWinIni
            );

        const int SPI_SETDESKWALLPAPER = 0x14;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        List<string> list = new List<string>();
        Timer timer;

        private int _frequency;
        private int _dayTime;
        private int _nightTime;
        private string _dayFolderPath;
        private string _nightFolderPath;
        private string _normalFolderPath;
        TimeState timeState, previousTimeState;
        enum TimeState { Day, Night }
        int i = 0;

        public ChangeWallpaper(int frequency, string dayFolderPath, string nightFolderPath, string dayTime, string nightTime)
        {
            _frequency = frequency;
            _dayTime = Convert.ToInt32(dayTime.Replace(":", ""));
            _nightTime = Convert.ToInt32(nightTime.Replace(":", ""));
            _dayFolderPath = dayFolderPath;
            _nightFolderPath = nightFolderPath;
        }

        public ChangeWallpaper(int frequency, string normalFolderPath)
        {
            _frequency = frequency;
            _normalFolderPath = normalFolderPath;
        }

        public void Start() => timer = new Timer(Change, null, 0, _frequency);

        void checkTime()
        {
            int currentTime = Convert.ToInt32(DateTime.Now.ToString("H:mm").Replace(":", ""));

            if (currentTime > _nightTime || currentTime < _dayTime)
                timeState = TimeState.Night;

            else if (currentTime > _dayTime && currentTime < _nightTime)
                timeState = TimeState.Day;
        }

        void getFiles()
        {
            if (Test.ConfigList[0].State is Config.States.Normal)
            {
                foreach (string file in Directory.GetFiles(_normalFolderPath))
                    if (Test.ConfigList[0].ExtensionsList.Any(x => x == Path.GetExtension(file)))
                        list.Add(file);
            }

            else if (timeState == TimeState.Day)
            {
                foreach (string file in Directory.GetFiles(_dayFolderPath))
                    if (Test.ConfigList[0].ExtensionsList.Any(x => x == Path.GetExtension(file)))
                        list.Add(file);
            }

            else if (timeState == TimeState.Night)
            {
                foreach (string file in Directory.GetFiles(_nightFolderPath))
                    if (Test.ConfigList[0].ExtensionsList.Any(x => x == Path.GetExtension(file)))
                        list.Add(file);
            }
        }

        void Change(object? stateinfo)
        {

            if (Test.ConfigList[0].State == Config.States.DayNight)
                checkTime();

            if (previousTimeState != timeState || list.Count == 0)
                getFiles();

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, list[i++], SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            ;
            if (i == list.Count)
                i = 0;

            previousTimeState = timeState;
        }

        public void Stop() => timer?.Dispose();
    }
}
