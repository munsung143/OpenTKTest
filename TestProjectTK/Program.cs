


using OpenTK.Windowing.Desktop;

namespace TestProjectTK
{
    public static class Program
    {
        public static void Main()
        {
            using (Game game = new Game(800, 600, "LearnOpenTK"))
            {
                game.Run();
            }

        }
    }
}
