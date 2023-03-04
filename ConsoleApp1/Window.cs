using SFML.Graphics;
using SFML.Window;

namespace ConsoleApp1;

internal abstract class Window
{
    protected static readonly RenderWindow SfWindow = 
        new (new VideoMode(), "Go", Styles.Fullscreen);

    protected ButtonLayout? Layout;

    protected bool IsMousePressed;

    private bool _isOpen = true;

    protected bool IsOpen
    {
        set => _isOpen = value;
        get
        {
            if (_isOpen) return true;
            SfWindow.KeyReleased -= SfWindowOnKeyReleased;
            SfWindow.MouseMoved -= SfWindowOnMouseMoved;
            SfWindow.MouseButtonPressed -= SfWindowOnMouseButtonPressed;
            SfWindow.MouseButtonReleased -= SfWindowOnMouseButtonReleased;

            return false;
        }
    }

    public int Select { protected set; get; }
    
    protected Window()
    {
        Select = -1;

        //TODO: window icon and settings
        //SfWindow.SetIcon();
        SfWindow.SetVerticalSyncEnabled(true);
        SfWindow.SetFramerateLimit(60);

        SfWindow.Closed += SfWindowClosed;
        SfWindow.KeyReleased += SfWindowOnKeyReleased;
        SfWindow.MouseMoved += SfWindowOnMouseMoved;
        SfWindow.MouseButtonPressed += SfWindowOnMouseButtonPressed;
        SfWindow.MouseButtonReleased += SfWindowOnMouseButtonReleased;
    }

    public abstract void Loop();

    protected abstract void SfWindowOnMouseMoved(object? sender, MouseMoveEventArgs e);
    
    protected abstract void SfWindowOnMouseButtonPressed(object? sender, MouseButtonEventArgs e);

    protected abstract void SfWindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e);

    protected abstract void SfWindowOnKeyReleased(object? sender, KeyEventArgs e);
    
    private static void SfWindowClosed(object? sender, EventArgs e)
    {
        SfWindow.Close();
    }
}
