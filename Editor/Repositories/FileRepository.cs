using Editor.GameProject.Models;
using Editor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Editor.Repositories
{
    public class FileRepository : IFileRepository
    {
        public List<ProjectData> GetProjectData(string projectDataPath)
        {
            var projects = new List<ProjectData>();
            if (File.Exists(projectDataPath))
            {
                var projectsFromFile = Serializer.FromFile<ProjectDataList>(projectDataPath).Projects.OrderBy(x => x.Date);

                foreach (var project in projectsFromFile)
                {
                    if (File.Exists(project.FullPath))
                    {
                        project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\.Evrenin\Icon.png");
                        project.Screenshot = File.ReadAllBytes($@"{project.ProjectPath}\.Evrenin\Screenshot.png");
                        projects.Add(project);
                    }
                }
            }

            return projects;
        }

        public void SaveProjectData(string projectDataPathList, List<ProjectData> projectsToSave)
        {
            var projects = projectsToSave.OrderBy(x => x.Date).ToList();
            var projectDataList = new ProjectDataList() { Projects = projects };

            Serializer.ToFile(projectDataList, projectDataPathList);
        }

        public List<ProjectData> CreateOrAddProject(ProjectData projectData, List<ProjectData> projects)
        {
            if (projectData == null)
            {
                return projects;
            }

            if (projects == null)
            {
                return new List<ProjectData>();
            }

            var project = projects.FirstOrDefault(x => x.FullPath == projectData.FullPath);
            if (project != null)
            {
                project.Date = DateTime.Now;
            }
            else
            {
                project = projectData;
                project.Date = DateTime.Now;

                projects.Add(project);
            }

            return projects;
        }

        public (bool, List<ProjectData>) DeleteProject(ProjectData project, List<ProjectData> projects)
        {
            if (project == null)
            {
                return (false, projects);
            }

            if (Directory.Exists(project.ProjectPath))
            {
                Directory.Delete(project.ProjectPath, true);
            }

            var successfullyDeleted = projects
                .Remove(projects
                .Where(_ => _.FullPath == project.FullPath && _.ProjectName == project.ProjectName).Single());

            return (successfullyDeleted, projects);
        }
    }
}
