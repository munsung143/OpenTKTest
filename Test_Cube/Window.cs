using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace TK_Texture
{

    public class Window : GameWindow
    {
        float[] _cube = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
};

        private int _vbo;
        private int _vao;
        private Shader _shader;
        private Texture[] _texture;

        float rotateY;
        float rotateX;

        float projDeg = 45f;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }
        protected override void OnLoad()
        {
            base.OnLoad();

            _texture = new Texture[2];
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _shader = new Shader("Shaders/rectshader.vert", "Shaders/rectshader.frag");

            _shader.SetInt("texture0", 0);
            _shader.SetInt("texture1", 1);

            int loc_position = _shader.GetAttribLocation("aPosition");
            int loc_texCoord = _shader.GetAttribLocation("aTexCoord");

            _texture[0] = Texture.LoadFromFile("Resources/container.png");
            _texture[1] = Texture.LoadFromFile("Resources/awesomeface.png");

            // 깊이 버퍼(Z buffer) 사용 활성화
            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // 순서 : (셰이더 활성화) - VAO바인딩 - VBO바인딩 - (EBO바인딩) - VAO어트리뷰트 링크
            // 사각형
            _shader.Use();
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: _cube.Length * sizeof(float),
                data: _cube,
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

            _shader.Use();
            _texture[0].Use(TextureUnit.Texture0);
            _texture[1].Use(TextureUnit.Texture1);
            GL.BindVertexArray(_vao);



        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
           
            Matrix4 rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotateX));
            Matrix4 rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotateY));
            // Y축에 대한 회전이 절대 좌표가 아니라 종속된 로컬 좌표계에 대한 회전처럼 된다.
            // Y축에 대하여 돌린 후에 X축에 대하여 돌리기 때문이다.
            // Y회전값에 변화가 발생하여도 렌더링마다 처음에 정렬된 상태로 Y축 회전 후 X축에 대하여 돌리기 때문
            Matrix4 trans = rotationY * rotationX;

            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(projDeg), 800f / 600, 0.1f, 100.0f);

            _shader.SetMatrix4("transform", trans);
            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);
            _texture[0].Use(TextureUnit.Texture0);
            _texture[1].Use(TextureUnit.Texture1);
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

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
            if (KeyboardState.IsKeyDown(Keys.W)) rotateX -= 0.03f;
            if (KeyboardState.IsKeyDown(Keys.A)) rotateY -= 0.03f;
            if (KeyboardState.IsKeyDown(Keys.S)) rotateX += 0.03f;
            if (KeyboardState.IsKeyDown(Keys.D)) rotateY += 0.03f;
            if (KeyboardState.IsKeyDown(Keys.Up)) projDeg += 0.01f;
            if (KeyboardState.IsKeyDown(Keys.Down)) projDeg -= 0.01f;
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

    }
}
