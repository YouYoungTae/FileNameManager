using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace Yutil.YFile
{
    
    
    public class YFileNameManager :INotifyPropertyChanged 
    {
        public class ExntestionChk :INotifyPropertyChanged
        {
            private string extenstion;
            private bool chk;

            public string Extenstion {
                get => extenstion;
                set
                {
                    extenstion = value;
                    OnPropertyChanged();

                }
            }
            public bool Chk {
                get => chk;
                set
                {
                    chk = value;
                    OnPropertyChanged();

                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string nm = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nm));
            }
        }    
        List<FileInfo> _files;

        ObservableCollection<ExntestionChk> _Extentions;
        
        

        string _userPath;
        public string UserPath { get => _userPath;
            set
            {
                _userPath = value;
                OnPropertyChanged();
            }
        }

        string _ReName;
        Dictionary<string, List<FileInfo>> _extentionFiles;
        public List<FileInfo> Files { get => _files;

            set
            {
                _files = value;
                OnPropertyChanged();
            }
        }

        internal void FolrderNameChange()
        {
            string path = DirectoryInfo.Parent.FullName;
            string r = $"{path}\\{ReName}";
            DirectoryInfo.MoveTo($"{path}\\{ReName}");

            UserPath = r;
            OpenDirectory(UserPath);
        }

        public DirectoryInfo DirectoryInfo { get => _directoryInfo; set => _directoryInfo = value; }


        public ObservableCollection<ExntestionChk> Extentions { get => _Extentions;
            set
            {
                _Extentions = value;
                OnPropertyChanged();
            }
        }
        public Dictionary<string, List<FileInfo>> ExtentionFiles { get => _extentionFiles; set => _extentionFiles = value; }

        internal void ReNameMake()
        {
            int num = 0;
            if (FirstExtentionFiles == null)
            {
                return;
            }
            else if (SecondExtentionFiles == null)
            {

            }
            else
            {
                int cnt = FirstExtentionFiles.Count;
                int cnt2 = SecondExtentionFiles.Count;
                if (cnt != cnt2) return;
                var temp1 = FirstExtentionFiles;
                var temp2 = SecondExtentionFiles;
                var extenstrs = Extentions.Where(s => s.Chk == true).Select(s => s.Extenstion).ToArray();
         
                if (cnt < 100)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        num += 1;
                        temp1[i] = Tuple.Create<string, FileInfo>($"{ReName}_{num.ToString("00")}{extenstrs[0]}", temp1[i].Item2);
                        temp2[i] = Tuple.Create<string, FileInfo>($"{ReName}_{num.ToString("00")}{extenstrs[1]}", temp2[i].Item2);
                    }
                }
                else if (cnt < 1000)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        num += 1;
                        temp1[i] = Tuple.Create<string, FileInfo>($"{ReName}_{num.ToString("000")}{extenstrs[0]}", temp1[i].Item2);
                        temp2[i] = Tuple.Create<string, FileInfo>($"{ReName}_{num.ToString("000")}{extenstrs[1]}", temp2[i].Item2);
                    }
                }

                FirstExtentionFiles = null;
                FirstExtentionFiles = temp1;
                SecondExtentionFiles = null;
                SecondExtentionFiles = temp2;
            }



        }

        public void FileNmChange()
        {
            
            if (FirstExtentionFiles != null)
            {
                int cnt = FirstExtentionFiles.Count;
               
                for (int i = 0; i < cnt; i++)
                {
                    var nname= FirstExtentionFiles[i].Item2.DirectoryName + "\\" + FirstExtentionFiles[i].Item1;
                    FirstExtentionFiles[i].Item2.MoveTo(nname);
                }
            }
            if (SecondExtentionFiles != null)
            {
                int cnt = SecondExtentionFiles.Count;

                for (int i = 0; i < cnt; i++)
                {
                    var nname = SecondExtentionFiles[i].Item2.DirectoryName + "\\" + SecondExtentionFiles[i].Item1;
                    SecondExtentionFiles[i].Item2.MoveTo(nname);
                }
            }
        }
        private bool isfolderNMChangeMode = true;
        public bool IsfolderNMChangeMode { get => isfolderNMChangeMode;
            set
            {
                isfolderNMChangeMode = value;
                OnPropertyChanged(); ;
            }

        }
        public string ReName { get => _ReName;
            set
            {
                _ReName = value;
                OnPropertyChanged();
            }
        }

        DirectoryInfo _directoryInfo;

        public void OnPropertyChanged([CallerMemberName] string nm ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nm));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public string OpenFolderPath()
        {
            return Win32Files.OpenFolder();
        }
        private void MakeExtention()
        {
            var loopExtentions= Files.Select(s => s.Extension.ToLower()).Distinct();
            Extentions = new ObservableCollection<ExntestionChk>();
            foreach (var item in loopExtentions)
            {
                Extentions.Add(new ExntestionChk()
                {
                    Chk = true,
                    Extenstion = item
                });
            }
            MakeExtentionFiles("");
        }

        public bool OpenDirectory(string path)
        {
            try
            {
                path = path + "\\";
                DirectoryInfo = new DirectoryInfo(path);
                
                Files = new List<FileInfo>(DirectoryInfo.EnumerateFiles());
                ExtentionFilesMaxCount = Files.Count;
                MakeExtention();
                
                if (_directoryInfo.Exists)
                {
                    return true;
                }
                else
                {
                    _directoryInfo = null;
                    return false;
                }
            }
            catch (Exception)
            {
                _directoryInfo = null;
                return false;
            }
        }

        public void MakeExtentionFiles(string 지정명 = "")
        {
            if (Extentions == null || Extentions.Count < 1) return;
            FirstExtentionFiles = null;
            SecondExtentionFiles = null;
            var useExtention = Extentions.Where(s => s.Chk == true).ToList();
            int num = useExtention.Count();
            for (int i = 0; i< num; i++)
            {
                var findExtentions = _directoryInfo.EnumerateFiles($"*{useExtention[i].Extenstion}");
                if (i == 0)
                {

                    var temp = new List<Tuple<string, FileInfo>>();
                    foreach (var item in findExtentions)
                    {
                        if (_ExtentionFilesMaxCount == temp.Count)
                        {
                            break;
                        }
                        temp.Add(Tuple.Create(item.Name, item));

                    }
                    FirstExtentionFiles = temp;
                }

                if (i == 1)
                {
                    var temp = new List<Tuple<string, FileInfo>>();
                    foreach (var item in findExtentions)
                    {
                        if (_ExtentionFilesMaxCount == temp.Count)
                        {
                            break;
                        }
                        temp.Add(Tuple.Create(item.Name, item));
                    }
                    SecondExtentionFiles = temp;


                }

            }

            //for (int i = 0; i < 2; i++)
            //{
            //    var findExtentions = _directoryInfo.EnumerateFiles($"*{Extentions[i].Extenstion}");
            //    if (i == 0)
            //    {

            //        var temp = new List<Tuple<string, FileInfo>>();
            //        foreach (var item in findExtentions)
            //        {
            //            temp.Add(Tuple.Create(item.Name, item));
            //        }
            //        FirstExtentionFiles = temp;
            //    }

            //    if (i == 1)
            //    {
            //        var temp = new List<Tuple<string, FileInfo>>();
            //        foreach (var item in findExtentions)
            //        {
            //            temp.Add(Tuple.Create(item.Name, item));
            //        }
            //        SecondExtentionFiles = temp;


            //    }
            //}

        }
        int _ExtentionFilesMaxCount;
        List<Tuple<string, FileInfo>> _FirstExtentionFiles;
        public List<Tuple<string, FileInfo>> FirstExtentionFiles
        {
            get
            {
                return _FirstExtentionFiles;
            }
            set
            {
                _FirstExtentionFiles = value;
                OnPropertyChanged();
            }
        }

        List<Tuple<string, FileInfo>> _SecondExtentionFiles;
        public List<Tuple<string, FileInfo>> SecondExtentionFiles
        {
            get
            {
                return _SecondExtentionFiles;
            }
            set
            {
                _SecondExtentionFiles = value;
                OnPropertyChanged();
            }
        }

        public int ExtentionFilesMaxCount
        {
            get => _ExtentionFilesMaxCount;
            set
            {
                _ExtentionFilesMaxCount = value;
                OnPropertyChanged();
            }
        }

        
    }
}
