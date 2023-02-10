﻿using Editor.Exceptions;
using Editor.GameProject.Models;
using Editor.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Editor.GameProject.ViewModels
{
    public class NewProjectViewModel : ViewModelBase
    {
        private ObservableCollection<ProjectTemplate> _projectTemplates = new();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }
        //TODO: Get path from the installation location 
        private readonly string _template = @"..\..\Editor\ProjectTemplates";


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

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
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


        public NewProjectViewModel()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);

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

        public string CreateProject(ProjectTemplate projectTemplate)
        {
            ValidateProjectPath();
            if (!IsValid)
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

        public void GetProjectTemplates()
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
            IsValid = false;

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
                ErrorMessage = "Selected project folder already exists an is not empty.";
            }
            else
            {
                ErrorMessage = string.Empty;
                IsValid = true;
            }

            return IsValid;
        }

        private void EndsInProjectSeperator()
        {
            if (!Path.EndsInDirectorySeparator(ProjectPath))
            {
                ProjectPath += @"\";
            }
        }
    }
}
