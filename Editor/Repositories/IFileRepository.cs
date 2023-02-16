using Editor.GameProject.Models;
using System.Collections.Generic;

namespace Editor.Repositories
{
    public interface IFileRepository
    {
        List<ProjectData> GetProjectData(string projectDataPath);
        (bool, List<ProjectData>) DeleteProject(ProjectData project, List<ProjectData> projects);
        void SaveProjectData(string projectDataPathList, List<ProjectData> projectsToSave);
    }
}