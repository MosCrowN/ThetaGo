using MNIST.IO;
using static Tensorflow.KerasApi;
using Tensorflow.NumPy;
using Tensorflow.Keras.Utils;

namespace ConsoleApp2;

internal class Test
{
    private static void Main()
    {
        var labels = new byte[60000];
        var images = new float[60000, 784];

        var data = FileReaderMNIST.LoadImagesAndLables(
            "data/train-labels-idx1-ubyte.gz",
            "data/train-images-idx3-ubyte.gz");
        var i = 0;
        foreach (var t in data)
        {
            labels[i] = t.Label;
            for (var j = 0; j < 28; ++j)
            for (var k = 0; k < 28; ++k)
                images[i, j * k] = t.Image[j, k] / 255f;

            if (++i % 6000 != 0) continue;
            Console.Write("Data prepare is ready for ");
            Console.Write(i / 600);
            Console.WriteLine("%");
        }

// build keras model
        var model = keras.Sequential();
        model.add(keras.layers.Dense(128, activation: "relu", input_shape: (784)));
        model.add(keras.layers.Dense(10, activation: "softmax"));
        model.summary();
// compile keras model in tensorflow static graph
        model.compile(loss: keras.losses.CategoricalCrossentropy(),
            optimizer: keras.optimizers.Adam(), metrics: new[] { "acc" });
// prepare dataset
        var xTrain = np.array(images);
        var yTrain = np_utils.to_categorical(np.array(labels), 10);
// training
        model.fit(xTrain, yTrain,
            batch_size: 100,
            epochs: 100,
            verbose: 1,
            validation_split: 0.2f);
// save the model
        model.save("./my_model");
    }
}