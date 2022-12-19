using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using StbImageSharp;
using System.IO;
public class Texture
{
    public readonly int Handle;

    public static Texture LoadFromFile(string path)
    {
        // згенеруємо вказівник
        int handle = GL.GenTexture();

        // прив'яжемо його
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);

        // OpenGL має своє транслювання текстури в нижньому лівому куті замість верхнього лівого кута,
        // тому ми використаємо StbImageSharp щоб перевертати зображення під час завантаження.
        StbImage.stbi_set_flip_vertically_on_load(1);

        // Тут ми відкриваємо потік до файлу та передаємо його для завантаження StbImageSharp.
        using (Stream stream = File.OpenRead(path))
        {
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            // тепер створимо текстуру
            // GL.TexImage2D.
            // Аргументи:
            // Тип текстури, яку ми створюємо. Існує багато різних типів текстур, але єдина, яка нам зараз потрібна, це Texture2D.
            // Рівень деталізації.
            // Цільовий формат пікселів. Це формат, у якому OpenGL зберігатиме зображення.
            // Ширина зображення
            // Висота зображення.
            // Границя зображення. Це завжди має бути 0; це застарілий параметр
            // Формат пікселів
            // Тип даних пікселів.
            // пікселі.
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        }

        // cпочатку ми встановлюємо min і mag фільтр. Вони використовуються, коли текстуру зменшено та збільшено відповідно.
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // встановимо режим обтікання. S для осі X, а T для осі Y.
        // встановимо значення Repeat, щоб текстури повторювалися під час обертання
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        // Далі генеруємо mipmaps.
        // Mipmaps — це зменшені копії текстури. Кожен рівень mipmap вдвічі менший за попередній
        // запобігає ефекту морьє
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        return new Texture(handle);
    }

    public Texture(int glHandle)
    {
        Handle = glHandle;
    }

    // Активація текстури
    // використаємо GL.ActiveTexture, щоб установити, до якого слота прив’язується GL.BindTexture.
    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}
