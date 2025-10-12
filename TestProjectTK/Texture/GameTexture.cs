using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestProjectTK
{
    public class GameTexture : GameWindow
    {
        // 사각형의 NDC 좌표
        float[] vertices =
        {
    //Position          Texture coordinates
         0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
         0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };
        uint[] indices = {  // note that we start from 0!
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
        };
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;

        Shader shader;
        Texture texture;
        public GameTexture(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
        }
        // 화면 열릴 때 가장 처음, 단 한번 호출
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: vertices.Length * sizeof(float),
                data: vertices,
                usage: BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(
                target: BufferTarget.ElementArrayBuffer,
                size: indices.Length * sizeof(float),
                data: indices,
                usage: BufferUsageHint.StaticDraw);

            shader = new Shader(
                vertexPath: "C:\\Git\\OpenTKTest\\TestProjectTK\\Texture\\shader.vert",
                fragmentPath: "C:\\Git\\OpenTKTest\\TestProjectTK\\Texture\\shader.frag");
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(
                index: 0,
                size: 3,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 5 * sizeof(float),
                offset: 0);

            //int texCoordLocation = GL.GetAttribLocation(shader.Handle, "aTexCoord");
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(
                index: 1,
                size: 2,
                type: VertexAttribPointerType.Float,
                normalized: false,
                stride: 5 * sizeof(float),
                offset: 3 * sizeof(float));


            texture = new Texture("C:\\Git\\OpenTKTest\\TestProjectTK\\Texture\\container.png");
            //texture.Use();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(
                mode: PrimitiveType.Triangles,
                count: indices.Length,
                type: DrawElementsType.UnsignedInt,
                indices: 0);
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
