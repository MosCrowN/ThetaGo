﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ConsoleApp1;

internal class MainWindow : Window
{
    public MainWindow()
    {
        Layout = LayoutFactory.MainMenu(SfWindow.Size.X, SfWindow.Size.Y);
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
        
        Params.Ost.Stop();
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