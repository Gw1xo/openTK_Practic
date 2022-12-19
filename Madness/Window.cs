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
    private Texture _texture2;

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

        // щось через коментарі шейдери перестали працювати
        // тому коментуватиму зміни тут
        // додано вхідний параметер для другої текстури
        _shader = new Shader("Shaders/shader.vert", "Shaders/fragshader.frag");
        _shader.Use();

        var vertexLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _texture = Texture.LoadFromFile("Resources/container.png");
        // Перша текстура йде в блок текстури 0.
        _texture.Use(TextureUnit.Texture0);

        // оскільки System.Drawing читає пікселі не так як очікує OpenGL
        _texture2 = Texture.LoadFromFile("Resources/awesomeface.png");
        // друга текстура йде в блок текстури 1.
        _texture2.Use(TextureUnit.Texture1);

        // Далі ми повинні налаштувати вибірки в шейдерах для використання правильних текстур.
        // Int, який ми надсилаємо у uniform, вказує, яку текстурну одиницю має використовувати семплер.
        _shader.SetInt("texture0", 0);
        _shader.SetInt("texture1", 1);
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
