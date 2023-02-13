using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.GameProject.Models
{
    public static class Constants
    {
        public static string ApplicationDataPath { get; set; } = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\EvreninMotoru\";
        public static string ProjectDataPath { get; set; } = $@"{ApplicationDataPath}ProjectData.xml";
    }
}
