using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using Numpy;

namespace ConsoleApp3;

internal class Dataset
{
    public readonly NDarray Label;
    
    public readonly NDarray Data;

    public Dataset(Container data)
    {
        Label = np.array(data.Labels.ToArray());
        var x = new float[data.Data.Count, 362];
        for (var i = 0; i < data.Data.Count; ++i)
        for (var j = 0; j < 362; ++j)
            x[i, j] = data.Data[i][j];
        Data = np.array(x);
    }
}

[Serializable]
internal class Container
{
     public List<sbyte> Labels = new ();

     public List<float[]> Data = new();
}

internal class GamesToData
{
    public Container Container = new(); 

    private string _text = "";

    private  void LoadTextFromFile(string path)
    {
        using var stream = File.OpenRead(path);
        var buffer = new byte[stream.Length];
        var _ = stream.Read(buffer, 0, buffer.Length);
        _text = Encoding.Default.GetString(buffer);
    }
    
    public GamesToData()
    {
        var n = 0;
        for (var i = 1; i < 30000; ++i)
        {
            var path = "dataset/" + i + ".sgf";
            if (!File.Exists(path)) continue;

            LoadTextFromFile(path);

            var j = 0;
            do ++j;
            while (j < _text.Length - 1 &&
                   (_text[j] != 'R' || _text[j + 1] != 'E'));
            if (j == _text.Length - 1) 
                continue;
            
            if (_text[j+3] == 'W')
                Container.Labels.Add(1);
            else if (_text[j+3] == 'B') 
                Container.Labels.Add(-1);
            else continue;

            Play(n, j);

            ++n;
            if (i % 258 != 0) continue;
            Console.Clear();
            Console.WriteLine($"Loading data: {i / 258f} %");
        }
        
        Console.Clear();
        Console.WriteLine($"Loading data: ready ({100 - n / 258f}% loss)");
    }

    private void Play(int n, int j)
    {
        var board = new Board();
        for (; j < _text.Length - 3; ++j)
        {
            var x = C2I(_text[j + 2]);
            var y = C2I(_text[j + 3]);
            if (x < 0 || y < 0) continue;
            if (_text[j] == 'W')
                board.PutStone(x, y, true);
            else if (_text[j] == 'B')
                board.PutStone(x, y, false);
        }
            
        Container.Data.Add(new float[362]);
        for (var x = 0; x < 19; ++x)
        for (var y = 0; y < 19; ++y)
        {
            if (board.Desk[x + 1, y + 1] == Board.States.White)
                Container.Data[n][x + 19 * y] = 1;
            else if (board.Desk[x + 1, y + 1] == Board.States.Black)
                Container.Data[n][x + 19 * y] = -1;
        }
        Container.Data[n][361] = board.DScore();
    }

    public static float[] Play(string text)
    {
        var board = new Board();
        for (var j = 0; j < text.Length - 3; ++j)
        {
            var x = C2I(text[j + 2]);
            var y = C2I(text[j + 3]);
            if (x < 0 || y < 0) continue;
            if (text[j] == 'W')
                board.PutStone(x, y, true);
            else if (text[j] == 'B')
                board.PutStone(x, y, false);
        }
            
        var data = new float[362];
        for (var x = 0; x < 19; ++x)
        for (var y = 0; y < 19; ++y)
        {
            if (board.Desk[x + 1, y + 1] == Board.States.White)
                data[x + 19 * y] = 1;
            else if (board.Desk[x + 1, y + 1] == Board.States.Black)
                data[x + 19 * y] = -1;
            else data[x + 19 * y] = 0; 
        }
        data[361] = board.DScore();
        return data;
    }

    public static NDarray PyPlay(string text) => np.array(Play(text));

    private static int C2I(char i)
    {
        return i switch
        {
           'a' => 0,
           'b' => 1,
           'c' => 2,
           'd' => 3,
           'e' => 4,
           'f' => 5,
           'g' => 6,
           'h' => 7,
           'j' => 8,
           'k' => 9,
           'l' => 10,
           'm' => 11,
           'n' => 12,
           'o' => 13,
           'p' => 14,
           'q' => 15,
           'r' => 16,
           's' => 17,
           't' => 18,
            _ => -1
        };
    }
}


internal class Board
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
    
    private void CaptureGroup(int x, int y)
    {
        if (Desk[x, y] != States.White &&
            Desk[x, y] != States.Black)
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
    
    private void Capture(int ix, int iy, bool isWhite)
    {
        var color = isWhite ? 
            States.Black : States.White;
        
        if (Desk[ix + 1, iy] == color)
            CaptureGroup(ix + 1, iy);
        if (Desk[ix - 1, iy] == color)
            CaptureGroup(ix - 1, iy);
        if (Desk[ix, iy + 1] == color)
            CaptureGroup(ix, iy + 1);
        if (Desk[ix, iy - 1] == color)
            CaptureGroup(ix, iy - 1);

        CaptureGroup(ix + 1, iy);
        CaptureGroup(ix - 1, iy);
        CaptureGroup(ix, iy + 1);
        CaptureGroup(ix, iy - 1);
        CaptureGroup(ix, iy);
    }
    public void PutStone(int ix, int iy, bool isWhite)
    {
        if (isWhite) Desk[ix + 1, iy + 1] = States.White;
        else Desk[ix + 1, iy + 1] = States.Black;

        Capture(ix + 1, iy + 1, isWhite);
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

internal static class _
{
    public static readonly SoapFormatter Formatter = new ();
}

public static class Serializer<T> where T : class
{
    public static void Serialize(T obj, string path)
    {
        using var fs = new FileStream(path, FileMode.Create);
        _.Formatter.Serialize(fs, obj);
    }
    
    public static T Deserialize(string path)
    {
        using var fs = new FileStream(path, FileMode.Open);
        return (T)_.Formatter.Deserialize(fs);
    }
}