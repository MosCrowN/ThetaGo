using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ConsoleApp1;

internal class MainWindow : Window
{
    public MainWindow()
    {
        _layout = ButtonFactory.MainMenu(_window.Size.X, _window.Size.Y);
    }
    
    public override void Loop()
    {
        var mainMenuBg = new Sprite(Settings.MenuBg);
        mainMenuBg.Scale = new Vector2f(_window.Size.X / (float)mainMenuBg.Texture.Size.X,
            _window.Size.Y / (float)mainMenuBg.Texture.Size.Y);
        
        Settings.Ost.Play();

        while (_window.IsOpen)
        {
            _window.DispatchEvents();

            _window.Clear();
            _window.Draw(mainMenuBg);
            _window.Draw(_layout);
            _window.Display();
        }
        
        Settings.Ost.Stop();
    }

    protected override void WindowOnMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        if (_layout == null) return;
        Select = _layout.Selected(e.X, e.Y, IsMousePressed);
    }

    protected override void WindowOnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        IsMousePressed = true;
        if (_layout == null) return;
        Select = _layout.Selected(e.X, e.Y, true);
    }

    protected override void WindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        IsMousePressed = false;
        if (e.Button != Mouse.Button.Left) return;
        switch (Select)
        {
            case 1:
                _window.Close();
                break;
            case 3:
                _window.Close();
                break;
        }
    }

    protected override void WindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Escape:
                _window.Close();
                break;
        }
    }
}