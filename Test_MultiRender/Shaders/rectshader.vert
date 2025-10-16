#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 transform;

void main(void)
{
    
    // Pass texture coordinates directly to fragment shader
    texCoord = aTexCoord;

    // Position remains the same
    gl_Position = vec4(aPosition, 1.0) * transform;
}