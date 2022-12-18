using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

internal class Program
{
    private static void Main(string[] args)
    {
        //налаштуємо вікно
        var nativeWindowSettings = new NativeWindowSettings() {
            // елементарні налаштування вікна
            Size = new Vector2i(800, 600),
            Title = "Madness",
            Flags = ContextFlags.ForwardCompatible,
        };

        //створимо екземпляр вікна
        using(var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
        {
            //запуск вікна методом Run()
            window.Run();
        }
    }
}