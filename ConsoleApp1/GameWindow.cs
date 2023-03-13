using SFML.Graphics;
using SFML.Window;

namespace ConsoleApp1;

internal class GameWindow : Window
{
    private readonly Board _board;

    private readonly BoardSprite _sprite;

    public GameWindow(string path)
    {
        _board = Serializer<Board>.Deserialize(path);
        Params.DeskSize = _board.Desk.GetLength(0) - 2;
        _sprite = new BoardSprite(_board, (int)SfWindow.Size.X, (int)SfWindow.Size.Y);
    }
    
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
        Serializer<Board>.Serialize(_board, "board.soap");
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
