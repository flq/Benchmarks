using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(1, 1, 2)]
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

public static class Extension
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? s)
    {
        return s == null || !s.Any();
    }
}