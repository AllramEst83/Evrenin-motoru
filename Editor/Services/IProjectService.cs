using Editor.GameProject.Models;
using Editor.GameProject.ViewModels;
using System.Collections.Generic;

namespace Editor.Services
{
    public interface IProjectService
    {
        (ProjectViewModel, List<ProjectData>) CreateOrAddProject(ProjectData projectData, List<ProjectData> projects);
    }
}