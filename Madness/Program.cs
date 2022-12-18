using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

internal class Program
{
    private static void Main(string[] args)
    {
        //налаштуємо вікно
        var nativeWindowSettings = new NativeWindowSettings() {
            // елементарні налаштування вікна
            Size = new Vector2i(800, 600),
            Title = "Madness",
            
        };

        //створимо екземпляр вікна
        using(var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
        {
            //запуск вікна методом Run()
            window.Run();
        }
    }
}