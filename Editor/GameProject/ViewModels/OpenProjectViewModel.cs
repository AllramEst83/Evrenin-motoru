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

        private ObservableCollection<ProjectData> _projects = new();
        public ObservableCollection<ProjectData> Projects
        {
            get { return _projects; }
            set
            {
                if (_projects != value)
                {
                    _projects = value;
                    OnPropertyChanged(nameof(Projects));
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
                Projects = new ObservableCollection<ProjectData>();
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

            var project = Projects.FirstOrDefault(x => x.FullPath == projectData.FullPath);
            if (project != null)
            {
                project.Date = DateTime.Now;
            }
            else
            {
                project = projectData;
                project.Date = DateTime.Now;
                Projects.Add(project);
            }

            WriteProjectData();

            return null;
        }

        public void DeleteProject(ProjectData projectData)
        {
            ReadProjectData();

            if (Directory.Exists(projectData.ProjectPath))
            {
                Directory.Delete(projectData.ProjectPath, true);
            }

            var successfullyDeleted = Projects
                .Remove(Projects
                .Where(_ => _.FullPath == projectData.FullPath && _.ProjectName == projectData.ProjectName).Single());

            if (successfullyDeleted)
            {
                WriteProjectData();
            }
        }

        private void ReadProjectData()
        {
            if (File.Exists(_projectDataPath))
            {
                var projects = Serializer.FromFile<ProjectDataList>(_projectDataPath).Projects.OrderBy(x => x.Date);
                Projects.Clear();

                foreach (var project in projects)
                {
                    if (File.Exists(project.FullPath))
                    {
                        project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\.Evrenin\Icon.png");
                        project.Screenshot = File.ReadAllBytes($@"{project.ProjectPath}\.Evrenin\Screenshot.png");
                        Projects.Add(project);
                    }
                }
            }
        }

        private void WriteProjectData()
        {
            var projects = Projects.OrderBy(x => x.Date).ToList();
            var projectDataList = new ProjectDataList() { Projects = projects };

            Serializer.ToFile(projectDataList, _projectDataPath);
        }


    }
}
