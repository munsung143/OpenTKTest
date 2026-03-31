#version 330

out vec4 outputColor;

// get from vertex shader
in vec2 texCoord;

// sampler2D - bind with textureunit.textrue0
uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    // texture function
    outputColor = mix(texture(texture0, texCoord), texture(texture1, texCoord), 0.2);
}