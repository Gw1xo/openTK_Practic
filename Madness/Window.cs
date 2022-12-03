using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

//Тут буде написаний увесь код OpenGL
//Open Toolkit дозволяє перевизначати кілька функцій для розширення функціональності;
public class Window : GameWindow
{
    // Простий конструктор, який дозволяє установлювати такі властивості, як розмір вікна, заголовок, FPS
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }
    /* ігровий цикл:
          Initialisation
               |
            __isRun___
           |    |     |
           |__Resize  | 
           |    |     |
           |__Update  |
           |    |     |
           |__Draw    |
                      |
                 _____| 
                 |
              Delete*/
    //перевизначимо метод ініціалізації
    protected override void OnLoad()
    {
        base.OnLoad();
    }
    //метод зміни розмірів
    protected override void OnResize(ResizeEventArgs e) //аргумент події зміни розмірів
    {
        base.OnResize(e);
    }
    //метод оновлення кадру
    protected override void OnUpdateFrame(FrameEventArgs args)// аргумент події кадру
    {
        //назначимо r g b на зміну кольорів
        if (KeyboardState.IsKeyDown(Keys.R))
            GL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
        if (KeyboardState.IsKeyDown(Keys.G))
            GL.ClearColor(0.0f, 1.0f, 0.0f, 1.0f);
        if (KeyboardState.IsKeyDown(Keys.B))
            GL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);
        //назначимо ескей на вимкнення
        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
        base.OnUpdateFrame(args);
    }
    //метод відмальовки кадру
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        //застосуємо очищення екрану кольором
        GL.Clear(ClearBufferMask.ColorBufferBit);
        //зміна буферу
        SwapBuffers();
        base.OnRenderFrame(args);
    }
    //метод обробника закриття вікна
    protected override void OnUnload()
    {
        base.OnUnload();
    }
}
