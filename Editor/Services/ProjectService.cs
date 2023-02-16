using Editor.GameProject;
using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Services
{
    public class ProjectService : IProjectService
    {
        public (ProjectViewModel, List<ProjectData>) CreateOrAddProject(ProjectData projectData, List<ProjectData> projects)
        {
            if (projectData == null)
            {
                return (null, projects)!;
            }

            if (projects == null)
            {
                return (null, new List<ProjectData>())!;
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

            return (ProjectViewModel.Load(project.FullPath), projects);
        }
    }
}
