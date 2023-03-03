using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ConsoleApp1;

internal class GameWindow : Window
{
    private Board _board;

    private int _ix, _iy;

    public GameWindow()
    {
        _board = new Board((int)_window.Size.X, (int)_window.Size.Y);
    }

    public override void Loop()
    {
        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            
            _window.Clear(Color.White);
            _window.Draw(_board);
            _window.Display();
        }
    }

    protected override void WindowOnMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        _board.Selected = (e.X, e.Y);
        //Console.WriteLine(_board.Selected);
    }

    protected override void WindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        _board.PutStone();
    }

    protected override void WindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Up:
                --_iy;
                if (_iy <= 0)
                    _iy = Settings.DeskSize;
                break;
            case Keyboard.Key.Down:
                ++_iy;
                if (_iy > Settings.DeskSize)
                    _iy = 1;
                break;
            case Keyboard.Key.Left:
                --_ix;
                if (_ix <= 0)
                    _ix = Settings.DeskSize;
                break;
            case Keyboard.Key.Right:
                ++_ix;
                if (_ix > Settings.DeskSize)
                    _ix = 1;
                break;
            case Keyboard.Key.Escape:
                _window.Close();
                break;
        }
    }
}