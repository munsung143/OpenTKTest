#version 330

out vec4 outputColor;

// get from vertex shader
in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

// sampler2D - bind with textureunit.textrue0
uniform sampler2D texture0;

uniform vec3 lightPos;
uniform vec4 lightColor;
uniform vec3 viewPos;
uniform vec4 objectColor;

void main()
{
    // texture function
    outputColor = objectColor;
}