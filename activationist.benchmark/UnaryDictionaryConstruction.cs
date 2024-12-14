using System.Diagnostics.CodeAnalysis;
using activationist.app.Stuff;
using BenchmarkDotNet.Attributes;

namespace activationist.benchmark;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class UnaryDictionaryConstruction
{
    private static readonly IEqualityComparer<int> Comparer = EqualityComparer<int>.Default;

    [Benchmark(Baseline = true)]
    public IDictionary<int, int>  PlatformConstruction() => new Dictionary<int, int>(Comparer) { { 1, 10 } };    

    [Benchmark]
    public IDictionary<int, int> UnaryConstruction() => new UnaryDictionary<int, int>(1, 1000);
}