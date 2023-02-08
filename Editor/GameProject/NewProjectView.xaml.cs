using Editor.GameProject.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Editor.GameProject
{
    /// <summary>
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    public partial class NewProjectView : UserControl
    {
        public NewProjectView()
        {
            InitializeComponent();
        }

        private void OnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fInfo = new FileInfo(openFileDialog.FileName);
                filePathTxt.Text = fInfo.DirectoryName;
            }
        }

        private void OnCreate_Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NewProjectViewModel;
            var projectPath = viewModel.CreateProject(templateListBox.SelectedItem as ProjectTemplate);

            bool dialogResult = false;
            var window = Window.GetWindow(this);
            if (!string.IsNullOrEmpty(projectPath))
            {
                dialogResult = true;
            }

            window.DialogResult = dialogResult;
            window.Close();
        }
    }
}
