using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
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

namespace Editor.GameProject
{
    /// <summary>
    /// Interaction logic for test.xaml
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
            var viewModel = DataContext as OpenProjectViewModel;
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
