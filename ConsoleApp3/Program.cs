using Tensorflow;
using System.Text;
using System.Text.Json;
using Tensorflow.Keras.Engine;
using static Tensorflow.KerasApi;
using Tensorflow.NumPy;
using Tensorflow.Keras.Utils;

namespace ConsoleApp3;

internal static class Education
{
    ///*
    private static void Main()
    {
        //*
        var data = new GamesToData();
        
        var model = keras.Sequential();
        model.add(keras.layers.Input(362));
        model.add(keras.layers.Dense(128, activation: "tanh"));
        model.add(keras.layers.Dense(1, activation: "sigmoid"));
        model.summary();
        
        model.compile(loss: keras.losses.BinaryCrossentropy(),
            optimizer: keras.optimizers.Adam(), metrics: new[] { "acc" });
        
        
        if (data.Data is null || data.Label is null)
            throw new NullReferenceException();
        model.fit(data.Data, data.Label,
            batch_size: 10,
            epochs: 20,
            verbose: 1,
            validation_split: 0.2f);
            //*/ 
        /*
        //Save model and weights
        model.save("./model");
        model.save_weights("./model/weights.h5");
        var weights = model.Weights.ToArray();
        Console.WriteLine($"length {model.Weights.Count}");
        foreach (var w in weights)
        {
            var w0 = w.numpy().ToArray<float>();
            foreach (var i in w0)
            {
                   Console.Write($"{i}; ");
            }
            Console.WriteLine($"len {w0.Length}");
        }
        //*/  //*
        
        /*Load model and weight
        var neur = new Net(new List<string?> { "tanh", "sigmoid" }, model.Weights);

        var game = "B[pd];W[dc];B[pq];W[co];B[de];W[ce];B[cf];W[cd];B[cq];" +
                   "W[ep];B[eq];W[fp];B[fq];W[gp];B[df];W[dj];B[ec];W[eb];" +
                   "B[dd];W[cb];B[fc];W[fb];B[hc];W[po];B[qo];W[qn];B[qp];" +
                   "W[pn];B[op];W[pi];B[pg];W[mn];B[nh];W[pc];B[qc];W[qd];" +
                   "B[qe];W[rd];B[rc];W[pb];B[od];W[re];B[qb];W[qf];B[oc];" +
                   "W[pf];B[of];W[qg];B[gb];W[hi];B[bo];W[cn];B[bn];W[cm];" +
                   "B[dp];W[do];B[cp];W[mp];B[hq];W[kh];B[hp];W[ho];B[io];" +
                   "W[in];B[jo];W[jn];B[fi];W[fj];B[ih];W[hh];B[jh];W[ki];" +
                   "B[gj];W[gi];B[ej];W[fk];B[ek];W[ei];B[fh];W[eh];B[fl];" +
                   "W[gk];B[fg];W[ck];B[hn];W[go];B[hj];W[hl];B[ij];W[kf];" +
                   "B[if];W[oe];B[ne];W[nf];B[pe];W[og];B[mf];W[oe];B[me];" +
                   "W[mc];B[of];W[pp];B[oq];W[oe];B[ob];W[qq];B[rq];W[qr];" +
                   "B[rr];W[rn];B[sp];W[kd];B[lo];W[mo];B[hm];W[il];B[im];" +
                   "W[jl];B[jm];W[jj];B[ii];W[kn];B[gl];W[gm];B[km];W[ko];" +
                   "B[hk];W[gn];B[lm];W[kl];B[ll];W[lk];B[mk];W[lj];B[nl];" +
                   "W[ln];B[mi];W[nj];B[mj];W[mg];B[ng];W[mh];B[of];W[ni];" +
                   "B[oh];W[lf];B[oi];W[ml];B[pj];W[qj];B[qk])";
        //*/
        var gameRes = data._data.ToArray();
        var test = new float[gameRes.Length,362];
        for (var i = 0; i < gameRes.Length; ++i)
        for (var j = 0; j < 362; ++j)
            test[i, j] = gameRes[i][j];
        //*/
        var path = "model.xml";
        
        MySerializer<Sequential>.Serialize(model, path);
        var model1 = MySerializer<Sequential>.Deserialize(path);

        Console.WriteLine(model1.predict(np.array(test)));
        Console.WriteLine(model.predict(np.array(test)));

    }

}

internal class GamesToData
{
    public List<sbyte> _labels = new ();
    public NDArray? Label;
    
    public List<float[]> _data = new();
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