using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace TK_Texture
{

    public class Window2 : GameWindow
    {
        List<Cube> cubes;
        private int[] _vbos;
        private int[] _vaos;
        private Shader _shader;

        float projDeg = 45f;
        Matrix4 projection;

        float moveX;
        float moveY;
        float moveZ;
        float rotateY;
        float rotateX;

        public Window2(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }
        protected override void OnLoad()
        {
            base.OnLoad();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(projDeg), 800f / 600, 0.1f, 100.0f);

            cubes = new List<Cube>();
            cubes.Add(new Cube(new Vector3(0f, 3f, -10f), 0.5f, "Resources/container.png", "Resources/awesomeface.png"));
            cubes.Add(new Cube(new Vector3(0f, -3f, -15f), 0.5f, "Resources/container.png", "Resources/awesomeface.png"));
            cubes.Add(new Cube(new Vector3(0f, 0f, -1f), 0.1f, "Resources/kaede.png", "Resources/kaede.png"));
            cubes.Add(new Cube(new Vector3(-30f, 20f, -60f), 20f, "Resources/container.png", "Resources/container.png"));

            _vaos = new int[cubes.Count];
            _vbos = new int[cubes.Count];
            for (int i = 0; i < cubes.Count; i++)
            {
                _vaos[i] = GL.GenVertexArray();
                _vbos[i] = GL.GenBuffer();
            }
            _shader = new Shader("Shaders/rectshader.vert", "Shaders/rectshader.frag");

            _shader.SetInt("texture0", 0);
            _shader.SetInt("texture1", 1);

            int loc_position = _shader.GetAttribLocation("aPosition");
            int loc_texCoord = _shader.GetAttribLocation("aTexCoord");

            // 깊이 버퍼(Z buffer) 사용 활성화
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // 순서 : (셰이더 활성화) - VAO바인딩 - VBO바인딩 - (EBO바인딩) - VAO어트리뷰트 링크
            _shader.Use();
            for (int i = 0; i < cubes.Count; i++)
            {
                GL.BindVertexArray(_vaos[i]);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbos[i]);
                GL.BufferData(
                    target: BufferTarget.ArrayBuffer,
                    size: cubes[i].Vertices.Length * sizeof(float),
                    data: cubes[i].Vertices,
                    usage: BufferUsageHint.StaticDraw);
                GL.EnableVertexAttribArray(loc_position);
                GL.VertexAttribPointer(
                    index: loc_position,
                    size: 3,
                    type: VertexAttribPointerType.Float,
                    normalized: false,
                    stride: 5 * sizeof(float),
                    offset: 0);
                GL.EnableVertexAttribArray(loc_texCoord);
                GL.VertexAttribPointer(
                    index: loc_texCoord,
                    size: 2,
                    type: VertexAttribPointerType.Float,
                    normalized: false,
                    stride: 5 * sizeof(float),
                    offset: 3 * sizeof(float));
            }
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 move = Matrix4.CreateTranslation(moveX, moveY, moveZ);
            Matrix4 rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotateY));
            Matrix4 rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotateX));
            Matrix4 view = move * rotationY * rotationX;

            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);

            for (int i = 0; i < cubes.Count; i++)
            {
                Matrix4 trans = Matrix4.CreateTranslation(cubes[i].Pos);
                _shader.SetMatrix4("transform", trans);
                cubes[i].UseTexture();
                GL.BindVertexArray(_vaos[i]);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            SwapBuffers();
        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            float size = 0.001f;
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                moveZ += (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
                moveX += (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                moveZ -= (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
                moveX += (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                moveZ -= (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
                moveX -= (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                moveZ += (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
                moveX -= (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift)) moveY += size;
            if (KeyboardState.IsKeyDown(Keys.Space)) moveY -= size;
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                rotateY -= size * 4;
            }
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                rotateY += size * 4;
            }
            if (KeyboardState.IsKeyDown(Keys.Up)) rotateX -= size * 4;
            if (KeyboardState.IsKeyDown(Keys.Down)) rotateX += size * 4;
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }

    public class Cube
    {
        public Vector3 Pos;
        private float size;
        public float[] Vertices;
        public Texture[] Texture;
        public Cube(Vector3 pos, float size, string tex0, string tex1)
        {
            this.Pos = pos;
            this.size = size;
            Texture = new Texture[2];
            Texture[0] = TK_Texture.Texture.LoadFromFile(tex0);
            Texture[1] = TK_Texture.Texture.LoadFromFile(tex1);
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
            Texture[0].Use(TextureUnit.Texture0);
            Texture[1].Use(TextureUnit.Texture1);
        }
    }
}
