using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;


public class Window : GameWindow
{   
    // тут розглядено уніфіковані типи змінних
    private readonly float[] _vertices =
    {
            -0.5f, -0.5f, 0.0f, 
             0.5f, -0.5f, 0.0f, 
             0.0f,  0.5f, 0.0f  
        };

    // змусими трикутник пульсувати міє певним діапазоном кольорів
    // для цього нам потрібен таймер адже вінпостійно зростає 
    private Stopwatch _timer;

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader _shader;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _vertexBufferObject = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
        Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();

        // запускаємо секундомір
        _timer = new Stopwatch();
        _timer.Start();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();

        // тут ми отримуємо загальну кількість секунд, що минули з моменту останнього скидання цього методу
        // і ми призначаємо його змінній timeValue, щоб його можна було використовувати для пульсуючого кольору
        double timeValue = _timer.Elapsed.TotalSeconds;

        // оскільки синус набуває значень між -1 та 1 зробимо синусоїдальну зміну кольору
        float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;

        // отримуємо уніфіковану змінну з фрагментного шейдеру
        int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");

        // тут ми призначаємо змінну ourColor у фрагментному шейдері
        // через метод OpenGL Uniform, який приймає значення як окремі значення vec
        GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);
        // GL.Uniform4(vertexColorLocation, new OpenTK.Mathematics.Color4(0f, greenValue, 0f, 0f));
       
        GL.BindVertexArray(_vertexArrayObject);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }
}
