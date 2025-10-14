#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

void main(void)
{
    
    // 텍스처 좌표는 정점 셰이더에서 필요가 없기에, 바로 아웃해준다.
    // fragment 셰이더에서 사용
    texCoord = aTexCoord;

    // 위치는 그대로 입력
    gl_Position = vec4(aPosition, 1.0);
}