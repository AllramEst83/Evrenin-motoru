using System.Diagnostics;
using System.Runtime.Serialization;

namespace Editor.GameProject.ViewModels
{
    [DataContract]
    public class SceneViewModel : ViewModelBase
    {
        private string _name;
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        [DataMember]
        public ProjectViewModel Project { get; private set; }

        public SceneViewModel(string name, ProjectViewModel project)
        {
            Debug.Assert(project != null);
            Name = name;
            Project = project;
        }
    }
}
