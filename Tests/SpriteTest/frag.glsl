#version 330 core

in vec2 texcoord;
out vec4 fragcolour;

uniform sampler2D image;

void main()
{
	fragcolour = texture(image, texcoord);
}