#version 330 core

layout(location = 0) in vec2 position;
layout(location = 1) in vec2 texpos;
layout(location = 2) in vec4 colour;

out vec2 uv;
out vec4 col;

void main()
{
	uv = texpos;
	col = colour;
	gl_Position = vec4(position, 0.0, 1.0);
}