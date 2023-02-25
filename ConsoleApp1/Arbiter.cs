namespace ConsoleApp1;
class Arbiter
{
    public int scoreWhite, scoreBlack;
    private Board _board;

    public Arbiter(Board board)
    {
        scoreBlack = scoreWhite = 0;
        _board = board;
    }
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