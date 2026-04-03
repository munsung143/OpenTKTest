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
        Cube lightingCube;
        private Shader _shader;
        private List<Shader> shaders;
        private List<Texture> textures;

        float projDeg = 45f;
        Matrix4 projection;

        // 마우스 관련
        Vector2 lastMousePos;
        bool mouseUnset = true;
        Camera cam;

        Light light = new Light(
            new Color4(0.3f, 0.3f, 0.3f, 1.0f),
            new Color4(1.0f, 1.0f, 1.0f, 1.0f),
            new Color4(-1.0f, -1.0f, -1.0f, 1.0f),
            new Vector3(0.0f, 0.0f, 0.0f));

        public Window3(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }
        protected override void OnLoad()
        {
            // 기본 설정
            base.OnLoad();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(projDeg), 800f / 600, 0.1f, 100.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            CursorState = CursorState.Grabbed;
            cubes = new List<Cube>();
            cam = new Camera();
            shaders = new List<Shader>();
            textures = new List<Texture>();

            //텍스쳐
            textures.Add(Texture.LoadFromFile("Resources/awesomeface.png"));
            textures.Add(Texture.LoadFromFile("Resources/container.png"));
            textures.Add(Texture.LoadFromFile("Resources/kaede.png"));
            textures.Add(Texture.LoadFromFile("Resources/none.png"));

            // 셰이더
            _shader = new Shader("Shaders/rectshader.vert", "Shaders/SingleTexture.frag");
            _shader.SetInt("texture0", 0);
            shaders.Add(_shader);

            // 셰이더
            _shader = new Shader("Shaders/rectshader.vert", "Shaders/Lighting.frag");
            //_shader.SetInt("texture0", 0);
            shaders.Add(_shader);

            // 큐브 생성
            cubes.Add(new Cube(
                new Vector3(0f, 0f, -3f),
                1.0f,
                Color4.White,
                textures[3],
                shaders[0],
                new Material(
                    new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    new Color4(0.0f, -1.0f, 1.0f, 1.0f),
                    32)));
            //cubes.Add(new Cube(
            //    new Vector3(0f, -3f, -15f),
            //    0.5f, 
            //    Color4.White,
            //    textures[1],
            //    shaders[0]));
            //cubes.Add(new Cube(
            //    new Vector3(0f, 0f, -1f),
            //    0.1f,
            //    Color4.White,
            //    textures[2],
            //    shaders[0]));
            //cubes.Add(new Cube(
            //    new Vector3(-30f, 20f, -60f),
            //    20f,
            //    Color4.White,
            //    textures[1],
            //    shaders[0]));
            lightingCube = new LightingCube(light.position, 0.05f, light.diffuse, null, shaders[1], new Material());

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            for (int i = 0; i < cubes.Count; i++)
            {
                CubeRender(cubes[i]);
            }
            CubeRender(lightingCube);

            SwapBuffers();
        }
        private void CubeRender(Cube cube)
        {
            cube.SetViewModel(cam.view, projection);
            cube.SetLight(light, cam.position);
            cube.UseTexture();
            cube.BindArray();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
            if (KeyboardState.IsKeyPressed(Keys.Tab)) CursorState = CursorState == CursorState.Normal ? CursorState.Grabbed : CursorState.Normal;
            if (KeyboardState.IsKeyDown(Keys.Left)) light.position.X -= 0.0005f;
            if (KeyboardState.IsKeyDown(Keys.Right)) light.position.X += 0.0005f;
            if (KeyboardState.IsKeyDown(Keys.Up)) light.position.Y += 0.0005f;
            if (KeyboardState.IsKeyDown(Keys.Down)) light.position.Y -= 0.0005f;
            if (KeyboardState.IsKeyDown(Keys.N)) light.position.Z -= 0.0005f;
            if (KeyboardState.IsKeyDown(Keys.M)) light.position.Z += 0.0005f;
            cam.OnFrame(KeyboardState, (float)args.Time);
            lightingCube.Pos = light.position;
            for (int i = 0; i < cubes.Count; i++)
            {
                //cubes[i].yaw += 0.00005f;
            }
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

    public struct Material
    {
        public Color4 ambient;
        public Color4 diffuse;
        public Color4 specular;
        public float shininess;
        public Material(Color4 ambient, Color4 diffuse, Color4 specular, float shininess)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.shininess = shininess;
        }
    }
    public struct Light
    {
        public Color4 ambient;
        public Color4 diffuse;
        public Color4 specular;
        public Vector3 position;
        public Light(Color4 ambient, Color4 diffuse, Color4 specular, Vector3 position)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.position = position;
        }
    }
}
