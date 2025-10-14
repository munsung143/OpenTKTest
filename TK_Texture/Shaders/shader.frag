#version 330

out vec4 outputColor;

// 정점 셰이더에서 받아옴
in vec2 texCoord;

// sampler2D - 셰이더의 텍스처 변수, 텍스처 유닛에 바인드됨
uniform sampler2D texture0;

void main()
{
    // 텍스처를 사용하는 함수
    outputColor = texture(texture0, texCoord);
}