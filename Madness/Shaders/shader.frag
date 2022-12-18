#version 330 core

out vec4 outputColor;

// створимо вхідну змінну кольору

in vec4 vertexColor;

void main()
{
    outputColor = vertexColor;
}