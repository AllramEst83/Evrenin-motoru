using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
using Editor.Repositories;
using Microsoft.Extensions.DependencyInjection;
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
            DataContext = App.AppHost?.Services.GetRequiredService<NewProjectViewModel>();

        }

        private void OnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
