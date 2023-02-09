using Editor.GameProject.Models;
using Editor.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Editor.GameProject.ViewModels
{
    public class OpenProjectViewModel : ViewModelBase
    {
        private readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\EvreninMotoru\";
        private readonly string _projectDataPath;
        private readonly ObservableCollection<ProjectData> _projects = new ObservableCollection<ProjectData>();
        public ReadOnlyObservableCollection<ProjectData> Projects { get; }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }
        public OpenProjectViewModel()
        {
            try
            {
                if (!Directory.Exists(_applicationDataPath))
                {
                    Directory.CreateDirectory(_applicationDataPath);
                }

                _projectDataPath = $@"{_applicationDataPath}ProjectData.xml";
                Projects = new ReadOnlyObservableCollection<ProjectData>(_projects);
                ReadProjectData();
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                //TODO: Logging
            }
        }

        public ProjectData Open(ProjectData projectData)
        {
            ReadProjectData();

            var project = _projects.FirstOrDefault(x => x.FullPath == projectData.FullPath);
            if (project != null)
            {
                project.Date = DateTime.Now;
            }
            else
            {
                project = projectData;
                project.Date = DateTime.Now;
                _projects.Add(project);
            }

            WriteProjectData();

            return null;
        }

        public void DeleteProject(ProjectData projectData)
        {
            ReadProjectData();

            var project = _projects.FirstOrDefault(_ => _.FullPath == projectData.FullPath && _.ProjectName == projectData.ProjectName);
            if (project != null)
            {
                _projects.Remove(project);
                WriteProjectData();
                Message = "Project has been deleted";
            }
        }

        private void ReadProjectData()
        {
            if (File.Exists(_projectDataPath))
            {
                var projects = Serializer.FromFile<ProjectDataList>(_projectDataPath).Projects.OrderBy(x => x.Date);
                _projects.Clear();

                foreach (var project in projects)
                {
                    if (File.Exists(project.FullPath))
                    {
                        project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\.Evrenin\Icon.png");
                        project.Screenshot = File.ReadAllBytes($@"{project.ProjectPath}\.Evrenin\Screenshot.png");
                        _projects.Add(project);
                    }
                }
            }
        }

        private void WriteProjectData()
        {
            var projects = _projects.OrderBy(x => x.Date).ToList();
            var projectDataList = new ProjectDataList() { Projects = projects };

            Serializer.ToFile(projectDataList, _projectDataPath);
        }


    }
}
