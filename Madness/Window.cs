using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


//побудуємо примітив
public class Window : GameWindow
{

    // побудуємо вершини трикутника
    private readonly float[] _vertices =
    {
         0.0f,  0.5f, 0.0f, //верхня вершина
        -0,5f, -0.5f, 0.0f, //нижня ліва вершина
         0,5f, -0,5f, 0.0f, //нижня права вершина
    };

    // оголосимо дескриптори VBA i VAO
    private int _vertexBufferObject;

    private int _vertexArrayObject;

    // створимо об'єкт шейдеру
    private Shader _shader;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        // встановимо колір очищення
        GL.ClearColor(0.34f, 0.47f, 0.2f, 1.0f);

        // Нам потрібно обробляти вершини графічною картою
        // створимо VBO 

        // спочатку створимо буфер. ця функція повертає дескриптор
        _vertexBufferObject = GL.GenBuffer();

        // прив'яжемо буфер
        // Перший аргумент — це enum, що визначає тип буфера, який ми прив’язуємо. VBO — це ArrayBuffer.
        // Другим аргументом є дескриптор нашого буфера.
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

        // тепер завантажимо вершини в буфер
        // Аргументи функції:
        //      1.До якого буфера надсилати дані
        //      2.Обсяг даних у байтах
        //      3.Самі вершини
        //      4.Спосіб використання буферу
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // оскільки буфер вершин не структорований і це просто купа даних
        // структуруємо його через VAO
        // створимо VAO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        // Тепер нам потрібно налаштувати, як вершинний шейдер інтерпретуватиме дані VBO;
        // Для цього ми використовуємо функцію GL.VertexAttribPointer
        // Ця функція має два завдання: повідомити opengl про формат даних, а також пов’язати поточний буфер масиву з VAO.
        // Аргументи:
        //      1.Розташування вхідної змінної в шейдері
        //      2.Скільки елементів буде надіслано до змінної
        //      3.Тип набору елементів
        //      4.Чи потрібно перетворювати дані в нормалізовані координати
        //      5.Крок
        //      6.Зсув
        //      7.Stride i Offset
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        //увімкнемо 0 атрибут шейдеру
        GL.EnableVertexAttribArray(0);

        // тепер створимо шейдерні програми
        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        //тепер увімкнемо шейдер
        _shader.UseShader();

        //налаштування завершено перейдемо до відмальовки трикутника

    }
    protected override void OnResize(ResizeEventArgs e) 
    {
        base.OnResize(e);
        // відкоректуємо відображення при зміні розмірів екрану
        GL.Viewport(0, 0, Size.X, Size.Y);
    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
        base.OnUpdateFrame(args);
    }

    // Тепер, коли ініціалізацію виконано, створимо цикл візуалізації
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        //очистимо зображення використовуючи GL.ClearColor який ми встановили раніше
        //для цього очистимо бітовий буфер кольору
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // біндим шейдер
        _shader.UseShader();

        // підв'язуємо VAO
        GL.BindVertexArray(_vertexArrayObject);

        // виключемо функцію малювання
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        
        // це все що потрібно для відмальовки
        SwapBuffers();
        base.OnRenderFrame(args);
    }

    //загалом не слід видаляти все при виході з програми але хай буде поки так
    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);

        GL.DeleteProgram(_shader.Handle);

        base.OnUnload();
    }
}
