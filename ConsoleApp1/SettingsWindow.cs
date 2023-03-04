using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ConsoleApp1;

internal class SettingsWindow : Window
{
    public SettingsWindow()
    {
        Layout = LayoutFactory.Settings(SfWindow.Size.X, SfWindow.Size.Y);
    }
    
    public override void Loop()
    {
        Params.Ost.Play();

        while (IsOpen)
        {
            SfWindow.DispatchEvents();

            SfWindow.Clear();
            SfWindow.Draw(Layout);
            SfWindow.Display();
        }
        
        Params.Ost.Pause();
    }

    protected override void SfWindowOnMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        if (Layout == null) return;
        Select = Layout.Selected(e.X, e.Y, IsMousePressed);
    }

    protected override void SfWindowOnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        IsMousePressed = true;
        if (Layout == null) return;
        Select = Layout.Selected(e.X, e.Y, true);
    }

    protected override void SfWindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        IsMousePressed = false;
        if (e.Button != Mouse.Button.Left) return;
        switch (Select)
        {
            case 4:
                IsOpen = false;
                break;
        }
    }

    protected override void SfWindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Escape:
                IsOpen = false;
                break;
        }
    }
}