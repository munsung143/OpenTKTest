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
        Vector3 cameraPos;

        public Window2(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
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


            // 실제 수학적 순서는 다음과 같음 :
            // 투영 행렬 * (카메라 이동 행렬 * 카메라 Y축기준 회전 행렬 * 카메라 X축기준 회전 행렬)(뷰) * 월드 기준 오브젝트의 위치 변환 행렬(월드) * 오브젝트 좌표 벡터
            // *왜 Y축 회전이 마지막인가 : Y축 회전을 월드 기준으로 고정, 즉 어딜 바라보든 Y축 회전축은 고정됨
            // *왜 이동 행렬을 회전 이후에 적용하는가 : 만약 이동을 먼저 할 경우 원점과 객체간 거리에 따라 같은 회전각도에서도 회전한 정도가 달라짐(멀수록 커짐) 즉 항상 일관된 회전 정도를 얻기 위함
            //Matrix4 move = Matrix4.CreateTranslation(moveX, moveY, moveZ);
            Matrix4 move = Matrix4.CreateTranslation(cameraPos);
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
                // vao 활성화 후 그리기
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
            // y축 회전 정도에 따른 x, z축 방향 동시 이동
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                cameraPos.Z += (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
                cameraPos.X += (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                cameraPos.Z -= (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
                cameraPos.X += (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                cameraPos.Z -= (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
                cameraPos.X -= (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                cameraPos.Z += (float)(size * Math.Sin(MathHelper.DegreesToRadians(-rotateY)));
                cameraPos.X -= (float)(size * Math.Cos(MathHelper.DegreesToRadians(-rotateY)));
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift)) cameraPos.Y += size;
            if (KeyboardState.IsKeyDown(Keys.Space)) cameraPos.Y -= size;
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                rotateY -= size * 4;
            }
            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                rotateY += size * 4;
            }
            if (KeyboardState.IsKeyDown(Keys.Up)) cameraPos.Y -= size * 4;
            if (KeyboardState.IsKeyDown(Keys.Down)) cameraPos.Y += size * 4;
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
            Texture[0].Use(TextureUnit.Texture0);
            Texture[1].Use(TextureUnit.Texture1);
        }
    }
}
