namespace ConsoleApp2;

public class Board
{
    public States[,] Desk;

    public Board()
    {
        var size = 21;
        Desk = new States[size, size];
        
        for (var i = 0; i < size; ++i)
        for (var j = 0; j < size; ++j)
                Desk[i, j] = States.Free;
        for (var i = 0; i < size; ++i)
        {
            Desk[i, 0] = States.Edge;
            Desk[i, size - 1] = States.Edge;
            Desk[0, i] = States.Edge;
            Desk[size - 1, i] = States.Edge;
        }
    }

    private States _color = States.Free;

    private int _captured;

    public float DScore() => _captured / (_captured > 0 ? 1f + _captured : 1f - _captured);
        
    private bool IsAlive(int x, int y)
    {
        if (Desk[x, y] == States.Free) 
            return true;
        Desk[x, y] = States.Visited;
        if (Desk[x + 1, y] == _color && IsAlive(x + 1, y) ||
            Desk[x + 1, y] == States.Free) 
            return true;
        if (Desk[x - 1, y] == _color && IsAlive(x - 1, y) ||
            Desk[x - 1, y] == States.Free) 
            return true;
        if (Desk[x, y + 1] == _color && IsAlive(x, y + 1) ||
            Desk[x, y + 1] == States.Free) 
            return true;
        return Desk[x, y - 1] == _color && IsAlive(x, y - 1) ||
               Desk[x, y - 1] == States.Free;
    }
    
    private void Capture(int x, int y)
    {
        if (Desk[x,y] != States.White &&
            Desk[x,y] != States.Black)
            return;
        
        _color = Desk[x, y];
        
        var flag = IsAlive(x, y);

        for (var ix = 1; ix <= 19; ++ix)
        for (var iy = 1; iy <= 19; ++iy)
            if (Desk[ix, iy] == States.Visited)
            {
                if (flag)
                {
                    Desk[ix, iy] = _color;
                    continue;
                }
                Desk[ix, iy] = States.Free;
                if (_color == States.White) 
                    --_captured;
                else ++_captured;
            }
    }
    public void PutStone(int ix, int iy, bool isWhite)
    {
        if (isWhite) Desk[ix + 1, iy + 1] = States.White;
        else Desk[ix + 1, iy + 1] = States.Black;
        
        Capture(ix, iy + 1);
        Capture(ix + 1, iy);
        Capture(ix + 2, iy + 1); 
        Capture(ix + 1, iy + 2);
        Capture(ix + 1, iy + 1);
    }

    public enum States : sbyte
    {
        White,
        Black,
        Free,
        Edge,
        Visited
    }
}