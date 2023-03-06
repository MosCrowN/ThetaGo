using SFML.Audio;
using SFML.Graphics;

namespace ConsoleApp1;

internal static class Params
{
    private const int MaxMusicVolume = 100;
    private static int _musicVolume = 100;

    public static int MusicVolume
    {
        set => _musicVolume = (int)(MaxMusicVolume / 100f * value);
        get => _musicVolume;
    }

    private const int MaxDeskSize = 35;
    private static  int _deskSize = 19;
    
    public static int DeskSize 
    {
        set => _deskSize = (int)(MaxDeskSize / 100f * value);
        get => _deskSize;
    }

    public static readonly Font Albert = new("font_albert.ttf");

    public static readonly Music Ost = new ("main_theme.wav");

    public static readonly Texture MenuBg = new ("bg_main_menu.jpg");

    public static readonly Texture DeskBg = new("wood.jpeg");

    public static readonly Texture MenuButton = new ("button.png");

    public static readonly Texture WhiteStone = new ("white.png");
    
    public static readonly Texture BlackStone = new ("black.png");
}

internal static class Start
{
    private static void Main()
    {
        Params.Ost.Loop = true;
        Window window = new MainWindow();
        while (true)
        {
            if (window.Select == 1 && window is MainWindow)
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
