
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
        public Texture[] texture;
        public Cube(Vector3 pos, float size, string tex0, string tex1)
        {
            this.Pos = pos;
            this.size = size;
            texture = new Texture[2];
            texture[0] = Texture.LoadFromFile(tex0);
            texture[1] = Texture.LoadFromFile(tex1);
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
        }
        public void UseTexture()
        {
            texture[0].Use(TextureUnit.Texture0);
            texture[1].Use(TextureUnit.Texture1);
        }
    }
}
