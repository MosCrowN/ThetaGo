namespace ConsoleApp1;

internal interface IArbiter
{
    public bool IsMoveAllowed(in Board.States[,] desk);

    public Board.States[,] Captured(in Board.States[,] desk);

    public (float white, float black) Score(in Board.States[,] desk);
}

internal class IngRulesDfs : IArbiter
{
    public bool IsMoveAllowed(in Board.States[,] desk)
    {
        throw new NotImplementedException();
    }

    public Board.States[,] Captured(in Board.States[,] desk)
    {
        throw new NotImplementedException();
    }

    public (float white, float black) Score(in Board.States[,] desk)
    {
        throw new NotImplementedException();
    }
}
