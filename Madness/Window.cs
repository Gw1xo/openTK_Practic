using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;


public class Window : GameWindow
{

    // тут попрацюємо з кольорами
    private readonly float[] _vertices =
    {
             // позиція           // кольор
             0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // нижній правий
            -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // нижній лівий
             0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // верхній
        };

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader _shader;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    // проініціалізуємо OpenGL
    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _vertexBufferObject = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        // створюємо вказівник для 3 позиційних компонентів наших вершин.
        // єдина відмінність тут полягає в тому, що нам потрібно врахувати значення 3 кольорів у змінній stride
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // створюємо новий покажчик для значень кольорів.
        // подібно до попереднього покажчика, ми призначаємо 6 у значенні кроку.
        // нам також потрібно правильно встановити зсув, щоб отримати значення кольорів.
        // дані кольору починаються після даних позиції, тому зміщення дорівнює 3 флоатам.
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        // Потім ми вмикаємо атрибут кольору (location=1), щоб він був доступний для шейдера.
        GL.EnableVertexAttribArray(1);

        GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
        Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();

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
