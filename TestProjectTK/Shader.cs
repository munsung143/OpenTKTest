using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestProjectTK
{
    public class Shader
    {
        int Handle; //최종적으로 컴파일된 셰이더 프로그램의 위치를 나타냄 

        public Shader(string vertexPath, string fragmentPath)
        {
            // 셰이더 소스코드 읽어들여 문자열에 저장하는 과정
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);

            // 각 셰이더를 생성하고 해당 셰이더에 접근 가능한 int값 지정. (핸들)
            // 위에서 가져온 소스코드를 셰이더에 바인드 시켜준다.
            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            //각 셰이더를 컴파일 해주고, 성공 여부를 확인한다 (실패 시 로그 발생)
            GL.CompileShader(VertexShader);
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }
            GL.CompileShader(FragmentShader);
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success2);
            if (success2 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            // 각 셰이더들이 컴파일 되었지만, 실사용을 위해 GPU에서 돌아가는 프로그램에
            // 컴파일된 셰이더들을 링크해주어야 한다.
            // 셰이더 정의 : 각 파이프라인 스텝에 GPU에서 돌아가는 작은 프로그램
            // 생성, 붙이기, 링크, 가져오기 과정 후 성공여부 판정
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);
            GL.LinkProgram(Handle);
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success3);
            if (success3 == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            // 프로그램에 셰이더가 링크된 이후 각 셰이더는 무쓸모함.
            // 프로그램에서 지우고, 삭제
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader); 
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        // 프로그램 종료 시에 호출. 해당 Shader클래스 삭제 시 Handle의 프로그램이 자동으로 지워지지 않기 때문에
        // 이걸 수동으로 지우기 위함.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
