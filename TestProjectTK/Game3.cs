using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Timers;

namespace TestProjectTK
{
    public class Game3 : GameWindow
    {
        private readonly float[] vertices =
          {
      // positions        // colors
         0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
        -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
         0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
        };

        private readonly float[] vertices2 =
  {
      // positions        // colors
         0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom right
        -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,   // bottom left
         0.0f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f    // top 
        };
        int VertexBufferObject;
        int VertexArrayObject;

        Shader shader;
        public Game3(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (KeyboardState.IsKeyPressed(Keys.A))
            {
                GL.BufferData(BufferTarget.ArrayBuffer, vertices2.Length * sizeof(float), vertices2, BufferUsageHint.StaticDraw);
            }
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(
                index: 0,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 6 * sizeof(float),
                offset : 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(
                index: 1,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 6 * sizeof(float),
                offset: 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            shader = new Shader("C:\\Git\\OpenTKTest\\TestProjectTK\\shader2.vert", "C:\\Git\\OpenTKTest\\TestProjectTK\\shader2.frag");
            shader.Use();

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            shader.Dispose();
        }
    }
}
