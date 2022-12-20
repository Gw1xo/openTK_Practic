using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;


// змусимо квадрат обертатись
public class Window : GameWindow
{
    private readonly float[] _vertices =
    {
            // позиція              теккстурні координати
             0.5f,  0.5f, 0.0f,      1.0f, 1.0f, 
             0.5f, -0.5f, 0.0f,      1.0f, 0.0f, 
            -0.5f, -0.5f, 0.0f,      0.0f, 0.0f, 
            -0.5f,  0.5f, 0.0f,      0.0f, 1.0f  
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

    // створимо змінну часу, щоб указати, скільки часу минуло з моменту відкриття програми.
    private double _time;

    // мотім створимо дві матриці для нашого перегляду та проекції
    private Matrix4 _view;
    // це показує, як будуть спроектовані вершини.
    private Matrix4 _projection;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        // Тут ми вмикаємо тестування глибини.Ми також очищаємо буфер глибини в GL.Clear в OnRenderFrame.
        GL.Enable(EnableCap.DepthTest);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        // додано юніфікованні в mat4 змінні model, view , projection 
        _shader = new Shader("Shaders/shader.vert", "Shaders/fragshader.frag");
        _shader.Use();

        var vertexLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _texture = Texture.LoadFromFile("Resources/container.png");
        _texture.Use(TextureUnit.Texture0);

        _texture2 = Texture.LoadFromFile("Resources/awesomeface.png");
        _texture2.Use(TextureUnit.Texture1);

        _shader.SetInt("texture0", 0);
        _shader.SetInt("texture1", 1);

        //  перемістимо вид на три одиниці назад по осі Z.
        _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

        // Для матриці ми використовуємо кілька параметрів.
        //   Поле зору.
        //   Співвідношення сторін.
        //   відсікання ближніх вершин.
        //   відсікання дальніх вершин.
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        // додаємо час, що минув з останнього кадру, помножений на 4,0 для прискорення анімації, до загальної кількості часу.
        _time += 4.0 * e.Time;

        // очищаємо буфер глибини
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.BindVertexArray(_vertexArrayObject);

        _texture.Use(TextureUnit.Texture0);
        _texture2.Use(TextureUnit.Texture1);
        _shader.Use();

        // Нарешті маємо матрицю моделі
        var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));

        // Потім ми передаємо всі ці матриці до вершинного шейдера.
        _shader.SetMatrix4("model", model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);

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
