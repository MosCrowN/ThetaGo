class Arbiter
{
    public int scoreWhite, scoreBlack;
    private Board _board;

    private static Arbiter _arbiter;
    private Arbiter()
    {
        _board = Board.GetBoard(0);
    }
    public static Arbiter GetArbiter() 
        => _arbiter ??= new Arbiter();
}

static class KoChecker
{
    private static sbyte[,]? _lastBlackState;
    private static sbyte[,]? _lastWhiteState;

    public static bool KoCheck(sbyte[,] curState, bool isWhite)
    {
        if (isWhite)
        {
            if (_lastWhiteState != null && Equals(_lastWhiteState, curState))
                return false;
            _lastWhiteState = curState.Clone() as sbyte[,];
        }
        else
        {
            if (_lastBlackState != null && Equals(_lastBlackState, curState))
                return false;
            _lastBlackState = curState.Clone() as sbyte[,];
        }

        return true;
    }
}