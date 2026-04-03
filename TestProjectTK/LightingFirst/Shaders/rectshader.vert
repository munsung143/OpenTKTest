#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aNormal;

out vec2 uv;
out vec3 normal;
out vec3 modelPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 normalModel;

void main(void)
{
    
    // Pass texture coordinates directly to fragment shader
    uv = aTexCoord;
    normal = aNormal * mat3(normalModel);
    modelPos = vec3(vec4(aPosition, 1.0) * model);
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}