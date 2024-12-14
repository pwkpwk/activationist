using System.Diagnostics.CodeAnalysis;
using activationist.app.Stuff;
using BenchmarkDotNet.Attributes;

namespace activationist.benchmark;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
[SuppressMessage("ReSharper", "ConvertToConstant.Global")]
[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnassignedField.Global")]
public class UnaryDictionaryMultiConstruction
{
    private static readonly IEqualityComparer<int> Comparer = EqualityComparer<int>.Default;

    [Params(1, 10, 1000)]
    public int Repetitions;

    [Benchmark(Baseline = true)]
    public IDictionary<int, int>  PlatformMultiConstruction()
    {
        IDictionary<int, int>? dictionary = null;
        
        for (int i = 0; i < Repetitions; i++) 
        {
            dictionary = new Dictionary<int, int>(Comparer) { { 1, 10 } };
        }
        
        return dictionary!;
    }    

    [Benchmark]
    public IDictionary<int, int> UnaryMultiConstruction()
    {
        IDictionary<int, int>? dictionary = null;

        for (int i = 0; i < Repetitions; i++)
        {
            dictionary = new UnaryDictionary<int, int>(1, 1000);
        }
        
        return dictionary!;
    }
}