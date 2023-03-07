namespace ConsoleApp2;

//Dm - decimal math
internal static class Dm
{
    //E - exponent
    public static decimal E(decimal x)
    {
        decimal ans = 1, pow = x, n = 1, i = 1;
        do
        {
            n /= i;
            ans += pow * n;
            pow *= x;
            ++i;
        } while (n > 0);

        return ans;
    }
    
    //Fd - Function delegate
    public delegate decimal Fd(decimal v);
    
    //Fl - activation Functions list
    public static Fd Fl(string name)
    {
        return name switch
        {
            "logistic" => v => 1 / (1 + E(v)),
            "ReLu" => v => v > 0 ? v : 0,
            _ => v => v
        };
    }

    //Dl - activation function Differentials list
    public static Fd Dl(string name)
    {
        return name switch
        {
            "logistic" => v => v * (1 - v),
            "ReLu" => v => v > 0 ? 1 : 0,
            _ => _ => 1
        }; 
    }
    private static void Main()
    {
        Console.WriteLine(Math.Exp(5d));
        Console.WriteLine(E(5M));
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

    //l - layers num and struct, f - activation functions, w - weights
    public NeuralNetwork(int[] l, string[][] f, decimal[][,]? w = null)
    {
        _n = new Neuron[l.Length][];
        _w = new decimal[l.Length - 1][,];

        //iL - iLayer, iN - iNeuron
        for (var iL = 0; iL < l.Length; ++iL)
        {
            _n[iL] = new Neuron[l[iL] + 1];
            for (var iN = 0; iN < l[iL]; ++iN)
                _n[iL][iN] = new Neuron(Dm.Fl(f[iL][iN]), Dm.Dl(f[iL][iN]));
            _n[iL][l[iL]] = new Neuron(_ => 1, _ => 0);
        }
        
        //iSl - iSynapse layer, iI - iInput, iO - iOutput, iB - iBias
        for (var iSl = 0; iSl < l.Length - 1; ++iSl)
        {
            _w[iSl] = new decimal[l[iSl] + 1, l[iSl + 1]];
            for (var iI = 0; iI < l[iSl]; ++iI)
            for (var iO = 0; iO < l[iSl + 1]; ++iO)
                _w[iSl][iI, iO] = w == null ? 0 : w[iSl][iI, iO];
            for (var iB = 0; iB < l[iSl + 1]; ++iB)
                _w[iSl][l[iSl], iB] = w == null ? 0 : w[iSl][l[iSl], iB];
            //TODO: bias neurons weight
        }
    }
    
    public decimal[] Count(decimal[] data)
    {
        if (data.Length != _n[0].Length)
            return new[] { 0M };

        for (var i = 0; i < _n[0].Length; ++i)
            _n[0][i].F = _n[0][i].V = data[i];

        //iL - iLayer num, iR - iReceiver, iS - iSender
        for (var iL = 1; iL < _n.Length; ++iL)
        for (var iR = 0; iR < _n[iL].Length - 1; ++iR)
        {
            _n[iL][iR].V = 0;
            for (var iS = 0; iS < _n[iL - 1].Length; ++iL)
                _n[iL][iR].V += _n[iL - 1][iS].F * _w[iL - 1][iS, iR];
            _n[iL][iR].F = _n[iL][iR].A(_n[iL][iR].V);
        }

        var ans = new decimal[_n[^1].Length];
        for (var i = 0; i < ans.Length; ++i)
            ans[i] = _n[^1][i].F;

        return ans;
    }

    private static decimal _dy = 0.01M;
    
    public void Train(decimal[] data)
    {
        if (data.Length != _n[^1].Length)
            return;

        for (var i = 0; i < _n[^1].Length; ++i)
            _n[^1][i].G = _n[^1][i].F - data[i];

        //iL - iLayer num, iR - iReceiver, iS - iSender
        for (var iL = _n.Length - 1; iL > 0; --iL)
        for (var iR = 0; iR < _n[iL - 1].Length; ++iR)
        {
            _n[iL][iR].V = 0;
            for (var iS = 0; iS < _n[iL].Length - 1; ++iL)
            {
                _w[iL - 1][iS, iR] -= _dy * _n[iL][iS].G * _n[iL][iS].F;
                _n[iL][iR].V += _n[iL - 1][iS].G * _w[iL - 1][iS, iR];
            }
            _n[iL][iR].G = _n[iL][iR].V * _n[iL][iR].D(_n[iL][iR].F);
        }
    }
}
