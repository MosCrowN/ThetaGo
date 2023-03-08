using MNIST.IO;

namespace ConsoleApp2;

internal class Test
{
    private static Random _r = new ();
    
    private static byte[] _labels = new byte[60000];
    private static decimal[][] _images = new decimal[60000][];

    private static void PrepareData()
    {
        var data = FileReaderMNIST.LoadImagesAndLables(
            "data/train-labels-idx1-ubyte.gz",
            "data/train-images-idx3-ubyte.gz");
        var i = 0;
        foreach (var t in data)
        {
            _labels[i] = t.Label;
            _images[i] = new decimal[784];
            for (var j = 0; j < 28; ++j)
            for (var k = 0; k < 28; ++k)
                _images[i][j * k] = t.Image[j, k];
            
            if (++i % 6000 != 0) continue;
            Console.Write("Data prepare is ready for ");
            Console.Write(i / 600);
            Console.WriteLine("%");
        }
    }
    private static void Main()
    { 
        /*
        PrepareData();

       var net = new NeuralNetwork(new[] { 784, 800, 10 }, new[] { "", "logistic", "logistic" });
       decimal loss = 0;
       for (var i = 0;;++i) {
           net.Count(_images[_r.Next(0, 59999)]);
           var exp = new decimal[10];
           exp[_labels[_r.Next(0, 59999)]] = 1;
           net.Train(exp);
           if (i % 100 == 0)
           {
               Console.Write("Iteration: ");
               Console.Write(i);
               Console.Write(" , loss: ");
               Console.WriteLine(loss);
               loss = 0;
           }
           else loss += net.Loss();
       }
       */

        var testData = new[]
        {
            new[] { -1M, -1, -1 },
            new[]{-1M, -1, 1},
            new[]{-1M, 1, -1},
            new[]{-1M, 1, 1},
            new[]{1M, -1, -1},
            new[]{1M, -1, 1},
            new[]{1M, 1, -1},
            new[]{1M, 1, 1}
        };

        var ans = new[] { -1M, 1, -1, 1, -1, 1, -1, -1 };
        
        var net = new NeuralNetwork(new[] { 3, 3, 1 }, new[] { "", "th", "th" });
        
        //Console.WriteLine(Dm.E(-1));
        decimal loss = 0;
        for (var i = 0; i < 1000000;++i)
        {
            var d = _r.Next(0, 8);
            net.Count(testData[d]);
            net.Train(new[]{ans[d]});
            if (i % 10000 == 0)
            {
                Console.Write("Iteration: ");
                Console.Write(i);
                Console.Write(" , loss: ");
                Console.WriteLine(loss / 10000);
                loss = 0;
            }
            else loss += net.Loss();
        }

        for (int i = 0; i < 8; ++i)
        {
            var res = net.Count(testData[i]);
            Console.Write("Neur: ");
            Console.Write(res[0]);
            Console.Write(" , Truly: ");
            Console.WriteLine(ans[i]);
        }
    }
}

//Dm - decimal math
internal static class Dm
{
    private static Random _r = new ();
    //R - random
    public static decimal R(decimal maxAbs = 1)
    {
        maxAbs = maxAbs <= 1 ? Convert.ToDecimal(_r.NextDouble()) :
            Convert.ToDecimal(_r.Next()) % maxAbs;
        if (_r.Next() % 2 == 0) maxAbs *= -1;
        return maxAbs;
    }
    
    //E - exponent
    public static decimal E(decimal x)
    {
        if (x > 64) return 0;
        decimal ans = 1, pow = x, i = 1;
        do
        {
            ++i;
            ans += pow;
            pow *= x / i;
        } while (pow != 0);

        return ans;
    }
    
    //Fd - Function delegate
    public delegate decimal Fd(decimal v);
    
    //Fl - activation Functions list
    public static Fd Fl(string name)
    {
        return name switch
        {
            "th" => v => 2 / (1 + E(0M-v)) - 1,
            "logistic" => v => 1 / (1 + E(v)),
            "relu" => v => v > 0 ? v : 0,
            _ => v => v
        };
    }

    //Dl - activation function Differentials list
    public static Fd Dl(string name)
    {
        return name switch
        {
            "th" => v => 0.5M * (1 + v) * (1 - v),
            "logistic" => v => v * (1 - v),
            "relu" => v => v > 0 ? 1 : 0,
            _ => _ => 1
        }; 
    }
}

