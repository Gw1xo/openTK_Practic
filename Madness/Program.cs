using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

internal class Program
{
    private static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings() {
            Size = new Vector2i(800, 600),
            Title = "Madness",
            
        };

        using(var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
        {
            window.Run();
        }
    }
}