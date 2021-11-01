#version 330 core

layout(location = 0) in vec2 position;
layout(location = 1) in vec4 col;

out vec2 uv;
out vec4 colour;

void main()
{
	colour = col;
	gl_Position = vec4(position, 0.0, 1.0);
}