using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            DataContext = App.AppHost?.Services.GetRequiredService<OpenProjectViewModel>();
        }

        private void OnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
