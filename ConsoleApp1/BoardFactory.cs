using SFML.Graphics;
namespace ConsoleApp1;
class BoardFactory
{
    public Board board { private set; get; }
    public Sprite desk { get; }

    private BoardFactory? _boardFactory;
    private BoardFactory(int size)
    {
        size += 2;
        board = new Board(size);
        for (int i = 0; i < size; ++i)
        {
            board.desk[i, 0] = -2;
            board.desk[i, size - 1] = -2;
            board.desk[0, i] = -2;
            board.desk[size - 1, i] = -2;
        }

        var texture = new RenderTexture(900, 900);
        texture.Clear();
        texture.Display();

        desk = new Sprite(texture.Texture);
    }

    public BoardFactory GetBoard(int size) 
        => _boardFactory ??= new BoardFactory(size);
}