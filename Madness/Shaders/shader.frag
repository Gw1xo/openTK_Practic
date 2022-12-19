#version 330 core

out vec4 outputColor;

// ключове слово Uniform дозволяє отримати доступ до змінної шейдера на будь-якому етапі ланцюжка шейдерів
uniform vec4 ourColor; 

void main()
{
    outputColor = ourColor;
}