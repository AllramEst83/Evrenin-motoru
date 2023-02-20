using System;

namespace Editor.GameProject.Models
{
    public static class Constants
    {
        public static string ApplicationDataPath { get; set; } = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\EvreninMotoru\";
        public static string ProjectDataPath { get; set; } = $@"{ApplicationDataPath}ProjectData.xml";
    }
}
