#version 330 core

layout(location = 0) in vec3 aPosition;

// Ми додаємо ще одну вхідну змінну для координат текстури.
layout(location = 1) in vec2 aTexCoord;

// Однак вони не потрібні для самого вершинного шейдера.
// Замість цього ми створюємо вихідну змінну, щоб ми могли надіслати ці дані до фрагментного шейдера.

out vec2 texCoord;

void main(void)
{
    // Потім ми додаємо вхідну текстурну координату до вихідної.
    // texCoord тепер можна використовувати у фрагментному шейдері.
    texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0);
}