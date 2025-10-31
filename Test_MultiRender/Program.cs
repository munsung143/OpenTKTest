//Install - Package OpenTK
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace TK_Texture
{
    public static class Program
    {
        public static void Main()
        {
            Vector2i size = new Vector2i(800, 600);
            string title = "Texture Practice";
            
            NativeWindowSettings setting = new NativeWindowSettings()
            {
                ClientSize = size,
                Title = title,
                Flags = ContextFlags.ForwardCompatible
            };
            
            using (var window = new Window(GameWindowSettings.Default, setting))
            {
                window.Run();
            }
        }
    }
}