using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using Speedy.Core;
using Speedy.UI.ViewModels;
using Speedy.UI.Views;

namespace Speedy.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var monitorLoop = new NetworkMonitorLoop();

            using(var cancellationTokenSource = new System.Threading.CancellationTokenSource())
            {
                monitorLoop.Run(cancellationTokenSource.Token);
                BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel(monitorLoop));
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
