#version 330 core

layout(location = 0) in vec2 position;
layout(location = 1) in vec3 inputcolour;
layout(location = 2) in vec2 inTexCoord;
        
out vec3 colour;
out vec2 texCoord;

uniform mat4 transform;

void main()
{
    colour = inputcolour;
    texCoord = inTexCoord;
    gl_Position = transform * vec4(position.x, position.y, 0.0, 1.0);
}


