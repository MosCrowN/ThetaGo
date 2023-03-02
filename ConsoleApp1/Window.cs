using SFML.Graphics;
using SFML.Window;

namespace ConsoleApp1;

internal abstract class Window
{
    protected RenderWindow? _window;

    protected Layout? _layout;

    public int Select { protected set; get; }
    
    protected Window()
    {
        Select = -1;
        
        _window = new RenderWindow(new VideoMode(), "Go", Styles.Fullscreen);
        _window.SetVerticalSyncEnabled(true);
        _window.SetFramerateLimit(60);

        _window.Closed += WindowClosed;
        _window.KeyReleased += WindowOnKeyReleased;
        _window.MouseMoved += WindowOnMouseMoved;
        _window.MouseButtonReleased += WindowOnMouseButtonReleased;
    }

    public abstract void Loop();

    protected abstract void WindowOnMouseMoved(object? sender, MouseMoveEventArgs e);

    protected abstract void WindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e);

    protected abstract void WindowOnKeyReleased(object? sender, KeyEventArgs e);
    
    protected virtual void WindowClosed(object? sender, EventArgs e)
    {
        _window?.Close();
    }
}