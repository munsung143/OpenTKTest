#version 330 core
layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}


//최종 위치에 인풋되는 위치 정보를 그대로 넣는다.