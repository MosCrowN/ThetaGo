namespace ConsoleApp1;

public interface IArbiter
{
    public (float white, float black) Score(in Board.States[,] desk);
}

[Serializable]
public class IngRules : IArbiter 
{
    public (float white, float black) Score(in Board.States[,] desk)
    {
        throw new NotImplementedException();
    }
}
