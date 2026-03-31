using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using TestProjectTK.Common;

namespace TestProjectTK
{

    public class Window3 : GameWindow
    {
        List<Cube> cubes;
        private int[] _vbos;
        private int[] _vaos;
        private Shader _shader;

        float projDeg = 45f;
        Matrix4 projection;

        // 마우스 관련
        Vector2 lastMousePos;
        bool mouseUnset = true;

        Camera cam;

        public Window3(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }
        protected override void OnLoad()
        {
            base.OnLoad();
            // 원근 투영 행렬 생성
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(projDeg), 800f / 600, 0.1f, 100.0f);

            // 큐브들을 생성 (위치, 크기 텍스쳐)
            cubes = new List<Cube>();
            cubes.Add(new Cube(new Vector3(0f, 3f, -10f), 0.5f, "Resources/container.png", "Resources/awesomeface.png"));
            cubes.Add(new Cube(new Vector3(0f, -3f, -15f), 0.5f, "Resources/container.png", "Resources/awesomeface.png"));
            cubes.Add(new Cube(new Vector3(0f, 0f, -1f), 0.1f, "Resources/kaede.png", "Resources/kaede.png"));
            cubes.Add(new Cube(new Vector3(-30f, 20f, -60f), 20f, "Resources/container.png", "Resources/container.png"));
            CursorState = CursorState.Grabbed;
            cam = new Camera();
            // 큐브의 수만큼 vao, vbo 생성 및 초기화
            _vaos = new int[cubes.Count];
            _vbos = new int[cubes.Count];
            for (int i = 0; i < cubes.Count; i++)
            {
                _vaos[i] = GL.GenVertexArray();
                _vbos[i] = GL.GenBuffer();
            }
            _shader = new Shader("Shaders/rectshader.vert", "Shaders/rectshader.frag");

            // 프레그먼트 셰이더에서
            //uniform sampler2D texture0;
            //uniform sampler2D texture1;
            _shader.SetInt("texture0", 0);
            _shader.SetInt("texture1", 1);

            // 버텍스 셰이더에서
            //layout(location = 0) in vec3 aPosition;
            //layout(location = 1) in vec2 aTexCoord;
            int loc_position = _shader.GetAttribLocation("aPosition");
            int loc_texCoord = _shader.GetAttribLocation("aTexCoord");

            // 깊이 버퍼(Z buffer) 사용 활성화
            GL.Enable(EnableCap.DepthTest);

            //화면 색상 지정
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // 순서 : (셰이더 활성화) - VAO바인딩 - VBO바인딩 - (EBO바인딩) - VAO어트리뷰트 링크
            _shader.Use();
            for (int i = 0; i < cubes.Count; i++)
            {
                GL.BindVertexArray(_vaos[i]);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbos[i]);
                // 정보 전달
                GL.BufferData(
                    target: BufferTarget.ArrayBuffer,
                    size: cubes[i].Vertices.Length * sizeof(float),
                    data: cubes[i].Vertices,
                    usage: BufferUsageHint.StaticDraw);
                // 정보를 어떤 식으로 쓸건지 (정점 또는 텍스쳐 좌표)
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
            _shader.SetMatrix4("view", cam.view);
            _shader.SetMatrix4("projection", projection);

            for (int i = 0; i < cubes.Count; i++)
            {
                Matrix4 trans = Matrix4.CreateTranslation(cubes[i].Pos);
                _shader.SetMatrix4("transform", trans);
                cubes[i].UseTexture();
                // vao 활성화 후 그리기
                GL.BindVertexArray(_vaos[i]);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
            cam.OnFrame(KeyboardState, (float)args.Time);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseUnset)
            {
                lastMousePos = new Vector2(e.X, e.Y);
                mouseUnset = false;
                return;
            }
            float mouseDeltaX = e.X - lastMousePos.X;
            float mouseDeltaY = e.Y - lastMousePos.Y;
            lastMousePos = new Vector2(e.X, e.Y);
            cam.OnMouse(mouseDeltaX, mouseDeltaY);

        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
