using Editor.GameProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Editor.Utils
{
    public static class MessageBoxHelper
    {
        public static (string, string, MessageBoxButton, MessageBoxImage) GetMessageBoxSettings(ProjectData projectData)
        {
            var message = $@"Do you really want to delete project ""{projectData.ProjectName}"".";
            var header = "Delete project";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Warning;

            return (message, header, buttons, messageBoxImage);
        }
    }
}