class Neuron
{
    //weighted sum value, function activation value and local gradient
    public decimal V, F, G; 
    //A - function Activation, D - differential
    public Dm.Fd A, D;

    public Neuron(Dm.Fd func, Dm.Fd diff)
    {
        V = F = G = 0;
        A = func;
        D = diff;
    }
}

class NeuralNetwork
{
    //_n - neurons
    private Neuron[][] _n;
    //_w - weights of synapses
    private decimal[][,] _w;

    //l - layers num and struct, f - activation functions, isB - is Bias, w - weights
    public NeuralNetwork(int[] l, string[] f, bool isB = true, decimal[][,]? w = null)
    {
        _n = new Neuron[l.Length][];
        _w = new decimal[l.Length - 1][,];

        //iL - iLayer, iN - iNeuron
        for (var iL = 0; iL < l.Length; ++iL)
        {
            _n[iL] = new Neuron[l[iL] + 1];
            for (var iN = 0; iN < l[iL]; ++iN)
                _n[iL][iN] = new Neuron(Dm.Fl(f[iL]), Dm.Dl(f[iL]));
            _n[iL][l[iL]] = isB ? new Neuron(_ => 1, _ => 0) :
                 new Neuron(_ => 0, _ => 0);
        }
        
        //iSl - iSynapse layer, iI - iInput, iO - iOutput, iB - iBias
        for (var iSl = 0; iSl < l.Length - 1; ++iSl)
        {
            _w[iSl] = new decimal[l[iSl] + 1, l[iSl + 1]];
            for (var iI = 0; iI < l[iSl]; ++iI)
            for (var iO = 0; iO < l[iSl + 1]; ++iO)
                _w[iSl][iI, iO] = w == null ? Dm.R() : w[iSl][iI, iO];
            for (var iB = 0; iB < l[iSl + 1]; ++iB)
                _w[iSl][l[iSl], iB] = w == null ? Dm.R() : w[iSl][l[iSl], iB];
        }
    }
    
    public decimal[] Count(decimal[] data)
    {
        if (data.Length != _n[0].Length - 1)
            return new[] { 0M };

        for (var i = 0; i < _n[0].Length - 1; ++i)
            _n[0][i].F = _n[0][i].V = data[i];

        //iL - iLayer num, iR - iReceiver, iS - iSender
        for (var iL = 1; iL < _n.Length; ++iL)
        {
            for (var iR = 0; iR < _n[iL].Length - 1; ++iR)
            {
                _n[iL][iR].V = 0;
                for (var iS = 0; iS < _n[iL - 1].Length; ++iS)
                    _n[iL][iR].V += _n[iL - 1][iS].F * _w[iL - 1][iS, iR];
                _n[iL][iR].F = _n[iL][iR].A(_n[iL][iR].V);
            }

            _n[iL][^1].F = 1;
        }

        var ans = new decimal[_n[^1].Length];
        for (var i = 0; i < ans.Length; ++i)
            ans[i] = _n[^1][i].F;

        return ans;
    }

    private static decimal _dy = 0.001M;

    private decimal _loss;
    public void Train(decimal[] data)
    {
        if (data.Length != _n[^1].Length - 1)
            return;

        _loss = 0;
        for (var i = 0; i < _n[^1].Length - 1; ++i)
        {
            var e = _n[^1][i].F - data[i];
            _loss += e * e / _n[^1].Length;
            _n[^1][i].G = e * _n[^1][i].D(_n[^1][i].F);
        }

        //iL - iLayer num, iR - iReceiver, iS - iSender
        for (var iL = _n.Length - 1; iL > 0; --iL)
        for (var iR = 0; iR < _n[iL - 1].Length; ++iR)
        {
            _n[iL - 1][iR].V = 0;
            for (var iS = 0; iS < _n[iL].Length - 1; ++iS)
            {
                _w[iL - 1][iR, iS] -= _dy * _n[iL][iS].G * _n[iL - 1][iR].F;
                _n[iL - 1][iR].V += _n[iL][iS].G * _w[iL - 1][iR, iS];
            }
            _n[iL - 1][iR].G = _n[iL - 1][iR].V * _n[iL - 1][iR].D(_n[iL - 1][iR].F);
        }
    }

    public decimal Loss() =>  _loss;
}
