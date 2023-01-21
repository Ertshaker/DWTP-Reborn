using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Linq;

namespace DWTP_Reborn.Models.WallpaperChanger
{
    public class ChangeWallpaper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo
            (
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

        private Mode _mode;
        private enum Mode
        {
            DayNight = 1,
            Normal = 2
        }
        private int _frequency;
        private int _dayTime;
        private int _nightTime;
        private string _dayFolderPath;
        private string _nightFolderPath;
        private string _normalFolderPath;
        private string[] _extensionList;
        TimeState timeState, previousTimeState;
        enum TimeState { Day, Night }
        int i = 0;

        public ChangeWallpaper(int frequency, string dayFolderPath, string nightFolderPath, string dayTime, string nightTime, string[] extensionList)
        {
            _frequency = frequency;
            _dayTime = Convert.ToInt32(dayTime.Replace(":", ""));
            _nightTime = Convert.ToInt32(nightTime.Replace(":", ""));
            _dayFolderPath = dayFolderPath;
            _nightFolderPath = nightFolderPath;
            _extensionList = extensionList;
            _mode = Mode.DayNight;
        }

        public ChangeWallpaper(int frequency, string normalFolderPath, string[] extensionList)
        {
            _frequency = frequency;
            _normalFolderPath = normalFolderPath;
            _extensionList = extensionList;
            _mode = Mode.Normal;
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
            if (_mode is Mode.Normal)
            {
                foreach (string file in Directory.GetFiles(_normalFolderPath))
                    if (_extensionList.Any(x => x == Path.GetExtension(file)))
                        list.Add(file);
            }

            else if (timeState == TimeState.Day)
            {
                foreach (string file in Directory.GetFiles(_dayFolderPath))
                    if (_extensionList.Any(x => x == Path.GetExtension(file)))
                        list.Add(file);
            }

            else if (timeState == TimeState.Night)
            {
                foreach (string file in Directory.GetFiles(_nightFolderPath))
                    if (_extensionList.Any(x => x == Path.GetExtension(file)))
                        list.Add(file);
            }
        }
        void Change(object? stateinfo)
        {

            if (_mode == Mode.DayNight)
                checkTime();

            if (previousTimeState != timeState || list.Count == 0)
                getFiles();

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, list[i++], SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            if (i == list.Count)
                i = 0;

            previousTimeState = timeState;
        }

        public void Stop() => timer?.Dispose();
    }
}
