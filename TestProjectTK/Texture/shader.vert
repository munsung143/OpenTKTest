#version 330 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

void main(void)
{
    texCoord = aTexCoord; //텍스처 좌표도 그대로 아웃풋 함

    gl_Position = vec4(aPosition, 1.0); // 위치 정보는 그대로 넣음
}