using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Editor.GameProject.Models
{
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }
        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }
        public string IconFilePath { get; set; }
        public string ScreenShotFilePath { get; set; }
        public string ProjecttFilePath { get; set; }
    }
}
