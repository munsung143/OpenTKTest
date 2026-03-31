using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestProjectTK
{
    public class Game : GameWindow
    {
        // 사각형의 NDC 좌표
        float[] vertices = {
         0.5f,  0.5f, 0.0f,  // top right
         0.5f, -0.5f, 0.0f,  // bottom right
        -0.5f, -0.5f, 0.0f,  // bottom left
        -0.5f,  0.5f, 0.0f   // top left
        };
        uint[] indices = {  // note that we start from 0!
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
        };

        // OpenGL의 오브젝트에 대한 핸들. 해당 오브젝트가 GPU의 어디에 존재하는지를
        // 나타내는 용도의 정수 값. 마치 포인터처럼 이것을 이용해 오브젝트에 접근
        // 오브젝트를 만들 때 값 할당받음
        int VertexBufferObject;
        int VertexArrayObject;

        int ElementBufferObject;

        Shader shader;
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
        // 화면 열릴 때 가장 처음, 단 한번 호출
        protected override void OnLoad()
        {
            base.OnLoad();

            // 화면 색상 결정
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // 셰이더 프로그램 생성용 객체 생성
            shader = new Shader("C:\\Git\\OpenTKTest\\TestProjectTK\\shader.vert", "C:\\Git\\OpenTKTest\\TestProjectTK\\shader.frag");

            // VBO생성 및 아이디 할당
            VertexBufferObject = GL.GenBuffer();
            // VBO 바인딩
            // OpenGL의 전역 상태를 가진다. 아래 코드 호출 이후로
            // VBO를 수정하는 코드는 모두 바인딩된 VertexBufferObject VBO에 적용됨
            // 다른 VBO를 바인딩하기 전까지.
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // 버퍼에 실제 데이터 전달
            // 버퍼의 포멧은 다음과 같다.
            // v1X, v1Y, v1Z, v2X, v2Y, v2Z, v3X, v3Y, v3Z 각 float로 4바이트, 총 36바이트, Stride:12바이트
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // VAO 생성
            VertexArrayObject = GL.GenVertexArray();
            // VAO 바인딩 (VAO는 생성, 바인딩만 해도 충분함)
            GL.BindVertexArray(VertexArrayObject);

            // 정점 어트리뷰트의 포인터를 설정함. (해당 포인터는 VAO에 할당됨)
            // index : 어떤 위치의 어트리뷰트를 사용할 것인지 (VS에서 locatin = 0 로 어트리뷰트 구성)
            // size : 어트리뷰트의 크기 (vec3을 사용하므로 3)
            // type : 어트리뷰트의 자료형 타입 (셰이더의 vec은 float타입으로 구성됨)
            // normalized : 자료형이 float가 아닌 int, bool등 일 경우 정규화
            // stride : 정점 데이터는 3개의 float자료형 즉 12바이트 만큼 서로 떨어져 있음
            // offset : 위치 데이터가 버퍼의 시작점부터 얼마만큼 떨어져 있는지 (처음부터 시작하기 때문에 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            // 어트리뷰트의 위치(location)을 매개변수로
            GL.EnableVertexAttribArray(0);

            // EBO 생성
            ElementBufferObject = GL.GenBuffer();
            // 바인딩
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            // 값 할당

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader.Use();

            //Code goes here
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // 화면 클리어, 랜더링 시 처음 호출되는 함수
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //shader.Use();
            //GL.BindVertexArray(VertexArrayObject);

            // mode : 어떤 식으로 그릴 것인지
            // first : 정점 배열의 첫 위치
            // count : 그릴 정점의 개수
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            // EBO를 활용한 드로잉
            GL.DrawElements(PrimitiveType.Lines, indices.Length, DrawElementsType.UnsignedInt, 0);

            //Code goes here.
            // 더블 버퍼링 : openTK의 두 가지 드로잉 영역을 뒤바꿈 (이미 화면상에 렌더링된 영역 <-> 이제 렌더링 해야 할 영역)
            SwapBuffers();
        }

        // 윈도우 프레임버퍼 사이즈가 바뀔때 호출
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            // NDC(Nomalized device coordinate 정규화 좌표) 를 윈도우 화면의 좌표로 변환
            // 프레임버퍼의 사이즈를 업데이트 하여 오래된 프레임버퍼 사이즈를 사용하는 것을 피함
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            shader.Dispose();
        }
    }
}
