using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Controls;

namespace Editor.GameProject
{
    /// <summary>
    /// Interaction logic for OpenProjectView.xaml
    /// </summary>
    public partial class OpenProjectView : UserControl
    {
        OpenProjectViewModel openProjectViewModel;
        public OpenProjectView()
        {
            InitializeComponent();
            openProjectViewModel = (OpenProjectViewModel)DataContext;

        }

        private void OnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnDelete_Click(object sender, RoutedEventArgs e)
        {
            var listItem = projectsListBox.SelectedItem as ProjectData;
            if (listItem == null)
            {
                return;
            }

            (string message, string header, MessageBoxButton button, MessageBoxImage messageBoxImage) = GetMessageBoxSettings(listItem);
            if (MessageBox.Show(message, header, button, messageBoxImage) == MessageBoxResult.Yes)
            {
                openProjectViewModel.DeleteProject(listItem);
            }
        }

        public static (string, string, MessageBoxButton, MessageBoxImage) GetMessageBoxSettings(ProjectData projectData)
        {
            var message = $@"Do you really want to delete project ""{projectData.ProjectName}"".";
            var header = "Delete project";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Warning;

            return (message, header, buttons, messageBoxImage);
        }   

        private void OnOpen_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
