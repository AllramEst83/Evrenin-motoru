using System.Diagnostics;
using System.Runtime.Serialization;

namespace Editor.GameProject.ViewModels
{
    [DataContract(Name = "Scene")]
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

        private bool _isActive;
        [DataMember]
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }

                
            }
        }

        public SceneViewModel(string name, ProjectViewModel project, bool isActive)
        {
            Debug.Assert(project != null);
            Name = name;
            Project = project;
            IsActive = isActive;
        }
    }
}
