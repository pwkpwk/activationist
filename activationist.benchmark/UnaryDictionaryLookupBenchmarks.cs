using System.Diagnostics.CodeAnalysis;
using activationist.app.Stuff;
using BenchmarkDotNet.Attributes;

namespace activationist.benchmark;

[SuppressMessage("ReSharper", "ConvertToConstant.Global")]
[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnassignedField.Global")]
public class UnaryDictionaryLookupBenchmarks
{
    private IDictionary<int, int>? _unary;
    private IDictionary<int, int>? _platform;

    [Params(1, 10, 1000)]
    public int Repetitions;
    
    [GlobalSetup]
    public void SetUp()
    {
        _unary = new UnaryDictionary<int, int>(1, 10);
        _platform = new Dictionary<int, int> { { 1, 10 } };
    }
    
    [Benchmark(Baseline = true)]
    public int PlatformLookup() => Lookup(_platform!, Repetitions);
    
    [Benchmark]
    public int UnaryLookup() => Lookup(_unary!, Repetitions);
    
    private static int Lookup(IDictionary<int, int> dictionary, int repetitions)
    {
        int sum = 0;
        
        for (int i = 0; i < repetitions; i++) 
        {
            sum += dictionary[1];
        }

        return sum;
    }
}