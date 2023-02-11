using Editor.GameProject.Models;
using Editor.Handlers;
using Editor.Repositories;
using Editor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Editor.GameProject.ViewModels
{
    public class OpenProjectViewModel : ViewModelBase
    {
        #region Fields
        private readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\EvreninMotoru\";
        private readonly string _projectDataPath;
        #endregion

        #region Properties
        public bool CanExecute
        {
            get
            {
                return SelectedItem != null;
            }
        }
        #endregion

        #region Dynamic properties
        private ICommand _deleteClickCommand;
        public ICommand DeleteClickCommand
        {
            get
            {
                return _deleteClickCommand ??= new CommandHandler(DeleteProject, () => CanExecute);
            }
        }

        private ProjectData _selectedItem = new();
        public ProjectData SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

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

        public IFileRepository fileRepository { get; }
        #endregion


        public OpenProjectViewModel(IFileRepository _fileRepository)
        {
            fileRepository = _fileRepository;
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

        #region Public methods
        // Bind To ButtonCommand?
        public ProjectData Open(ProjectData projectData)
        {
            ReadProjectData();

            var listOfProjects = fileRepository.CreateOrAddProject(projectData, Projects.ToList());

            RefreshProjects(listOfProjects);
            WriteProjectData();

            return null;
        }

        public void DeleteProject(object arg)
        {
            if (arg == null)
            {
                return;
            }

            var projectData = arg as ProjectData;

            (string message, string header, MessageBoxButton button, MessageBoxImage messageBoxImage) = MessageBoxHelper.GetMessageBoxSettings(projectData);
            if (MessageBox.Show(message, header, button, messageBoxImage) == MessageBoxResult.Yes)
            {

                if (Directory.Exists(projectData.ProjectPath))
                {
                    Directory.Delete(projectData.ProjectPath, true);
                }

                ReadProjectData();

                (var successfullyDeleted, var updatedList) = fileRepository.DeleteProject(projectData, Projects.ToList());

                if (successfullyDeleted)
                {
                    RefreshProjects(updatedList);
                    WriteProjectData();
                }
            }
        }
        #endregion

        #region Private methods
        
        private void RefreshProjects(List<ProjectData> listOfProjects)
        {
            Projects.Clear();
            listOfProjects.ForEach(x =>
            {
                Projects.Add(x);
            });
        }
        
        private void ReadProjectData()
        {
            var listOfProjects = fileRepository.GetProjectData(_projectDataPath);

            RefreshProjects(listOfProjects);
        }

        private void WriteProjectData()
        {
            fileRepository.SaveProjectData(_projectDataPath, Projects.ToList());
        }
        #endregion
    }
}
