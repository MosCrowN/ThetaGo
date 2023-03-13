using System.Xml;
using System.Xml.Serialization;
using Tensorflow;

namespace ConsoleApp3;

delegate float Activation(float x);

static class Functions
{
    private static float Sigmoid(float x)
    {
        if (x > 20) return 1;
        if (x < -20) return 0;
        float exp = 1, pow = x = -x;
        for (var i = 1; pow != 0; ++i)
            exp += pow *= x / i;
        return 1 / (1 + exp);
    }
    
    public static Activation GetByName(string? name = null)
    {
        return name switch
        {
            "tanh" => f => 2 * Sigmoid(f) - 1,
            "sigmoid" => Sigmoid,
            _ => f => f
        };
    }  
}

class Net
{
    private float[][] _bias;
    private float[][] _weight;
    private (float val, float func)[][] _neurons;

    private List<Activation> _functions = new ();

    public Net(in List<string?> activation, in List<IVariableV1> weights)
    {
        foreach (var str in activation)
            _functions.Add(Functions.GetByName(str));

        _neurons = new (float, float)[weights.Count / 2 + 1][];
        for (var i = 0; i < weights.Count; i += 2)
            _neurons[i/2] = new (float, float)
                [weights[i].numpy().size / weights[i+1].numpy().size];
        _neurons[^1] = new (float, float)[weights[^1].numpy().size];

        _bias = new float[weights.Count / 2][];
        _weight = new float[weights.Count / 2][];
        for (var i = 0; i < weights.Count; i += 2)
        {
            _weight[i/2] = weights[i].numpy().ToArray<float>();
            _bias[i/2] = weights[i + 1].numpy().ToArray<float>();
        }
    }

    public float[,] Count(in float[,] input)
    {
        if (input.GetLength(1) != _neurons[0].Length)
            throw new Exception("Invalid input data size");

        var ans = new float[input.GetLength(0),_neurons[^1].Length];
        for (var step = 0; step < input.GetLength(0); ++step)
        {
            for (var i = 0; i < _neurons[0].Length; ++i)
                _neurons[0][i].func = input[step, i];

            for (var i = 1; i < _neurons.Length; ++i)
            for (var j = 0; j < _neurons[i].Length; ++j)
            {
                _neurons[i][j].val = 0;
                for (var k = 0; k < _neurons[i - 1].Length; ++k) //TODO: check weights number
                    _neurons[i][j].val += _neurons[i - 1][k].func * _weight[i - 1][j * _neurons[i - 1].Length + k];
                _neurons[i][j].val += _bias[i - 1][j];
                _neurons[i][j].func = _functions[i - 1](_neurons[i][j].val);
            }
            
            for (var i = 0; i < _neurons[^1].Length; ++i)
                ans[step,i] = _neurons[^1][i].func;
        }

        return ans;
    }
    
    public float[,] Count1(in float[,] input)
    {
        if (input.GetLength(1) != _neurons[0].Length)
            throw new Exception("Invalid input data size");

        var ans = new float[input.GetLength(0),_neurons[^1].Length];
        for (var step = 0; step < input.GetLength(0); ++step)
        {
            for (var i = 0; i < _neurons[0].Length; ++i)
                _neurons[0][i].func = input[step, i];

            for (var i = 1; i < _neurons.Length; ++i)
            for (var j = 0; j < _neurons[i].Length; ++j)
            {
                _neurons[i][j].val = 0;
                for (var k = 0; k < _neurons[i - 1].Length; ++k) //TODO: check weights number
                    _neurons[i][j].val += _neurons[i - 1][k].func * _weight[i - 1][j + _neurons[i].Length * k];
                _neurons[i][j].val += _bias[i - 1][j];
                _neurons[i][j].func = _functions[i - 1](_neurons[i][j].val);
            }
            
            for (var i = 0; i < _neurons[^1].Length; ++i)
                ans[step,i] = _neurons[^1][i].func;
        }

        return ans;
    }
    
     public float[,] Count2(in float[,] input)
    {
        if (input.GetLength(1) != _neurons[0].Length)
            throw new Exception("Invalid input data size");

        var ans = new float[input.GetLength(0),_neurons[^1].Length];
        for (var step = 0; step < input.GetLength(0); ++step)
        {
            for (var i = 0; i < _neurons[0].Length; ++i)
                _neurons[0][i].func = input[step, i];

            for (var i = 1; i < _neurons.Length; ++i)
            for (var j = 0; j < _neurons[i].Length; ++j)
            {
                _neurons[i][j].val = 0;
                for (var k = 0; k < _neurons[i - 1].Length; ++k) //TODO: check weights number
                    _neurons[i][j].val += _neurons[i - 1][k].func * _weight[i - 1][j * _neurons[i - 1].Length + k];
                _neurons[i][j].val -= _bias[i - 1][j];
                _neurons[i][j].func = _functions[i - 1](_neurons[i][j].val);
            }
            
            for (var i = 0; i < _neurons[^1].Length; ++i)
                ans[step,i] = _neurons[^1][i].func;
        }

        return ans;
    }
    
    public float[,] Count3(in float[,] input)
    {
        if (input.GetLength(1) != _neurons[0].Length)
            throw new Exception("Invalid input data size");

        var ans = new float[input.GetLength(0),_neurons[^1].Length];
        for (var step = 0; step < input.GetLength(0); ++step)
        {
            for (var i = 0; i < _neurons[0].Length; ++i)
                _neurons[0][i].func = input[step, i];

            for (var i = 1; i < _neurons.Length; ++i)
            for (var j = 0; j < _neurons[i].Length; ++j)
            {
                _neurons[i][j].val = 0;
                for (var k = 0; k < _neurons[i - 1].Length; ++k) //TODO: check weights number
                    _neurons[i][j].val += _neurons[i - 1][k].func * _weight[i - 1][j + _neurons[i].Length * k];
                _neurons[i][j].val -= _bias[i - 1][j];
                _neurons[i][j].func = _functions[i - 1](_neurons[i][j].val);
            }
            
            for (var i = 0; i < _neurons[^1].Length; ++i)
                ans[step,i] = _neurons[^1][i].func;
        }

        return ans;
    }
}

public class MySerializer<T> where T : class
{
    public static void Serialize(T obj, string path)
    {
        XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
        using (var sww = new StringWriter())
        {
            using (XmlTextWriter writer = new XmlTextWriter(sww) { Formatting = Formatting.Indented })
            {
                xsSubmit.Serialize(writer, obj); 
                File.WriteAllText("model.xml", sww.ToString());
            }
        }
    }
    
    public static T? Deserialize(string path)
    {
        XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
        using (var xrd = XmlReader.Create(path))
        {
            return (T)xsSubmit.Deserialize(xrd)!;
        }
    }
}