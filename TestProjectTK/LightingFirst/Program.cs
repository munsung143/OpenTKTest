//Install - Package OpenTK
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace TestProjectTK
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
            using (var window = new Window3(GameWindowSettings.Default, setting))
            {
                window.Run();
            }

            //string? s = Console.ReadLine();
            //if (s == "1")
            //{
            //    using (var window = new Window(GameWindowSettings.Default, setting))
            //    {
            //        window.Run();
            //    }
            //}
            //else if (s == "2")
            //{
            //    using (var window = new Window2(GameWindowSettings.Default, setting))
            //    {
            //        window.Run();
            //    }
            //}
            //else if (s == "3")
            //{
            //    using (var window = new Window3(GameWindowSettings.Default, setting))
            //    {
            //        window.Run();
            //    }
            //}




        }
    }
}