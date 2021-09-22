#version 330 core

in vec2 uv;
in vec4 colour;

uniform sampler2D Texture;

out vec4 frag;

void main()
{
	frag = texture(Texture, uv) * colour;
}