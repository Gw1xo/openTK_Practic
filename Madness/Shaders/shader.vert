#version 330 core

// атрибут позиції 0
layout(location = 0) in vec3 aPosition;  

// сюди потрапляють значення кольорів які призначено в основній програмі
layout(location = 1) in vec3 aColor;

out vec3 ourColor; // передаємо колір до фрагментного шейдеру

void main(void)
{
    gl_Position = vec4(aPosition, 1.0); 

	// використаємо змінну outColor щоб передати інформацію про колір фрагментарному шейдеру
	ourColor = aColor;
}