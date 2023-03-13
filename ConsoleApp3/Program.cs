using Keras.Layers;
using Keras.Models;

namespace ConsoleApp3;

internal static class Education
{

    private static Dataset? _data;
    private static void PrepareData()
    {
        var games = new GamesToData();
        
        Serializer<Container>.Serialize(games.Container, "data.soap");
    }
    
    private static void Train()
    {
        PrepareData();
        
        var sourceData = Serializer<Container>.Deserialize("data.soap");

        for (int i = 0; i < 19; ++i)
        {
            for (int j = 0; j < 19; ++j)
            {
                Console.Write($"{sourceData.Data[0][i + 19 * j]}; ");
            }
            Console.WriteLine();
        }
        Console.WriteLine(sourceData.Labels[0]);

        _data = new Dataset(sourceData);
        
        var model = new Sequential();
        model.Add(new Input(362));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(100, activation: "softsign"));
        model.Add(new Dense(1, activation: "tanh"));
        model.Summary();
        
        model.Compile(loss: "mean_squared_error",
            optimizer: "adam", metrics: new[] { "accuracy" });
        
        if (_data.Data is null || _data.Label is null)
            throw new NullReferenceException("Training data is null");
        model.Fit(_data.Data, _data.Label,
            batch_size: 10,
            epochs: 100,
            verbose: 1,
            validation_split: 0.2f);
        
        var json = model.ToJson();
        File.WriteAllText("model.json", json);
        model.SaveWeight("model.h5");
        
        Console.WriteLine(model.Predict(_data.Data));
    }
    private static void Main()
    {
        Train();
        
        var loadedModel = BaseModel.ModelFromJson(File.ReadAllText("model.json"));
        loadedModel.LoadWeight("model.h5");
        
        Console.WriteLine(loadedModel.Predict(_data!.Data));
    }

}

