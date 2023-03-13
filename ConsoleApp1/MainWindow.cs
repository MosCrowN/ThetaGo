using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ConsoleApp1;

internal class MainWindow : Window
{
    public MainWindow()
    {
        Layout = new ButtonLayout();
        Layout.Add("CONTINUE");
        Layout.Add("NEW GAME");
        Layout.Add("SETTINGS");
        Layout.Add("EXIT");
        Layout.Compile(0, (int)SfWindow.Size.X/ 2, 0, (int)SfWindow.Size.Y);
        //LayoutFactory.MainMenu(SfWindow.Size.X, SfWindow.Size.Y);
    }
    
    public override void Loop()
    {
        var mainMenuBg = new Sprite(Params.MenuBg);
        mainMenuBg.Scale = new Vector2f(SfWindow.Size.X / (float)mainMenuBg.Texture.Size.X,
            SfWindow.Size.Y / (float)mainMenuBg.Texture.Size.Y);
        
        Params.Ost.Play();

        while (IsOpen)
        {
            SfWindow.DispatchEvents();

            SfWindow.Clear();
            SfWindow.Draw(mainMenuBg);
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
            case 1:
                IsOpen = false;
                break;
            case 2:
                IsOpen = false;
                break;
            case 3:
                IsOpen = false;
                SfWindow.Close();
                break;
        }
    }

    protected override void SfWindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Escape:
                IsOpen = false;
                SfWindow.Close();
                break;
        }
    }
}
