using DWTP_Reborn.Main;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DWTP_Reborn.ViewModels
{
    class FileSorterViewModel : ObservableObject
    {
        public RelayCommand StartSortingCommand { get; set; }
        public RelayCommand GetFilesCommand { get; set; }

        private string _folderPath;
        public string FolderPath { get { return _folderPath; } set { _folderPath = value; OnPropertyChanged(); } }

        private Dictionary<string, string> _extensions;
        public Dictionary<string,string> Extensions { get { return _extensions; } set { _extensions = value; OnPropertyChanged(); } }

        private string[] files;
        

        public FileSorterViewModel()
        {
            StartSortingCommand = new RelayCommand(o =>
            {
                Start();
            });
            GetFilesCommand = new RelayCommand(o =>
            {
                GetFiles();
            });
        }

        private void GetFiles()
        {
            var dialog = new FolderBrowserDialog() { AutoUpgradeEnabled = true };
            dialog.ShowDialog();
            Extensions = new Dictionary<string, string>();
            FolderPath = dialog.SelectedPath;

            if (FolderPath is null or "")
                return;

            files = Directory.GetFiles(FolderPath);

            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);
                if (!Extensions.ContainsKey(extension))
                    Extensions.Add(extension, $"{extension} files");
            }
        }
        private void Start()
        {
            foreach (var extension in Extensions)
                Directory.CreateDirectory($"{FolderPath}\\{extension.Value}");

            foreach (string file in Directory.GetFiles(FolderPath))
            {
                string destination = FolderPath + "\\" + Extensions[Path.GetExtension(file)];
                File.Move(file, destination + "\\" + Path.GetFileName(file), true);
            }
        }
    }
}
