


using OpenTK.Windowing.Desktop;

namespace TestProjectTK
{
    public static class Program
    {
        public static void Main()
        {
            //using (Game3 game = new Game3(800, 600, "LearnOpenTK"))
            //{
            //    game.Run();
            //}
            
        }
        public static void Diamond(int a)
        {
            int m = a / 2;
            for (int i = 0; i < a; i++)
            {
                int c = i > m ? a - 1 - i : i;
                for (int j = m; j >= -m; j--)
                {
                    int abs = j < 0 ? -j : j;
                    if (abs > c) Console.Write(" ");
                    else Console.Write("*");
                }
                Console.WriteLine();
            }
        }
    }
}
