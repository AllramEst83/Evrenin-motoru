using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Editor.GameProject.ViewModels
{
    [DataContract(Name = "Game")]
    public class ProjectViewModel : ViewModelBase
    {
        public static string Exstension { get; } = ".evrenin";
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Path { get; private set; }
        public string FullPath => $"{Path}{Name}{Exstension}";
        [DataMember(Name = "Scenes")]
        private ObservableCollection<SceneViewModel> _scenes = new ObservableCollection<SceneViewModel>();
        public ReadOnlyObservableCollection<SceneViewModel> Scenes { get; }
        public ProjectViewModel(string name, string path)
        {
            Name = name;
            Path = path;
            _scenes.Add(new SceneViewModel("Default scene", this));
        }
    }
}
