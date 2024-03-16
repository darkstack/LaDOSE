using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.ComponentModel;
using System.IO;
using LaDOSE.REST;
using Microsoft.Extensions.Configuration;
using Splat;
// using Xilium.CefGlue;
// using Xilium.CefGlue.Common;
// using Avalonia.Visuals;
namespace LaDOSE.DesktopApp.Avalonia;

sealed class Program
{
    

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        RegisterDependencies(Locator.CurrentMutable, Locator.Current);
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);  
    }

    private static void RegisterDependencies(IMutableDependencyResolver currentMutable, IReadonlyDependencyResolver current)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("settings.json", optional: true, reloadOnChange: true).Build();
        var restUrl = builder["REST:Url"].ToString();
        var restUser = builder["REST:User"].ToString();
        var restPassword = builder["REST:Password"].ToString();
        currentMutable.RegisterLazySingleton<RestService>(()=>
        {
            var restService = new RestService();
            restService.Connect(new Uri(restUrl),restUser,restPassword);
            return restService;
        });
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .AfterSetup(_ =>
            {
                // CefRuntimeLoader.Initialize(new CefSettings()
                // {
                //     WindowlessRenderingEnabled = true,
                //     NoSandbox = true,
                // });
            })
            .UseReactiveUI();
}