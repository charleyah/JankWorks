#version 330 core        

out vec4 fragcolour;

in vec3 colour;
in vec2 texCoord;

uniform sampler2D img;

uniform float brightness;
        
void main()
{
    fragcolour = texture(img, texCoord) * vec4(colour, 1.0) * brightness;
}