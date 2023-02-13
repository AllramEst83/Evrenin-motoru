using Editor.GameProject;
using Editor.GameProject.ViewModels;
using Editor.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.IO;
using System.Windows;
using static Editor.App;

namespace Editor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {

    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        AppHost = Host.CreateDefaultBuilder()
        .ConfigureServices((hostContext, services) =>
        {
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddSingleton<NewProjectViewModel>();
            services.AddSingleton<OpenProjectViewModel>();
            services.AddSingleton<NewProjectView>();
            services.AddSingleton<OpenProjectView>();
            services.AddSingleton<MainWindow>();


        }).Build();

        await AppHost!.StartAsync();
        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);


    }

    protected override void OnExit(ExitEventArgs e)
    {
        AppHost!.StopAsync();

        base.OnExit(e);
    }
}
