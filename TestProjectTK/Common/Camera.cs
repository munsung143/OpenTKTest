using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestProjectTK.Common
{
    public class Camera
    {
        public Vector3 position = Vector3.Zero;
        Vector3 front = -Vector3.UnitZ;
        Vector3 up = Vector3.UnitY;

        float speed = 3;
        float mouseSensitivity = 0.3f;
        float yaw = -90;
        float pitch = 0;

        Vector3 right => Vector3.Normalize(Vector3.Cross(front, up));
        Vector3 target => position + front;
        public Matrix4 view => Matrix4.LookAt(position, target, up);

        public Camera()
        {
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);
        }
        public void OnFrame(KeyboardState input, float deltaTime)
        {
            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed * deltaTime; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed * deltaTime; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                position -= right * speed * deltaTime; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                position += right * speed * deltaTime; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position += up * speed * deltaTime; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * speed * deltaTime; //Down
            }
        }
        public void OnMouse(float mouseDelatX, float mouseDeltaY)
        {
            if (pitch > 89) pitch = 89;
            else if (pitch < -89) pitch = -89;
            else pitch -= mouseDeltaY * mouseSensitivity;
            yaw += mouseDelatX * mouseSensitivity;
            //Console.WriteLine(yaw);

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);
        }
    }
}
