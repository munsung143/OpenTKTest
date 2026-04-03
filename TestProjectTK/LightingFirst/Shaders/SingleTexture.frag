#version 330

out vec4 outputColor;

// get from vertex shader
in vec2 uv;
in vec3 normal;
in vec3 modelPos;

struct Material
{
    vec4 ambient;
    vec4 diffuse;
    vec4 specular;
    float shininess;
};
struct Light
{
    vec3 position;
    vec4 ambient;
    vec4 diffuse;
    vec4 specular;
};

uniform Material material;
uniform Light light;

uniform sampler2D texture0;
uniform vec3 cameraPos;

void main()
{
    vec4 textureColor = texture(texture0, uv);
    vec3 lightDirection = normalize(light.position - modelPos);
    vec3 viewDirection = normalize(cameraPos - modelPos);
    vec3 reflectDirection = reflect(-lightDirection, normal);

    float diff = max(dot(normalize(normal), lightDirection), 0.0);
    float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), material.shininess);

    vec4 resultAmbient = light.ambient * material.ambient * textureColor;
    vec4 resultDiffuse = light.diffuse * diff * material.diffuse * textureColor;
    vec4 resultSpecular = light.specular * spec * material.specular * textureColor;

    outputColor = resultAmbient + resultDiffuse + resultSpecular;
}