using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

// створимо ЕВО
public class Window : GameWindow
{
    // розширюємо масив вершин
    private readonly float[] _vertices =
    {
             0.5f,  0.5f, 0.0f, // верхній правий
             0.5f, -0.5f, 0.0f, // нижній правий
            -0.5f, -0.5f, 0.0f, // нижній лівий
            -0.5f,  0.5f, 0.0f, // верхній лівий
        };

    // створимо масив індексів
    private readonly uint[] _indices =
    {
            0, 1, 3, // перший трикутник
            1, 2, 3  // другий трикутник
        };

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader _shader;

    // створимо вказівник ЕВО
    private int _elementBufferObject;

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

        // генеруємо ЕВО
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        // заносимо данні в ЕВО
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
      
        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();

        GL.BindVertexArray(_vertexArrayObject);

        // DrawElements
        // Аргументи:
        // Примітивний тип для малювання. Трикутники в даному випадку.
        // Скільки індексів потрібно намалювати. У цьому випадку шість.
        // Тип даних індексів. Індекси є unsigned int, тому ми також хочемо, щоб це було тут.
        // Зміщення в EBO. Встановіть значення 0, оскільки ми хочемо намалювати все.
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

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

