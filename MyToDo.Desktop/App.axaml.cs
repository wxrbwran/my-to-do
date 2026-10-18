using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyToDo.Desktop.Services;
using MyToDo.Desktop.ViewModels;
using MyToDo.Desktop.Views;

namespace MyToDo.Desktop
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainWindowViewModel>()
                };
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // 注册 HTTP 客户端（连接本地运行的 MyToDo.Api，默认端口通常为 5000 / 5173）
            services.AddSingleton(sp => new HttpRestClient("http://localhost:5000/"));
            services.AddSingleton<IToDoService, ToDoService>();
            services.AddSingleton<IMemoService, MemoService>();
            services.AddSingleton<IAuthService, AuthService>();

            // 注册 ViewModels
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}
