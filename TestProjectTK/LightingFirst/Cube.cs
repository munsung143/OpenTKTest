
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
        public int vao;
        public int vbo;
        public Shader shader;
        public Cube(Vector3 pos, float size, Texture texture, Shader shader)
        {
            this.Pos = pos;
            this.size = size;
            this.shader = shader;
            this.texture = texture;
            // 총6개 면에 대하여, 한 면에 3개의 삼각형의 좌표, 텍스처의 적용 범위 지정
            Vertices = new float[]{
            -size, -size, -size,  0.0f, 0.0f,
             size, -size, -size,  1.0f, 0.0f,
             size,  size, -size,  1.0f, 1.0f,
             size,  size, -size,  1.0f, 1.0f,
            -size,  size, -size,  0.0f, 1.0f,
            -size, -size, -size,  0.0f, 0.0f,

            -size, -size,  size,  0.0f, 0.0f,
             size, -size,  size,  1.0f, 0.0f,
             size,  size,  size,  1.0f, 1.0f,
             size,  size,  size,  1.0f, 1.0f,
            -size,  size,  size,  0.0f, 1.0f,
            -size, -size,  size,  0.0f, 0.0f,

            -size,  size,  size,  1.0f, 0.0f,
            -size,  size, -size,  1.0f, 1.0f,
            -size, -size, -size,  0.0f, 1.0f,
            -size, -size, -size,  0.0f, 1.0f,
            -size, -size,  size,  0.0f, 0.0f,
            -size,  size,  size,  1.0f, 0.0f,

             size,  size,  size,  1.0f, 0.0f,
             size,  size, -size,  1.0f, 1.0f,
             size, -size, -size,  0.0f, 1.0f,
             size, -size, -size,  0.0f, 1.0f,
             size, -size,  size,  0.0f, 0.0f,
             size,  size,  size,  1.0f, 0.0f,

            -size, -size, -size,  0.0f, 1.0f,
             size, -size, -size,  1.0f, 1.0f,
             size, -size,  size,  1.0f, 0.0f,
             size, -size,  size,  1.0f, 0.0f,
            -size, -size,  size,  0.0f, 0.0f,
            -size, -size, -size,  0.0f, 1.0f,

            -size,  size, -size,  0.0f, 1.0f,
             size,  size, -size,  1.0f, 1.0f,
             size,  size,  size,  1.0f, 0.0f,
             size,  size,  size,  1.0f, 0.0f,
            -size,  size,  size,  0.0f, 0.0f,
            -size,  size, -size,  0.0f, 1.0f };

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
                stride: 5 * sizeof(float),
                offset: 0);
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aTexCoord"));
            GL.VertexAttribPointer(
                index: shader.GetAttribLocation("aTexCoord"),
                size: 2,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 5 * sizeof(float),
                offset: 3 * sizeof(float));
        }
        public void SetViewModel(Matrix4 camView, Matrix4 projection)
        {
            shader.SetMatrix4("view", camView);
            shader.SetMatrix4("projection", projection);
            shader.SetMatrix4("transform", Matrix4.CreateTranslation(Pos));
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
}
