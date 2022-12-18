using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.IO;

//реалізуємо клас для створення шейдерів
public sealed class Shader
{
    public readonly int Handle;

    private readonly Dictionary<string, int> _uniformLocations;
    
    public Shader(string vertPath, string fragPath)
    {
        // Завантажуємо вершинний шейдер і компілюємо
        var shaderSource = File.ReadAllText(vertPath);

        // Створимо порожній вершинний шейдер
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);// Перелік ShaderType вказує, який тип шейдера буде створено

        // Тепер зв'яжемо код GLSL
        GL.ShaderSource(vertexShader, shaderSource);

        // А тепер скомпілюємо його
        CompileShader(vertexShader);

        // зробимо те саме і для фрагментного шейдеру
        shaderSource = File.ReadAllText(fragPath);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);
        CompileShader(fragmentShader);

        // тепер об'єднаємо ці два шейдера в шейдерну програму
        Handle = GL.CreateProgram();//функція ініціалізує програму і повертає обробник

        // робимо прив'язку обох шейдерів
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader (Handle, fragmentShader);

        // тепер зв'яжемо їх
        LinkProgram(Handle);

        // оскільки ми вже прив'язали шейдери до програми то нам не потрібні окремі шейдери
        // тому відв'яжемо і видалимо їх
        GL.DetachShader(Handle, fragmentShader);
        GL.DetachShader(Handle, vertexShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        //шейдер готовий до використання
        //тепер оптимізуємо використання закешувавши однорідні розташування шейдерів
        //оскільки кверінг у шейдера дуже повільний ми зробимо це один раз при ініціалізаціїї
        //і використовуватимо ці значення пізніше

        // спочатку отримаємо кількість активних юніформ у шейдері
        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUnifotms);

        //виділимо словник для зберігання розташувань
        _uniformLocations = new Dictionary<string, int>();

        // перебір усіх юніформ шейдерної програми
        for(var i=0; i < numberOfUnifotms; i++)
        {
            // отримуємо ім'я юніформи
            var key = GL.GetActiveUniform(Handle, i, out _, out _);

            //отримуємо її розташування
            var location = GL.GetUniformLocation(Handle, key);

            // і додаємо це все до словника
            _uniformLocations.Add(key, location);
        }
        // ініціалізатор шейдеру готовий
    }

    // напишемо метод компілювання шейдеру з перевіркою на успішність
    private static void CompileShader(int shader)
    {
        // компілюємо шейдер
        GL.CompileShader(shader);

        // перевіряємо на помилку компіляції, оскільки при невдачному компілюванні функція
        // не викидає помилку тому потрібно перевіряти статус компіляції
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);//статус компіляціїї знаходиться в перелічені параметрів шейдеру
        if (status != (int)All.True)
        {
            // тут перевірка статусу компіляції
            // якщо помилка кидаємо лог помилки для детальної інформації
            // та з яким шейдером проблема
            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }
    }

    // напишемо лінковщик програми з перевіркою на успішність лінкування
    private static void LinkProgram(int program)
    {
        // лінкуємо програму
        GL.LinkProgram(program);

        // перевірка на помилки лінкування
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var status);
        if (status != (int)All.True)
        {
            // аналогічно до компіляції шейдера
            var infoLog = GL.GetProgramInfoLog(program);
            throw new Exception($"Error occurred whilst compiling Shader({program}).\n\n{infoLog}");
        }

    }

    //інкапсулюємо увімкнення шейдерної програми
    public void UseShader()
    {
        GL.UseProgram(Handle);
    }

    // отримаємо розташування 
    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }

    // Сетери юніформ
    //  1.Прив'язуємо програму для юніформи
    //  2.Отримуємо маркер розташування юніформи
    //  3.Використовуємо відповідну функцію GL.Uniform* щоб встановити форму

    /// <summary>
    /// Встановлення int-юніформи для цього шейдера 
    /// </summary>
    /// <param name="name">Ім'я юніформи</param>
    /// <param name="data">Данні для встановлення</param>
    public void SetInt(string name, int data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    /// <summary>
    /// Встановлення float-юніформи для цього шейдера 
    /// </summary>
    /// <param name="name">Ім'я юніформи</param>
    /// <param name="data">Данні для встановлення</param>
    public void SetFloat(string name, float data)
    {
        GL.UseProgram(Handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    /// <summary>
    /// Встановлення Matrix4-юніформи для цього шейдера 
    /// </summary>
    /// <param name="name">Ім'я юніформи</param>
    /// <param name="data">Данні для встановлення</param>
    /// <remarks>
    ///   <para>
    ///   Матриця транспонується перед надсиланням дот шейдеру
    ///   </para>
    /// </remarks>
    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(Handle);
        GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }

    /// <summary>
    /// Встановлення Vector3-юніформи для цього шейдера 
    /// </summary>
    /// <param name="name">Ім'я юніформи</param>
    /// <param name="data">Данні для встановлення</param>
    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(Handle);
        GL.Uniform3(_uniformLocations[name], data);
    }
    //клас шейдеру готовий
}
