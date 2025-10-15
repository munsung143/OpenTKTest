using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;
using StbImageSharp;
using System.IO;


namespace TK_Texture
{
    public class Texture
    {
        public readonly int Handle;

        public static Texture LoadFromFile(string path)
        {
            // 텍스처 생성
            // Texture0유닛을 활성화 하고, (GPU내부의 텍스처 저장 공간)
            // 해당 유닛에 2D텍스처와 핸들을 바인드.
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle); 

            // 이미지를 좌하단(기본)이 아닌 좌상단부터 불러오도록 설정
            StbImage.stbi_set_flip_vertically_on_load(1);

            // 스트림을 이용해 경로에서 텍스처를 불러온 후 저장,
            // 결과를 통해 텍스처를 생성한다
            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(
                    target: TextureTarget.Texture2D, // 텍스처 타입, 1D 3D도 있음
                    level: 0, // 밉맵 디테일 레벨, 0은 기본(최대)
                    internalformat: PixelInternalFormat.Rgba, //OpenGL이 GPU에 픽셀을 저장하는 형식, RGBA로 저장함
                    width: image.Width, // 이미지 너비
                    height: image.Height, // 이미지 높이
                    border: 0, // 레거시, 항상 0 으로 설정하면됨
                    format: PixelFormat.Rgba, // 이미지의 바이트의 포멧, ImageSharp는 RGBA로 이미지를 불러오니, 이를 사용
                    type: PixelType.UnsignedByte, // 바이트는 부호 없는 정수로 불러와짐
                    pixels: image.Data); // 불러온 픽셀 배열
            }
            // 텍스처 랜더링 시 어떻게 보여질 것인지를 설정
            // 텍스처가 작아지고 커질때 어떻게 필터링 될 것인지,
            // 텍스처 좌표에 비해 랜더링 되어야 할 넓이가 넓을 경우, 가로 세로 빈공간을 어떻게 채울지
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            // 밉맵 생성
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // 바인딩된 핸들을 생성자로 보낸다.
            return new Texture(handle);
        }

        public Texture(int handle)
        {
            this.Handle = handle;
        }


        // 텍스처 활성화
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
