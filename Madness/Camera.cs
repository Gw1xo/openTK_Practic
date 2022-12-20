using OpenTK.Mathematics;
using System;

// налаштуємо клас камери
public class Camera
{
    // створимо вектори напрямку руху камери
    private Vector3 _front = -Vector3.UnitZ;

    private Vector3 _up = Vector3.UnitY;

    private Vector3 _right = Vector3.UnitX;

    // обертання по Х
    private float _pitch;

    // обертання по Y
    private float _yaw = -MathHelper.PiOver2; // костиль для повороту на 90 градусів ліворуч

    // поле зору камери
    private float _fov = MathHelper.PiOver2;

    public Camera(Vector3 position, float aspectRatio)
    {
        Position = position;
        AspectRatio = aspectRatio;
    }

    // позиція камери
    public Vector3 Position { get; set; }

    // це просто співвідношення сторін вікна перегляду, яке використовується для матриці проекції.
    public float AspectRatio { private get; set; }

    public Vector3 Front => _front;

    public Vector3 Up => _up;

    public Vector3 Right => _right;

    // перетворюємо градуси в радіани, як тільки встановлюється властивість для покращення продуктивності.
    public float Pitch
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            // фіксуємо значення кроку між -89 і 89, щоб запобігти перекиданню камери догори дном, і багів при використанні кутів ейлера
            var angle = MathHelper.Clamp(value, -89f, 89f);
            _pitch = MathHelper.DegreesToRadians(angle);
            UpdateVectors();
        }
    }

    // перетворюємо градуси в радіани, як тільки встановлюється властивість для покращення продуктивності.
    public float Yaw
    {
        get => MathHelper.RadiansToDegrees(_yaw);
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
            UpdateVectors();
        }
    }

    // поле зору камери (FOV)
    // перетворюємо градуси в радіани, як тільки встановлюється властивість для покращення продуктивності.
    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            var angle = MathHelper.Clamp(value, 1f, 90f);
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    // Отримаємо матрицю перегляду за допомогою функції LookAt,
    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + _front, _up);
    }

    // Отримаємо матрицю проекції
    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
    }

    // Ця функція оновить векторні вершини
    private void UpdateVectors()
    {
        // Спочатку передня матриця обчислюється за допомогою базової тригонометрії.
        _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
        _front.Y = MathF.Sin(_pitch);
        _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

        // перевірка чи нормалізовані вектори, щоб не було гкхм.. цікавих результатів
        _front = Vector3.Normalize(_front);

        // Обчислення правого та верхнього векторів за допомогою перехресного добутку.
        _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        _up = Vector3.Normalize(Vector3.Cross(_right, _front));
    }
}
