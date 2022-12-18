// встановлюємо версію GLSL
#version 330 core

//встановимо вхідну змінну aPosition
in vec3 aPosition;


void main(void)
{
// gl_Position — кінцева позиція вершини;
    gl_Position = vec4(aPosition, 1.0);
}