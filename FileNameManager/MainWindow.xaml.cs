using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yutil.YFile

{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        YFileNameManager _yFileNameManager;
        public MainWindow()
        {
            InitializeComponent();
            _yFileNameManager = new YFileNameManager();
            DataContext = _yFileNameManager;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _yFileNameManager.UserPath =_yFileNameManager.OpenFolderPath();
            _yFileNameManager.OpenDirectory(_yFileNameManager.UserPath);
        }


        private void Btn_ReName_Click(object sender, RoutedEventArgs e)
        {
            _yFileNameManager.ReNameMake();
        }



        private void BntFileConfirm_Click(object sender, RoutedEventArgs e)
        {
            _yFileNameManager.ReNameMake();
            _yFileNameManager.FileNmChange();
            _yFileNameManager.OpenDirectory(_yFileNameManager.UserPath);
            if (_yFileNameManager.IsfolderNMChangeMode)
            {
                _yFileNameManager.FolrderNameChange();
            }
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            _yFileNameManager.MakeExtentionFiles("");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _yFileNameManager.MakeExtentionFiles("");
        }

        private void btn폴더수정(object sender, RoutedEventArgs e)
        {
            _yFileNameManager.FolrderNameChange();
            
        }
    }
}
