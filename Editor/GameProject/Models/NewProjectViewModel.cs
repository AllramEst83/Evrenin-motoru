using Editor.Exceptions;
using Editor.GameProject.Models;
using Editor.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Editor.GameProject
{
    public class NewProjectViewModel : ViewModelBase
    {
        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }
        //TODO: Get path from the installation location 
        private readonly string _template = @"..\..\Editor\ProjectTemplates";
        private string _name = "NewProject";

        public NewProjectViewModel()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);

            try
            {
                GetProjectTemplates();

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

        public void GetProjectTemplates()
        {
            var templateFiles = Directory.GetFiles(_template, "template.xml", SearchOption.AllDirectories);

            Debug.Assert(templateFiles.Any());

            foreach (var file in templateFiles)
            {
                var template = Serializer.FromFile<ProjectTemplate>(file);

                template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file) ?? "", "Icon.png"));
                template.ScreenShotFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file) ?? "", "Screenshot.png"));
                template.ProjecttFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file) ?? "", template.ProjectFile));
                template.Icon = File.ReadAllBytes(template.IconFilePath);
                template.Screenshot = File.ReadAllBytes(template.ScreenShotFilePath);

                _projectTemplates.Add(template);
            };
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _folderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\EvreninMotoru\";
        public string FolderPath
        {
            get => _folderPath;
            set
            {
                if (_folderPath != value)
                {
                    _folderPath = value;
                    OnPropertyChanged(nameof(FolderPath));
                }
            }
        }
    }
}
