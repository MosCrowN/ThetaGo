using System.Text;
using Tensorflow.NumPy;

namespace ConsoleApp2;

class GamesToData
{
    private static List<sbyte> _labels = new ();
    public NDArray? Label;
    
    private static List<float[]> _data = new();
    public NDArray? Data;

    private string _text = "";

    private  void LoadTextFromFile(string path)
    {
        using var fstream = File.OpenRead(path);
        var buffer = new byte[fstream.Length];
        var _ = fstream.Read(buffer, 0, buffer.Length);
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
                _labels.Add(1);
            else if (_text[j+3] == 'B') 
                _labels.Add(0);
            else continue;

            Play(n, j);

            ++n;
            if (i % 258 != 0) continue;
            Console.Clear();
            Console.WriteLine($"Loading data: {i / 258f} %");
        }
        
        Console.Clear();
        Console.WriteLine($"Loading data: ready ({100 - n / 258f}% loss)");

        List2Np();
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
            
        _data.Add(new float[362]);
        for (var x = 0; x < 19; ++x)
        for (var y = 0; y < 19; ++y)
        {
            if (board.Desk[x + 1, y + 1] == Board.States.White)
                _data[n][x + 19 * y] = 1;
            else if (board.Desk[x + 1, y + 1] == Board.States.Black)
                _data[n][x + 19 * y] = -1;
        }
        _data[n][361] = board.DScore();
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

    public static NDArray PyPlay(string text) => np.array(Play(text));

    private void List2Np()
    {
        Label = np.array(_labels.ToArray());
        var data = new float[_data.Count, 362];
        for (var i = 0; i < _data.Count; ++i)
        for (var j = 0; j < 362; ++j)
            data[i, j] = _data[i][j];
        Data = np.array(data);
    }

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
           'i' => 8,
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

class MnistExample {}