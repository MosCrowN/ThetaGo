namespace ConsoleApp2;

internal static class DMath
{
    private static void Main()
    {
        Console.WriteLine(Math.Exp(5d));
    }
}

internal interface INeuron
{
    public double Value { set; get; }
    
    public double ThFunction { set; get; }
    
    public void DirectPropagation();

    public void BackPropagation();

    public void Clear();
}

internal class Synapse
{
    public double Weight; //weight

    public INeuron InputNeuron; //input

    public INeuron OutputNeuron; //output
}

internal class Neuron: INeuron
{
    public double Value { get; set; } //value

    public double ThFunction { set; get; } //function

    public double Gradient; //gradient

    public Synapse[]? InputSynapses; //input

    public Synapse[]? OutputSynapses; //output

    public (INeuron? prev, INeuron? next) NearNeurons; //nearest previous & next 

    public static double Step; //step
    
    public virtual void DirectPropagation() //Right
    {
        ThFunction = 2 / (1 + Math.Exp(Value)) - 1;
        for (var i = 0; i < OutputSynapses!.Length; ++i)
            OutputSynapses[i].OutputNeuron.Value += ThFunction * OutputSynapses[i].Weight;
        Value = 0;
        NearNeurons.next?.DirectPropagation();
    }
    
    public virtual void BackPropagation() //Back
    {
        Gradient = Value * 0.5 * (1 + ThFunction) * (1 - ThFunction);
        for (var i = 0; i < InputSynapses!.Length; ++i)
        { 
            InputSynapses[i].Weight -= Step * Gradient * InputSynapses[i].InputNeuron.ThFunction;
            InputSynapses[i].InputNeuron.Value += Gradient * InputSynapses[i].Weight;
        }
        NearNeurons.prev?.BackPropagation();
    }
    
    public virtual void Clear()
    {
        Value = 0;
        ThFunction = 0;
        Gradient = 0;
        NearNeurons.next?.Clear();
    }
}

internal class InputNeuron: Neuron
{
    public double InputValue;
    
    public override void DirectPropagation()
    {
        ThFunction = Value;
        for (var i = 0; i < OutputSynapses!.Length; ++i)
            OutputSynapses[i].OutputNeuron.Value += ThFunction * OutputSynapses[i].Weight;
        Value = 0;
        NearNeurons.next?.DirectPropagation();
    }

    public override void Clear()
    {
        InputValue = 0;
        base.Clear();
    } 
}

internal class OutputNeuron : Neuron
{
    public double OutputValue;

    public override void BackPropagation()
    {
        Gradient = Value - OutputValue;
        for (var i = 0; i < InputSynapses!.Length; ++i)
        { 
            InputSynapses[i].Weight -= Step * Gradient * InputSynapses[i].InputNeuron.ThFunction;
            InputSynapses[i].InputNeuron.Value += Gradient * InputSynapses[i].Weight;
        }
        NearNeurons.prev?.BackPropagation();
    }
    
    public override void Clear()
    {
        OutputValue = 0;
        base.Clear();
    } 
}