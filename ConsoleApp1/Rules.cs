namespace ConsoleApp1;

abstract class Dfs
{
    private Board.States[,]? _desk = new Board.States[1,1];

    private Board.States _color = Board.States.Free;

    private static (int white, int black) _captured = (0, 0);

    private bool IsAlive(int x, int y)
    {
        if (_desk![x, y] == Board.States.Free) 
            return true;
        _desk[x, y] = Board.States.Visited;
        if (_desk[x + 1, y] != Board.States.Visited && IsAlive(x + 1, y)) 
            return true;
        if (_desk[x - 1, y] != Board.States.Visited && IsAlive(x - 1, y)) 
            return true;
        if (_desk[x, y + 1] != Board.States.Visited && IsAlive(x, y + 1)) 
            return true;
        if (_desk[x, y - 1] != Board.States.Visited && IsAlive(x, y - 1)) 
            return true;       
        return false;
    }
    
    public void Capture(Board.States[,] desk, int x, int y)
    {
        _desk = desk.Clone() as Board.States[,];
        _color = _desk![x, y];
        
        if (IsAlive(x, y)) return;
        
        for (var ix = 1; ix <= Params.DeskSize; ++ix)
        for (var iy = 1; iy <= Params.DeskSize; ++iy)
            if (_desk[ix, iy] == Board.States.Visited)
            {
                desk[ix, iy] = Board.States.Free;
                if (_color == Board.States.White) 
                    ++_captured.white;
                else ++_captured.black;
            }
    }
}
internal interface IArbiter
{
    public bool IsMoveAllowed(in Board.States[,] desk, bool isWhite);

    public void Capture(Board.States[,] desk, int x, int y);

    public (float white, float black) Score(in Board.States[,] desk);
}

internal class IngRulesDfs : Dfs, IArbiter 
{
    public bool IsMoveAllowed(in Board.States[,] desk, bool isWhite) => true;

    public (float white, float black) Score(in Board.States[,] desk)
    {
        throw new NotImplementedException();
    }
}
