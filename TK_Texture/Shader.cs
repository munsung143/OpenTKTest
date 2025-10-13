using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace TK_Texture
{
    public class Shader
    {
        public readonly int Handle;
        private readonly Dictionary<string, int> _uniformLocations;
        private bool disposedValue = false;

        public Shader(string vertPath, string fragPath)
        {
            // 셰이더 텍스트를 불러오고,셰이더 생성 후 이것을 바인딩 및 컴파일, 컴파일 성공여부 확인 작업
            string vertString = File.ReadAllText(vertPath);
            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertString);
            GL.CompileShader(vertexShaderHandle);
            GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out int vert_result);
            if (vert_result == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShaderHandle);
                throw new Exception($"셰이더 컴파일 중 오류 발생({vertexShaderHandle}).\n\n{infoLog}");
            }

            string fragString = File.ReadAllText(fragPath);
            int fragShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShaderHandle, fragString);
            GL.CompileShader(fragShaderHandle);
            GL.GetShader(fragShaderHandle, ShaderParameter.CompileStatus, out int frag_result);
            if (frag_result == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragShaderHandle);
                throw new Exception($"셰이더 컴파일 중 오류 발생({fragShaderHandle}).\n\n{infoLog}");
            }

            // 셰이더 프로그램 생성 후 컴파일된 셰이더를 붙이고, 링크, 링크 여부 확인
            // 링크된 후 각자의 셰이더는 더 붙어있을 필요가 없어지기 때문에 떼어내고, 제거해준다.
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShaderHandle);
            GL.AttachShader(Handle, fragShaderHandle);
            GL.LinkProgram(Handle);
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out var program_result);
            if (program_result == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                throw new Exception($"프로그램 링크 중 오류 발생({Handle}).\n\n{infoLog}");
            }
            GL.DetachShader(Handle, vertexShaderHandle);
            GL.DetachShader(Handle, fragShaderHandle);
            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragShaderHandle);

            // 셰이더의 uniform 변수(전역접근 가능한 변수)의 위치를 가져오는 것은 매우 느림
            // 따라서 미리 가져온 후 딕셔너리에 저장하는 캐싱 과정을 거친다.
            // 우선 uniform의 개수를 구한 후, 순서대로 가져와서 저장.
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            _uniformLocations = new Dictionary<string, int>();
            for (int i = 0; i < numberOfUniforms; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }

        }
        // 셰이더 활성화 래퍼 함수
        public void Use()
        {
            GL.UseProgram(Handle);
        }
        // 어트리뷰트의 이름을 통해 위치(location)을 가져오는 래퍼 함수
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        // uniform 변수 세팅용 함수들
        // 설정하려는 셰이더 프로그램을 해당 클래스의 프로그램으로 하고,
        // 변수의 위치를 찾아서 값을 넣어준다.
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);  
        }

        // 프로그램 종료 시에 호출. 해당 Shader클래스 삭제 시 Handle의 프로그램이 자동으로 지워지지 않기 때문에
        // 이걸 수동으로 지우기 위함.
        public void Dispose()
        {
            DisposeProgram();
            GC.SuppressFinalize(this);
        }
        protected virtual void DisposeProgram()
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

    }
}
