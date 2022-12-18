#version 330 core

// позиція змінної 0
layout(location = 0) in vec3 aPosition; 

// ключове слово óut передає значення в наступне по ланцюжку значення
out vec4 vertexColor;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0);

	// передамо темночервоний колір 
	vertexColor = vec4(0.5, 0.0, 0.0, 1.0);
}