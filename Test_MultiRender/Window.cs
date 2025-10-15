using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace TK_Texture
{

    public class Window : GameWindow
    {
        private readonly float[] _rectangle =
        {
            // Position         Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };
        private readonly uint[] _rectIndices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private readonly float[] _rainbow =
  {
      // positions        // colors
         0.0f, 0.0f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
        -1.0f, 0.0f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
         -0.5f,  1.0f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
        };

        private readonly float[] _triangle =
{
      // positions
         0.0f, 0.0f, 0.0f, // bottom right
        1.0f, 0.0f, 0.0f, // bottom left
         0.5f,  -1.0f, 0.0f, // top 
        };

        private int count = 3;

        private int _ebo;
        private int[] _vbo;
        private int[] _vao;
        private Shader[] _shader;
        private Texture _texture;
        private Texture _texture2;
        Stopwatch timer;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }
        protected override void OnLoad()
        {
            base.OnLoad();

            _vbo = new int[count];
            _vao = new int[count];
            _shader = new Shader[count];

            timer = new Stopwatch();
            timer.Start();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _shader[0] = new Shader("Shaders/rectshader.vert", "Shaders/rectshader.frag");
            _shader[1] = new Shader("Shaders/rainbowshader.vert", "Shaders/rainbowshader.frag");
            _shader[2] = new Shader("Shaders/triangleshader.vert", "Shaders/triangleshader.frag");
            _shader[0].SetInt("texture0", 0);
            _shader[0].SetInt("texture1", 1);

            _texture = Texture.LoadFromFile("Resources/container.png");
            _texture2 = Texture.LoadFromFile("Resources/awesomeface.png");


            // 순서 : VAO생성 및 바인딩 - (셰이더 활성화) - VBO생성 및 바인딩 - (EBO생성 및 바인딩) - VAO어트리뷰트 링크
            // 사각형
            _vao[0] = GL.GenVertexArray();
            _shader[0].Use();
            GL.BindVertexArray(_vao[0]);
            _vbo[0] = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, _rectangle.Length * sizeof(float), _rectangle, BufferUsageHint.StaticDraw);
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _rectIndices.Length * sizeof(uint), _rectIndices, BufferUsageHint.StaticDraw);
            int vertextLocation = _shader[0].GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertextLocation);
            GL.VertexAttribPointer(
                index: vertextLocation,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 5 * sizeof(float),
                offset: 0);
            int texCoordLocation = GL.GetAttribLocation(_shader[0].Handle, "aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(
                index: texCoordLocation,
                size: 2,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 5 * sizeof(float),
                offset: 3 * sizeof(float));

            // 무지개 삼각형
            _vao[1] = GL.GenVertexArray();
            _shader[1].Use();
            GL.BindVertexArray(_vao[1]);
            _vbo[1] = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, _rainbow.Length * sizeof(float), _rainbow, BufferUsageHint.StaticDraw);
            int vertextLocation2 = _shader[1].GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertextLocation2);
            GL.VertexAttribPointer(
                index: vertextLocation2,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 6 * sizeof(float),
                offset: 0);
            int location = _shader[1].GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(
                index: location,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 6 * sizeof(float),
                offset: 3 * sizeof(float));

            //일반삼각형
            _vao[2] = GL.GenVertexArray();
            _shader[2].Use();
            GL.BindVertexArray(_vao[2]);
            _vbo[2] = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, _triangle.Length * sizeof(float), _triangle, BufferUsageHint.StaticDraw);
            int location2 = _shader[2].GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(location2);
            GL.VertexAttribPointer(
                index: location2,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 3 * sizeof(float),
                offset: 0);

            _shader[0].Use();
            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);
            GL.BindVertexArray(_vao[0]);



        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader[0].Use();
            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);
            GL.BindVertexArray(_vao[0]);
            GL.DrawElements(PrimitiveType.Triangles, _rectIndices.Length, DrawElementsType.UnsignedInt, 0);

            _shader[1].Use();
            GL.BindVertexArray(_vao[1]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            double timeValue = timer.Elapsed.TotalSeconds;
            float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
            _shader[2].SetVector4("ourColor", new Vector4(0.0f, greenValue, 0.0f, 1.0f));
            _shader[2].Use();
            GL.BindVertexArray(_vao[2]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

    }
}
