using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 2)]
public class StartsWithComparison
{
    [Params("hello", "Hello")]
    public string? Input;

    [Benchmark(Baseline = true)]
    public void RunOldCode()
    {
        var x = Input.StartStringWithLowerCaseOld();
    }
    
    [Benchmark]
    public void RunNewCode()
    {
        var x = Input.StartStringWithLowerCaseNew();
    }
    
    [Benchmark]
    public void RunNewCodeVariant()
    {
        Input.StartStringWithLowerCaseNewer();
        var x = Input.StartStringWithLowerCaseNewer();
    }
}

public static class NewExtension {
    
    public static string StartStringWithLowerCaseOld(this string? s) => TransformFirstChar(s, char.ToLower);
    
    private static string TransformFirstChar(string? s, Func<char, char> transform)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        var a = s.ToCharArray();
        a[0] = transform(a[0]);
        return new string(a);
    }
    
    public static string StartStringWithLowerCaseNew(this string? s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
		
        var buffer = s.Length <= 256 ? stackalloc char[s.Length] : new char[s.Length];
        s.CopyTo(buffer);
        buffer[0] = char.ToLower(buffer[0]);

        return buffer.ToString();
    }
    
    public static string StartStringWithLowerCaseNewer(this string? s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return string.Create(s.Length, s, static (span, value) =>
        {
            value.CopyTo(span);
            span[0] = char.ToLower(span[0]);
        });
    }
}