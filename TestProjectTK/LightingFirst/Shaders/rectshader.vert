#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aNormal;

out vec2 texCoord;
out vec3 normal;
out vec3 fragPos;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

void main(void)
{
    
    // Pass texture coordinates directly to fragment shader
    texCoord = aTexCoord;
    normal = aNormal;

    // Position remains the same
    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
    fragPos = vec3(transform * vec4(aPosition, 1.0));
}