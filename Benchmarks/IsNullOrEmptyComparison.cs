using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 2)]
public class IsNullOrEmptyComparison
{
    
    [Benchmark(Baseline = true)]
    public void Baseline()
    {
        var x = string.IsNullOrEmpty("hello");
    }
    
    [Benchmark]
    public void RunNewCode()
    {
        var x = "hello".IsNullOrEmpty();
    }
}

public static class Extension {
    
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? s) => s == null || !s.Any();
}