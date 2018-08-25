using Avalonia.Logging.Serilog;
using Avalonia.SkiaWritableBitmap.ViewModels;
using Avalonia.SkiaWritableBitmap.Views;

namespace Avalonia.SkiaWritableBitmap
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()

                .UseSkia()
                //.UseDirect2D1()

                .LogToDebug();
    }
}
