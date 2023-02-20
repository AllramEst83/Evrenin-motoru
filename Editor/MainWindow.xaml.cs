using Editor.GameProject;
using Editor.GameProject.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
            Closing += OnMainWindowClosing;
        }

        private void OnMainWindowClosing(object? sender, CancelEventArgs e)
        {
            Closing -= OnMainWindowClosing;
            ProjectViewModel.Current?.Unload();
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnMainWindowLoaded;
            OpenProjectBrowserDialog();
        }

        private static void OpenProjectBrowserDialog()
        {
            var projectBrowserDialog = new ProjectBrowserDialog();
            if (projectBrowserDialog.ShowDialog() == false && projectBrowserDialog.DataContext == null)
            {
                Application.Current.Shutdown();
            }
            else
            {
                ProjectViewModel.Current?.Unload();
                Application.Current.MainWindow.DataContext = projectBrowserDialog.DataContext;
            }
        }
    }
}
