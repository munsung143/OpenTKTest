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
            //Vector2i size = new Vector2i(800, 600);
            //string title = "Texture Practice";
            //
            //NativeWindowSettings setting = new NativeWindowSettings()
            //{
            //    ClientSize = size,
            //    Title = title,
            //    Flags = ContextFlags.ForwardCompatible
            //};
            //
            //using (var window = new Window(GameWindowSettings.Default, setting))
            //{
            //    window.Run();
            //}
            var m1 = new Measurement(5);
            Console.WriteLine(m1);  // output: 5 (Ordinary measurement)

            var m2 = new Measurement();
            Console.WriteLine(m2);  // output: 0 ()

            var m3 = default(Measurement);
            Console.WriteLine(m3);  // output: 0 ()

            var m4 = new Measurement("aaa");
            Console.WriteLine(m4);
        }
        public readonly struct Measurement
        {

            public Measurement(double value)
            {
                Value = value;
            }

            public Measurement(double value, string description)
            {
                Value = value;
                Description = description;
            }

            public Measurement(string description)
            {
                Description = description;
            }

            public double Value { get; init; } = 4;
            public string Description { get; init; } = "Ordinary measurement";

            public override string ToString() => $"{Value} ({Description})";
        }
    }
}