#version 330 core
out vec4 FragColor;
  
in vec4 vertexColor; // the input variable from the vertex shader (same name and same type)  
uniform vec4 ourColor; // we set this variable in the OpenGL code.

void main()
{
    //FragColor = vertexColor;
    FragColor = ourColor;
} 

// 정점 셰이더의 out 변수와 동일한 이름의 in 변수를 선언하여 값을 받아온다.
// uniform 변수는 모든 셰이더 및 메인 프로그램에서 접근 가능한 전역 변수같은 느낌이다.