using Editor.GameProject.Models;
using Editor.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace Editor.GameProject.ViewModels
{
    [DataContract(Name = "Game")]
    public class ProjectViewModel : ViewModelBase
    {
        public static string Exstension { get; } = ".evrenin";
        [DataMember]
        public string Name { get; private set; } = "New project";
        [DataMember]
        public string Path { get; private set; }
        public string FullPath => $"{Path}{Name}{Exstension}";

        [DataMember(Name = "Scenes")]
        private ObservableCollection<SceneViewModel> _scenes = new ObservableCollection<SceneViewModel>();
        public ReadOnlyObservableCollection<SceneViewModel> Scenes { get; private set; }

        private SceneViewModel _activeScene;
        [DataMember]
        public SceneViewModel ActiveScene
        {
            get { return _activeScene; }
            set
            {
                if (_activeScene != value)
                {
                    _activeScene = value;
                    OnPropertyChanged(nameof(ActiveScene));
                }
            }
        }
        public static ProjectViewModel Current => Application.Current.MainWindow.DataContext as ProjectViewModel;
        public static ProjectViewModel Load(string file)
        {
            Debug.Assert(File.Exists(file));

            return Serializer.FromFile<ProjectViewModel>(file);
        }
        public static void Save(ProjectViewModel project)
        {
            Serializer.ToFile(project, project.FullPath);
        }
        public void Unload()
        {

        }
        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext)
        {
            if (_scenes != null && !_scenes.Any())
            {
                _scenes.Add(new SceneViewModel("Default scene", this, true));
                Scenes = new ReadOnlyObservableCollection<SceneViewModel>(_scenes);
            }
            else if (_scenes != null)
            {
                Scenes = new ReadOnlyObservableCollection<SceneViewModel>(_scenes);
            }

            OnPropertyChanged(nameof(Scenes));
            ActiveScene = Scenes.FirstOrDefault(x => x.IsActive)!;
        }
        public ProjectViewModel(string name, string path)
        {
            Name = name;
            Path = path;
            OnDeserialized(new StreamingContext());
        }
    }
}
