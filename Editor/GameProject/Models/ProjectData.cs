using Editor.GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Editor.GameProject.Models
{
    [DataContract]
    public class ProjectData
    {
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectPath { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        public string FullPath { get => $"{ProjectPath}{ProjectName}{ProjectViewModel.Exstension}"; }
        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }
    }
}
