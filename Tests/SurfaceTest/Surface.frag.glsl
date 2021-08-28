#version 330 core
in vec2 texcoord;
out vec4 fragcolour;
uniform sampler2D rtexture;

void main()
{
	fragcolour = texture(rtexture, texcoord);
}