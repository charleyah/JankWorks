#version 330 core
        
out vec4 fragcolour;
in vec3 colour;

void main()
{
    fragcolour = vec4(colour, 1.0);
}