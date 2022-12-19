using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;


public class Window : GameWindow
{
    // Оскільки ми додаємо текстуру, ми змінюємо масив вершин, щоб включити координати текстури.
    // Координати текстури в діапазоні від 0,0 до 1,0, де (0,0, 0,0) представляють нижній лівий кут, а (1,0, 1,0) представляють верхній правий.
    private readonly float[] _vertices =
    {
            // Координати               Координати текстури
             0.5f,  0.5f, 0.0f,         1.0f, 1.0f, 
             0.5f, -0.5f, 0.0f,         1.0f, 0.0f, 
            -0.5f, -0.5f, 0.0f,         0.0f, 0.0f, 
            -0.5f,  0.5f, 0.0f,         0.0f, 1.0f  
        };

    private readonly uint[] _indices =
    {
            0, 1, 3,
            1, 2, 3
        };

    private int _elementBufferObject;

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader _shader;

    private Texture _texture;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        _shader = new Shader("Shaders/shader.vert", "Shaders/fragshader.frag");
        _shader.Use();

        // Оскільки між початком першої вершини та початком другої тепер є 5 флоатів,
        // ми змінюємо крок від 3 * sizeof(float) до 5 * sizeof(float).
        // Це тепер передасть новий масив вершин до буфера.
        var vertexLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        // Далі ми також встановлюємо координати текстури. Це працює приблизно так само.
        // Ми додаємо зсув 3, оскільки координати текстури йдуть після даних позиції.
        // Ми також змінюємо кількість даних на 2, тому що для координат текстури є лише 2 числа з плаваючою точкою.
        var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _texture = Texture.LoadFromFile("Resources/container.png");
        _texture.Use(TextureUnit.Texture0);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.BindVertexArray(_vertexArrayObject);

        _texture.Use(TextureUnit.Texture0);
        _shader.Use();

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
