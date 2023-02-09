using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Editor.GameProject
{
    /// <summary>
    /// Interaction logic for OpenProjectView.xaml
    /// </summary>
    public partial class OpenProjectView : UserControl
    {
        public OpenProjectView()
        {
            InitializeComponent();
        }

        private void OnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnDelete_Click(object sender, RoutedEventArgs e)
        {
            var listItem = projectsListBox.SelectedItem as ProjectData;

            if (ShowWarningMessageBox() == MessageBoxResult.OK)
            {
                var projectData = new ProjectData()
                {
                    ProjectName = listItem.ProjectName,
                    ProjectPath = listItem.ProjectPath
                };

                OpenProjectViewModel openProjectViewModel = new OpenProjectViewModel();
                openProjectViewModel.DeleteProject(projectData);
            }

        }

        public static MessageBoxResult ShowWarningMessageBox()
        {
            var message = "$\"Do you really want to delete project \\\"{listItem.ProjectName}\\\"\"";
            var header = "Delete project";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Warning;

            return MessageBox.Show(message, header, buttons, messageBoxImage);
        }
    }
}
