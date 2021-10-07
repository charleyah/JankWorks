#version 330 core

in vec2 uv;
in vec4 col;

uniform sampler2D Texture;

out vec4 frag;

void main()
{		
    frag = col * vec4(1.0, 1.0, 1.0, texture(Texture, uv).r);
}