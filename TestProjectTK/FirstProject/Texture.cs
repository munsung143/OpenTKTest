using OpenTK.Graphics.OpenGL4;
using StbImageSharp; //NuGet\Install-Package StbImageSharp -Version 2.30.15
using System;
using System.Reflection.Metadata;
namespace TestProjectTK
{
    internal class Texture
    {
        public int Handle;

        string defPath = "C:/Git/OpenTKTest/TestProjectTK/";

        public Texture(string path)
        {
            // 사용할 빈 텍스처 생성
            Handle = GL.GenTexture();
            // 테스처 바인드
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            // 이미지를 좌하단(기본)이 아닌 좌상단부터 불러오도록 설정
            StbImage.stbi_set_flip_vertically_on_load(1);
            // 이미지 로드
            ImageResult image = ImageResult.FromStream(File.OpenRead(defPath + path), ColorComponents.RedGreenBlueAlpha);
            // 텍스처 업로드
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

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
            // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            // 밉맵 생성
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


        }
        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);

        }
        
    }
}
