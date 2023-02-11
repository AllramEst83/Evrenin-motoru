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

        private void OnOpen_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
