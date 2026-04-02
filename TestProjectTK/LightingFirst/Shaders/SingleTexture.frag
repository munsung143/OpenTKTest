#version 330

out vec4 outputColor;

// get from vertex shader
in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

// sampler2D - bind with textureunit.textrue0
uniform sampler2D texture0;

uniform vec3 lightPos;
uniform vec3 lightColor;

uniform vec4 objectColor;

void main()
{
    // texture function
    vec4 defaultColor = texture(texture0, texCoord) * objectColor;
    vec3 lightDirection = normalize(lightPos - fragPos);
    vec3 diffuse = lightColor * max(dot(normalize(normal), lightDirection), 0.0);
    outputColor = defaultColor * vec4(diffuse, 1.0);
}