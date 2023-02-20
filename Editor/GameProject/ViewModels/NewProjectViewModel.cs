using Editor.Exceptions;
using Editor.GameProject.Models;
using Editor.Handlers;
using Editor.Repositories;
using Editor.Services;
using Editor.Utils;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Editor.GameProject.ViewModels
{
    public class NewProjectViewModel : ViewModelBase
    {
        private ObservableCollection<ProjectTemplate> _projectTemplates = new();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }
        //TODO: Get path from the installation location 
        private readonly string _template = @"..\..\Editor\ProjectTemplates";
        public IFileRepository fileRepository { get; }
        public IProjectService projectService { get; }

        public NewProjectViewModel(IFileRepository _fileRepository, IProjectService _projectService)
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
            fileRepository = _fileRepository;
            projectService = _projectService;

            try
            {
                GetProjectTemplates();
                ValidateProjectPath();

            }
            catch (ReadFromFileException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #region DynamicVariables

        private ProjectTemplate _selectedItem = new();
        public ProjectTemplate SelectedItem
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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        private bool _validationIsValid;
        public bool validationIsValid
        {
            get => _validationIsValid;
            set
            {
                if (_validationIsValid != value)
                {
                    _validationIsValid = value;
                    OnPropertyChanged(nameof(validationIsValid));
                }
            }
        }

        private string _projectName = "NewProject";
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\EvreninMotoruProjects\";
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (_projectPath != value)
                {
                    _projectPath = value;

                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }
        #endregion

        #region ButtonCommands

        private ICommand _openFileDialogClickCommand;
        public ICommand OpenFileDialogClickCommand
        {
            get
            {
                return _openFileDialogClickCommand ??= new CommandHandler(OpenFileDialog, () => CanExecute);
            }
        }

        private ICommand _createlickCommand;
        public ICommand CreateClickCommand
        {
            get
            {
                return _createlickCommand ??= new CommandHandler(ExecuteOnCreateProject, () => CanExecute);
            }
        }
        public bool CanExecute
        {
            get
            {
                return ValidateProjectPath() == true && SelectedItem.ProjectType != null && validationIsValid;
            }
        }
        #endregion

        #region PublicFunctions
        public void ExecuteOnCreateProject(object args)
        {
            if (args == null)
            {
                return;
            }

            if (SelectedItem == null)
            {
                return;
            }

            var newProjectView = args as NewProjectView;
            var projectPath = CreateProject(SelectedItem);
            var dialogResult = false;
            var window = Window.GetWindow(newProjectView);

            if (!string.IsNullOrEmpty(projectPath))
            {
                ProjectData projectData = new()
                {
                    ProjectName = ProjectName,
                    ProjectPath = projectPath,
                };

                var projects = fileRepository.GetProjectData(Constants.ProjectDataPath);
                (var project, var lostOfProjetcs) = projectService.CreateOrAddProject(projectData, projects);
                fileRepository.SaveProjectData(Constants.ProjectDataPath, lostOfProjetcs);

                window.DataContext = project;
                dialogResult = true;
            }

            window.DialogResult = dialogResult;
            window.Close();

        }
        #endregion

        #region PrivateFunction

        private void OpenFileDialog(object args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fInfo = new FileInfo(openFileDialog.FileName);
                ProjectPath = fInfo.DirectoryName ?? string.Empty;
            }
        }
        private string CreateProject(ProjectTemplate projectTemplate)
        {
            ValidateProjectPath();
            if (!validationIsValid)
            {
                return string.Empty;
            }

            EndsInProjectSeperator();
            var path = $@"{ProjectPath}{ProjectName}\";

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var folder in projectTemplate.Folders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), folder)));
                }

                var directoryInfo = new DirectoryInfo(path + @".Evrenin\");
                directoryInfo.Attributes |= FileAttributes.Hidden;

                File.Copy(projectTemplate.IconFilePath, Path.GetFullPath(Path.Combine(directoryInfo.FullName, "Icon.png")));
                File.Copy(projectTemplate.ScreenshotFilePath, Path.GetFullPath(Path.Combine(directoryInfo.FullName, "Screenshot.png")));

                var project = new ProjectViewModel(ProjectName, path);
                Serializer.ToFile(project, path + ProjectName + ProjectViewModel.Exstension);

                var projectXml = File.ReadAllText(projectTemplate.ProjectFilePath);
                projectXml = string.Format(projectXml, ProjectName, ProjectPath);
                var projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{ProjectViewModel.Exstension}"));
                File.WriteAllText(projectPath, projectXml);

                return path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //TODO - Logging
                return string.Empty;
            }
        }

        private void GetProjectTemplates()
        {
            var templateFiles = Directory.GetFiles(_template, "template.xml", SearchOption.AllDirectories);

            Debug.Assert(templateFiles.Any());

            foreach (var file in templateFiles)
            {
                var template = Serializer.FromFile<ProjectTemplate>(file);

                if (string.IsNullOrEmpty(file))
                {
                    continue;
                }

                template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
                template.ScreenshotFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Screenshot.png"));
                template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));
                template.Icon = File.ReadAllBytes(template.IconFilePath);
                template.Screenshot = File.ReadAllBytes(template.ScreenshotFilePath);

                _projectTemplates.Add(template);
            };
        }

        private bool ValidateProjectPath()
        {
            EndsInProjectSeperator();

            var path = ProjectPath;
            path += $@"{ProjectName}";
            validationIsValid = false;

            if (string.IsNullOrWhiteSpace(ProjectName.Trim()))
            {
                ErrorMessage = "Type in a project name.";
            }
            else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ErrorMessage = "Invalid character(s) used in project name.";
            }
            else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
            {
                ErrorMessage = "Select a valid project folder.";
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                ErrorMessage = "Invalid character(s) used in project path.";
            }
            else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
            {
                ErrorMessage = "Selected project folder or the project name already exists.";
            }
            else if (SelectedItem.ProjectType == null)
            {
                ErrorMessage = "Please select a template.";
            }
            else
            {
                ErrorMessage = string.Empty;
                validationIsValid = true;
            }

            return validationIsValid;
        }

        private void EndsInProjectSeperator()
        {
            if (!Path.EndsInDirectorySeparator(ProjectPath))
            {
                ProjectPath += @"\";
            }
        }
        #endregion       
    }
}
