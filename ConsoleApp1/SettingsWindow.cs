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
        
        Params.Ost.Stop();
    }

    protected override void SfWindowOnMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void SfWindowOnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void SfWindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void SfWindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        throw new NotImplementedException();
    }
}