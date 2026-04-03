
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using TestProjectTK.Common;

namespace TestProjectTK
{
    public class Cube
    {
        public Vector3 Pos;
        private float size;
        public float[] Vertices;
        public Texture texture;
        public Color4 color;
        public int vao;
        public int vbo;
        public Shader shader;
        public Material material;
        

        public float yaw;
        public Cube(Vector3 pos, float size, Color4 color, Texture texture, Shader shader, Material material)
        {
            this.Pos = pos;
            this.size = size;
            this.shader = shader;
            this.texture = texture;
            this.color = color;
            this.material = material;
            // 총6개 면에 대하여, 한 면에 3개의 삼각형의 좌표, 텍스처의 적용 범위 지정
            Vertices = new float[]{

                // Front (-Z)
                -size, -size, -size,  0.0f, 0.0f,   0f, 0f, -1f,
                 size, -size, -size,  1.0f, 0.0f,   0f, 0f, -1f,
                 size,  size, -size,  1.0f, 1.0f,   0f, 0f, -1f,
                 size,  size, -size,  1.0f, 1.0f,   0f, 0f, -1f,
                -size,  size, -size,  0.0f, 1.0f,   0f, 0f, -1f,
                -size, -size, -size,  0.0f, 0.0f,   0f, 0f, -1f,
                
                // Back (+Z)
                -size, -size,  size,  0.0f, 0.0f,   0f, 0f, 1f,
                 size, -size,  size,  1.0f, 0.0f,   0f, 0f, 1f,
                 size,  size,  size,  1.0f, 1.0f,   0f, 0f, 1f,
                 size,  size,  size,  1.0f, 1.0f,   0f, 0f, 1f,
                -size,  size,  size,  0.0f, 1.0f,   0f, 0f, 1f,
                -size, -size,  size,  0.0f, 0.0f,   0f, 0f, 1f,
                
                // Left (-X)
                -size,  size,  size,  1.0f, 0.0f,  -1f, 0f, 0f,
                -size,  size, -size,  1.0f, 1.0f,  -1f, 0f, 0f,
                -size, -size, -size,  0.0f, 1.0f,  -1f, 0f, 0f,
                -size, -size, -size,  0.0f, 1.0f,  -1f, 0f, 0f,
                -size, -size,  size,  0.0f, 0.0f,  -1f, 0f, 0f,
                -size,  size,  size,  1.0f, 0.0f,  -1f, 0f, 0f,
                
                // Right (+X)
                 size,  size,  size,  1.0f, 0.0f,   1f, 0f, 0f,
                 size,  size, -size,  1.0f, 1.0f,   1f, 0f, 0f,
                 size, -size, -size,  0.0f, 1.0f,   1f, 0f, 0f,
                 size, -size, -size,  0.0f, 1.0f,   1f, 0f, 0f,
                 size, -size,  size,  0.0f, 0.0f,   1f, 0f, 0f,
                 size,  size,  size,  1.0f, 0.0f,   1f, 0f, 0f,
                
                // Bottom (-Y)
                -size, -size, -size,  0.0f, 1.0f,   0f,-1f, 0f,
                 size, -size, -size,  1.0f, 1.0f,   0f,-1f, 0f,
                 size, -size,  size,  1.0f, 0.0f,   0f,-1f, 0f,
                 size, -size,  size,  1.0f, 0.0f,   0f,-1f, 0f,
                -size, -size,  size,  0.0f, 0.0f,   0f,-1f, 0f,
                -size, -size, -size,  0.0f, 1.0f,   0f,-1f, 0f,
                
                // Top (+Y)
                -size,  size, -size,  0.0f, 1.0f,   0f, 1f, 0f,
                 size,  size, -size,  1.0f, 1.0f,   0f, 1f, 0f,
                 size,  size,  size,  1.0f, 0.0f,   0f, 1f, 0f,
                 size,  size,  size,  1.0f, 0.0f,   0f, 1f, 0f,
                -size,  size,  size,  0.0f, 0.0f,   0f, 1f, 0f,
                -size,  size, -size,  0.0f, 1.0f,   0f, 1f, 0f
                };

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: Vertices.Length * sizeof(float),
                data: Vertices,
                usage: BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aPosition"));
            GL.VertexAttribPointer(
                index: shader.GetAttribLocation("aPosition"),
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 8 * sizeof(float),
                offset: 0);
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aTexCoord"));
            GL.VertexAttribPointer(
                index: shader.GetAttribLocation("aTexCoord"),
                size: 2,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 8 * sizeof(float),
                offset: 3 * sizeof(float));
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aNormal"));
            GL.VertexAttribPointer(
                index: shader.GetAttribLocation("aNormal"),
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 8 * sizeof(float),
                offset: 5 * sizeof(float));
        }
        public virtual void SetViewModel(Matrix4 camView, Matrix4 projection)
        {
            shader.SetMatrix4("view", camView);
            shader.SetMatrix4("projection", projection);
            Matrix4 model = Matrix4.CreateRotationY(yaw) * Matrix4.CreateTranslation(Pos);
            Matrix4 normalModel = Matrix4.Transpose(Matrix4.Invert(model));
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("normalModel", normalModel);
        }
        public virtual void SetLight(Light light, Vector3 viewPos)
        {
            shader.SetVector3("cameraPos", viewPos);

            shader.SetVector3("light.position", light.position);
            shader.SetVector4("light.ambient", (Vector4)light.ambient);
            shader.SetVector4("light.diffuse", (Vector4)light.diffuse);
            shader.SetVector4("light.specular", (Vector4)light.specular);

            shader.SetFloat("material.shininess", material.shininess);
            shader.SetVector4("material.ambient", (Vector4)material.ambient);
            shader.SetVector4("material.diffuse", (Vector4)material.diffuse);
            shader.SetVector4("material.specular", (Vector4)material.specular);


        }
        public void UseTexture()
        {
            if (texture != null)
            texture.Use(TextureUnit.Texture0);
        }
        public void BindArray()
        {
            GL.BindVertexArray(vao);
        }
    }

    public class LightingCube : Cube
    {
        public LightingCube(Vector3 pos, float size, Color4 color, Texture texture, Shader shader, Material material) : base(pos, size, color, texture, shader, material) { }

        public override void SetViewModel(Matrix4 camView, Matrix4 projection)
        {
            shader.SetMatrix4("view", camView);
            shader.SetMatrix4("projection", projection);
            Matrix4 model = Matrix4.CreateRotationY(yaw) * Matrix4.CreateTranslation(Pos);
            shader.SetMatrix4("model", model);
        }

        public override void SetLight(Light light, Vector3 viewPos)
        {
            shader.SetVector4("objectColor", (Vector4)color);
        }
    }
}
