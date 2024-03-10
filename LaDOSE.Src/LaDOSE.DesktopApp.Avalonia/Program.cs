using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.ComponentModel;
using LaDOSE.REST;
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
        currentMutable.RegisterLazySingleton<RestService>(()=>
        {
            var restService = new RestService();
            restService.Connect(new Uri("http://localhost:5000"),"dev","dev");
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