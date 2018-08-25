using Avalonia;
using Avalonia.Markup.Xaml;

namespace Avalonia.SkiaWritableBitmap
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
