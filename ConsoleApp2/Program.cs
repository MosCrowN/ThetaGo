namespace ConsoleApp2;

internal static class DMath
{
    public static decimal Exp(decimal x)
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
    
    public delegate decimal FuncDel(decimal v);
    
    public static FuncDel FuncList(string name)
    {
        return name switch
        {
            "logistic" => v => 1 / (1 + Exp(v)),
            "ReLu" => v => v > 0 ? v : 0,
            _ => v => v
        };
    }

    public static FuncDel DiffList(string name)
    {
        return name switch
        {
            "logistic" => v => v * (1 - v),
            "ReLu" => v => v > 0 ? 1 : 0,
            _ => v => 1
        }; 
    }
    private static void Main()
    {
        Console.WriteLine(Math.Exp(5d));
        Console.WriteLine(Exp(5M));
    }
}

class Neuron
{
    //weighted summ value, function activation value and local gradient
    public decimal V, F, G; 
    //function activation and differencial
    private DMath.FuncDel _func, _diff;

    public Neuron(DMath.FuncDel func, DMath.FuncDel diff)
    {
        V = F = G = 0;
        _func = func;
        _diff = diff;
    }

    public void Activate()
    {
        F = _func(V);
    }
}

class NeuralNetwork
{
    private Neuron[][] _neur;
    //weights of synapses
    private decimal[][,] _w;

    public NeuralNetwork(int[] layers, string[][] fAct, decimal[][,]? w = null)
    {
        //remember about the bias!!!
        _neur = new Neuron[layers.Length][];
        _w = new decimal[layers.Length - 1][,];

        for (var layerNum = 0; layerNum < layers.Length; ++layerNum)
        {
            _neur[layerNum] = new Neuron[layers[layerNum] + 1];
            for (var neuronNum = 0; neuronNum < layers[layerNum]; ++neuronNum)
                _neur[layerNum][neuronNum] = new Neuron(
                    DMath.FuncList(fAct[layerNum][neuronNum]),
                    DMath.DiffList(fAct[layerNum][neuronNum]));
            _neur[layerNum][layers[layerNum]] = 
                new Neuron(v => 1, v => 0);
        }

        for (var synapseLayer = 0; synapseLayer < layers.Length - 1; ++synapseLayer)
        {
            _w[synapseLayer] = new decimal[layers[synapseLayer] + 1, layers[synapseLayer + 1]];
            for (var input = 0; input <= layers[synapseLayer]; ++input)
            for (var output = 0; output < layers[synapseLayer + 1]; ++output)
                _w[synapseLayer][input, output] = w == null ? 
                    0 : w[synapseLayer][input, output];
            //TODO: bias neurons weight
        }
    }

    public decimal[] Count(decimal[] data)
    {
        if (data.Length != _neur[0].Length)
            return new[] { 0M };
        
        for (var i = 0; i < _neur[0].Length; ++i)
            _neur[0][i].V = data[i];
        
        for (var layerNum = 1; layerNum < _neur.Length; ++layerNum)
        for (var receiver = 0; receiver < _neur[layerNum].Length; ++receiver)
            {
                for (var sender = 0; sender < _neur[layerNum - 1].Length; ++layerNum)
                    _neur[layerNum][receiver].V += _neur[layerNum - 1][sender].F *
                                                   _w[layerNum - 1][sender, receiver];
                _neur[layerNum][receiver].Activate();
            }

        var ans = new decimal[_neur[^1].Length];
        for (var i = 0; i < ans.Length; ++i)
            ans[i] = _neur[^1][i].F;

        return ans;
    }
}

