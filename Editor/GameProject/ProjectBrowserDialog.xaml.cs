using System.Windows;

namespace Editor.GameProject
{
    /// <summary>
    /// Interaction logic for ProjectBrowserDialog.xaml
    /// </summary>
    public partial class ProjectBrowserDialog : Window
    {
        public ProjectBrowserDialog()
        {
            InitializeComponent();
        }

        private void OnToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == openProjectButton)
            {
                if (openProjectButton.IsChecked == true)
                {
                    createProjectButton.IsChecked = false;
                    projectBrowserStackPanel.Margin = new Thickness(0);
                }
                openProjectButton.IsChecked = true;
            }
            else
            {
                if (createProjectButton.IsChecked == true)
                {
                    openProjectButton.IsChecked = false;
                    projectBrowserStackPanel.Margin = new Thickness(-800, 0, 0, 0);
                }
                createProjectButton.IsChecked = true;
            }
        }

    }
}
