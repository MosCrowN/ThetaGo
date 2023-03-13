using System.Runtime.Serialization.Formatters.Soap;

namespace ConsoleApp1;

internal static class _
{
    public static readonly SoapFormatter Formatter = new ();
}

public static class Serializer<T> where T : class
{
    public static void Serialize(T obj, string path)
    {
        using var fs = new FileStream(path, FileMode.Create);
        _.Formatter.Serialize(fs, obj);
    }
    
    public static T Deserialize(string path)
    {
        using var fs = new FileStream(path, FileMode.Open);
        return (T)_.Formatter.Deserialize(fs);
    }
}
