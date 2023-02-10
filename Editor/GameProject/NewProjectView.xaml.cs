using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
            if (templateListBox.SelectedItem != null)
            {
                var viewModel = DataContext as NewProjectViewModel;
                var projectPath = viewModel.CreateProject(templateListBox.SelectedItem as ProjectTemplate);

                bool dialogResult = false;
                var window = Window.GetWindow(this);
                if (!string.IsNullOrEmpty(projectPath))
                {
                    var projectData = new ProjectData()
                    {
                        ProjectName = viewModel.ProjectName,
                        ProjectPath = projectPath,
                    };

                    OpenProjectViewModel openProjectViewModel = new OpenProjectViewModel();
                    var project = openProjectViewModel.Open(projectData);
                    dialogResult = true;
                }

                window.DialogResult = dialogResult;
                window.Close();
            }            
        }
    }
}
