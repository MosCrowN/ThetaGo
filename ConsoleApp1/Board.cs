namespace ConsoleApp1;
class Board
{
    private int _size;
    public readonly sbyte[,] desk;
    public Board(int size)
    {
        _size = size;
        desk = new sbyte[size, size];
    }

    public bool MakeTurn(bool isWhite, int x, int y)
    {
        desk[x, y] = 1; 
        return false;
    }
}