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
        private Texture[] _texture;
        Stopwatch timer;

        double timescale = 1;
        double total = 0;
        double timeValue = 0;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }
        protected override void OnLoad()
        {
            base.OnLoad();

            _vbo = new int[count];
            _vao = new int[count];
            _shader = new Shader[count];
            _texture = new Texture[2];
            for (int i = 0; i < count; i++)
            {
                _vao[i] = GL.GenVertexArray();
                _vbo[i] = GL.GenBuffer();
            }
            _ebo = GL.GenBuffer();
            _shader[0] = new Shader("Shaders/rectshader.vert", "Shaders/rectshader.frag");
            _shader[1] = new Shader("Shaders/rainbowshader.vert", "Shaders/rainbowshader.frag");
            _shader[2] = new Shader("Shaders/triangleshader.vert", "Shaders/triangleshader.frag");

            _shader[0].SetInt("texture0", 0);
            _shader[0].SetInt("texture1", 1);

            int rect_loc_position = _shader[0].GetAttribLocation("aPosition");
            int rect_loc_texCoord = _shader[0].GetAttribLocation("aTexCoord");
            int rainbow_loc_position = _shader[1].GetAttribLocation("aPosition");
            int rainbow_loc_color = _shader[1].GetAttribLocation("aColor");
            int triangle_loc_position = _shader[2].GetAttribLocation("aPosition");

            _texture[0] = Texture.LoadFromFile("Resources/container.png");
            _texture[1] = Texture.LoadFromFile("Resources/awesomeface.png");

            timer = new Stopwatch();
            timer.Start();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // 순서 : (셰이더 활성화) - VAO바인딩 - VBO바인딩 - (EBO바인딩) - VAO어트리뷰트 링크
            // 사각형
            _shader[0].Use();
            GL.BindVertexArray(_vao[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[0]);
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: _rectangle.Length * sizeof(float),
                data: _rectangle,
                usage: BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(
                target: BufferTarget.ElementArrayBuffer,
                size: _rectIndices.Length * sizeof(uint),
                data: _rectIndices,
                usage: BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(rect_loc_position);
            GL.VertexAttribPointer(
                index: rect_loc_position,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 5 * sizeof(float),
                offset: 0);
            GL.EnableVertexAttribArray(rect_loc_texCoord);
            GL.VertexAttribPointer(
                index: rect_loc_texCoord,
                size: 2,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 5 * sizeof(float),
                offset: 3 * sizeof(float));

            // 무지개 삼각형
            _shader[1].Use();
            GL.BindVertexArray(_vao[1]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[1]);
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: _rainbow.Length * sizeof(float),
                data: _rainbow,
                usage: BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(rainbow_loc_position);
            GL.VertexAttribPointer(
                index: rainbow_loc_position,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 6 * sizeof(float),
                offset: 0);
            GL.EnableVertexAttribArray(rainbow_loc_color);
            GL.VertexAttribPointer(
                index: rainbow_loc_color,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 6 * sizeof(float),
                offset: 3 * sizeof(float));

            //일반삼각형
            _shader[2].Use();
            GL.BindVertexArray(_vao[2]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[2]);
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: _triangle.Length * sizeof(float),
                data: _triangle,
                usage: BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(triangle_loc_position);
            GL.VertexAttribPointer(
                index: triangle_loc_position,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 3 * sizeof(float),
                offset: 0);

            _shader[0].Use();
            _texture[0].Use(TextureUnit.Texture0);
            _texture[1].Use(TextureUnit.Texture1);
            GL.BindVertexArray(_vao[0]);



        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            double delta = timer.Elapsed.TotalSeconds - total;
            total = timer.Elapsed.TotalSeconds;
            timeValue += delta * timescale;
            float rotateValue = (float)timeValue * 200;
            float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
            float scaleValue = (float)Math.Sin((timeValue - Math.PI/2) ) / 2.0f + 0.5f;
            float moveValue = (float)Math.Sin(timeValue/2);

            // 행렬을 통한 회전, 크기변환
            // 계산한 행렬을 유니폼 변수로 옮긴 후 정점 셰이더에 반영하기
            Matrix4 rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotateValue));
            Matrix4 scale = Matrix4.CreateScale(scaleValue, scaleValue, scaleValue);
            Matrix4 move = Matrix4.CreateTranslation(0, moveValue, 0);
            Matrix4 trans = rotation * scale * move;

            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800f / 600, 0.1f, 100.0f);

            _shader[0].SetMatrix4("transform", trans);
            _shader[0].SetMatrix4("view", view);
            _shader[0].SetMatrix4("projection", projection);
            _texture[0].Use(TextureUnit.Texture0);
            _texture[1].Use(TextureUnit.Texture1);
            GL.BindVertexArray(_vao[0]);
            GL.DrawElements(PrimitiveType.Triangles, _rectIndices.Length, DrawElementsType.UnsignedInt, 0);

            _shader[1].Use();
            GL.BindVertexArray(_vao[1]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            _shader[2].SetVector4("ourColor", new Vector4(0.0f, greenValue, 0.0f, 1.0f));
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
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                timescale += 0.001f;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"speed: {timescale}       ");
            }
            else if (KeyboardState.IsKeyDown(Keys.S))
            {
                timescale -= 0.001f;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"speed: {timescale}      ");
            }
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

    }
}
