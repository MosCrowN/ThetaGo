using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ConsoleApp1;

internal class SettingsWindow : Window
{
    public SettingsWindow() {}
    
    public override void Loop()
    {
        Settings.Ost.Play();

        while (_window.IsOpen)
        {
            _window.DispatchEvents();

            _window.Clear();
            _window.Draw(_layout);
            _window.Display();
        }
        
        Settings.Ost.Stop();
    }

    protected override void WindowOnMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void WindowOnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void WindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void WindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        throw new NotImplementedException();
    }
}