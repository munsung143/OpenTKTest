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
        private Shader _shader;
        private List<Shader> shaders;
        private List<Texture> textures;

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
            // 기본 설정
            base.OnLoad();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(projDeg), 800f / 600, 0.1f, 100.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            CursorState = CursorState.Grabbed;
            cubes = new List<Cube>();
            cam = new Camera();
            shaders = new List<Shader>();
            textures = new List<Texture>();

            //텍스쳐
            textures.Add(Texture.LoadFromFile("Resources/awesomeface.png"));
            textures.Add(Texture.LoadFromFile("Resources/container.png"));
            textures.Add(Texture.LoadFromFile("Resources/kaede.png"));

            // 셰이더
            _shader = new Shader("Shaders/rectshader.vert", "Shaders/SingleTexture.frag");
            _shader.SetInt("texture0", 0);
            shaders.Add(_shader);

            // 큐브 생성
            cubes.Add(new Cube(new Vector3(0f, 3f, -10f), 0.5f, textures[0], _shader));
            cubes.Add(new Cube(new Vector3(0f, -3f, -15f), 0.5f, textures[1], _shader));
            cubes.Add(new Cube(new Vector3(0f, 0f, -1f), 0.1f, textures[2], _shader));
            cubes.Add(new Cube(new Vector3(-30f, 20f, -60f), 20f, textures[1], _shader));

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].SetViewModel(cam.view, projection);
                cubes[i].UseTexture();
                cubes[i].BindArray();
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
