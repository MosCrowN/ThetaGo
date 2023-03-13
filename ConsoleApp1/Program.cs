using SFML.Audio;
using SFML.Graphics;

namespace ConsoleApp1;

internal static class Params
{
    public static int MusicVolume = 100;

    public static int DeskSize = 19;

    public static readonly Font Albert = new("./fonts/font_albert.ttf");

    public static readonly Music Ost = new ("./songs/main_theme.wav");

    public static readonly Texture MenuBg = new ("./pictures/bg_main_menu.jpg");

    public static readonly Texture DeskBg = new("./pictures/wood.jpeg");

    public static readonly Texture MenuButton = new ("./pictures/button.png");

    public static readonly Texture WhiteStone = new ("./pictures/white.png");
    
    public static readonly Texture BlackStone = new ("./pictures/black.png");
}

internal static class Start
{
    private static void Main()
    {
        Params.Ost.Loop = true;
        Window window = new MainWindow();
        while (true)
        {
            if (window.Select == 0 && window is MainWindow)
                window = new GameWindow("board.soap");
            else if (window.Select == 1 && window is MainWindow)
                window = new GameWindow();
            else if (window.Select == 2 && window is MainWindow)
                window = new SettingsWindow();
            else if (window.Select == 3 && window is MainWindow)
                return;
            window.Loop();
            if (window is not MainWindow) window = new MainWindow();
        }
    }
}
