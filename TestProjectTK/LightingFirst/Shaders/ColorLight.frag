#version 330

out vec4 outputColor;

// get from vertex shader
in vec2 texCoord;
in vec3 normal;

// sampler2D - bind with textureunit.textrue0
uniform sampler2D texture0;

uniform vec4 objectColor;
uniform vec4 lightColor;

void main()
{
    // texture function
    outputColor = lightColor * objectColor;
}