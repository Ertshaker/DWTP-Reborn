using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DWTP_Reborn.Models.FileSorter
{
    public class File : INotifyPropertyChanged
    {
        private string _extension;

        public string Extension
        {
            get { return _extension; }
            set { _extension = value; OnPropertyChanged(); }
        }

        private string _folderName;

        public string FolderName
        {
            get { return _folderName; }
            set { _folderName = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
