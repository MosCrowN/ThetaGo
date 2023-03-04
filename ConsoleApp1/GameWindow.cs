using SFML.Graphics;
using SFML.Window;

namespace ConsoleApp1;

internal class GameWindow : Window
{
    private readonly Board _board;

    private readonly BoardSprite _sprite;

    private int _ix, _iy;

    public GameWindow()
    {
        _board = new Board();
        _sprite = new BoardSprite(_board, (int)SfWindow.Size.X, (int)SfWindow.Size.Y);
    }

    public override void Loop()
    {
        while (IsOpen)
        {
            SfWindow.DispatchEvents();
            
            SfWindow.Clear(Color.White);
            SfWindow.Draw(_sprite);
            SfWindow.Display();
        }
    }

    protected override void SfWindowOnMouseMoved(object? sender, MouseMoveEventArgs e)
    {
        _sprite.Selected = (e.X, e.Y);
    }

    protected override void SfWindowOnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        
    }

    protected override void SfWindowOnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {
        _board.PutStone();
    }

    protected override void SfWindowOnKeyReleased(object? sender, KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Up:
                --_iy;
                if (_iy <= 0)
                    _iy = Params.DeskSize;
                break;
            case Keyboard.Key.Down:
                ++_iy;
                if (_iy > Params.DeskSize)
                    _iy = 1;
                break;
            case Keyboard.Key.Left:
                --_ix;
                if (_ix <= 0)
                    _ix = Params.DeskSize;
                break;
            case Keyboard.Key.Right:
                ++_ix;
                if (_ix > Params.DeskSize)
                    _ix = 1;
                break;
            case Keyboard.Key.Escape:
                IsOpen = false;
                break;
        }
    }
}