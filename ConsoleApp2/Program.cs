using MNIST.IO;
using Tensorflow;
using static Tensorflow.KerasApi;
using Tensorflow.NumPy;
using Tensorflow.Keras.Utils;

namespace ConsoleApp2;

internal class Education
{
    private static void MnistExample()
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
    private static void Main()
    {
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
            batch_size: 100,
            epochs: 50,
            verbose: 1,
            validation_split: 0.2f);
        
        Console.WriteLine("Trained");
        
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

        var ans = model.predict(GamesToData.PyPlay(game));
        
        Console.WriteLine(ans.ToArray()[0]);
    }

    /*private static void TrainModel()
    {
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
            batch_size: 100,
            epochs: 50,
            verbose: 1,
            validation_split: 0.2f);
        
        Console.WriteLine("Trained");
        
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

        var ans = model.predict(GamesToData.PyPlay(game));
        
        Console.WriteLine(ans.ToArray()[0]);
        
        model.save("./go_try1");
    } */
}