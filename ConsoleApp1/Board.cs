class Board
{
    private int _size;
    public readonly sbyte[,] desk;

    private static Board? _board;
    private Board(int size)
    {
        _size = size;
        desk = new sbyte[size, size];
    }

    public static Board GetBoard(int size) 
        => _board ??= new Board(size);

    public bool MakeTurn(bool isWhite, int x, int y)
    {
        desk[x, y] = 1; 
        return false;
    }
}