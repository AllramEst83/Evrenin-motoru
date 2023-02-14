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
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Editor.GameProject.ViewModels
{
    public class OpenProjectViewModel : ViewModelBase
    {
        public IFileRepository fileRepository { get; }

        public OpenProjectViewModel(IFileRepository _fileRepository)
        {
            fileRepository = _fileRepository;
            try
            {
                if (!Directory.Exists(Constants.ApplicationDataPath))
                {
                    Directory.CreateDirectory(Constants.ApplicationDataPath);
                }

                Projects = new ObservableCollection<ProjectData>();
                ReadProjectData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //TODO: Logging
            }
        }

        #region Properties
        public bool CanExecute
        {
            get
            {
                return OpenSelectedItem?.ProjectName != null && OpenSelectedItem.ProjectPath != null;
            }
        }
        #endregion

        #region Dynamic properties

        private ProjectData _openSelectedItem = new();
        public ProjectData OpenSelectedItem
        {
            get { return _openSelectedItem; }
            set
            {
                if (_openSelectedItem != value)
                {
                    _openSelectedItem = value;
                    OnPropertyChanged(nameof(OpenSelectedItem));
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
        #endregion

        #region ButtonCommands

        private ICommand _deleteClickCommand;
        public ICommand DeleteClickCommand
        {
            get
            {
                return _deleteClickCommand ??= new CommandHandler(DeleteProject, () => CanExecute);
            }
        }

        private ICommand _openProjectButtonCommand;
        public ICommand OpenProjectCommand
        {
            get
            {
                return _openProjectButtonCommand ??= new CommandHandler(OpenProject, () => CanExecute);
            }
        }

        #endregion

        #region Public methods

        public void DeleteProject(object args)
        {
            if (args == null)
            {
                return;
            }

            if (OpenSelectedItem == null)
            {
                return;
            }

            var projectData = args as ProjectData;
            (string message, string header, MessageBoxButton button, MessageBoxImage messageBoxImage) = MessageBoxHelper.GetMessageBoxSettings(projectData);
            
            if (MessageBox.Show(message, header, button, messageBoxImage) == MessageBoxResult.Yes)
            {
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
        
        private void OpenProject(object args)
        {
            if (args == null)
            {
                return;
            }

            if (OpenSelectedItem == null)
            {
                return;
            }

            var openProjectView = args as OpenProjectView;
            ReadProjectData();

            var listOfProjects = fileRepository.CreateOrAddProject(OpenSelectedItem, Projects.ToList());

            RefreshProjects(listOfProjects);
            WriteProjectData();

            bool dialogResult = true;
            var window = Window.GetWindow(openProjectView);
            window.DialogResult = dialogResult;
            window.Close();
        }

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
            var listOfProjects = fileRepository.GetProjectData(Constants.ProjectDataPath);

            RefreshProjects(listOfProjects);
        }

        private void WriteProjectData()
        {
            fileRepository.SaveProjectData(Constants.ApplicationDataPath, Projects.ToList());
        }
        #endregion
    }
}
