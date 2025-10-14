#version 330

out vec4 outputColor;

// get from vertex shader
in vec2 texCoord;

// sampler2D - bind with textureunit.textrue0
uniform sampler2D texture0;

void main()
{
    // texture function
    outputColor = texture(texture0, texCoord);
}