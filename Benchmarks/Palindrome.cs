using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(1, 1, 2)]
[ReturnValueValidator(failOnError: true)]
public class Palindrome
{
    [Params("kayak", "hello", "madam", "rotator", "heratulisasilutareh")]
    public string? Input;

    [Benchmark(Baseline = true)]
    public bool RunFowlerCode()
    {
        return Input.AsSpan().IsPalindromeFowler();
    }

    [Benchmark]
    public bool RunMine()
    {
        return Input.AsSpan().IsPalindrome();
    }
}

public static class Stuff
{
    public static bool IsPalindromeFowler(this ReadOnlySpan<char> s)
    {
        return s switch
        {
            [] => false,
            [var c] => true,
            [var f, var b] => f == b,
            [var f, .. var middle, var b] => f == b && IsPalindromeFowler(middle)
        };
    }
    
    public static bool IsPalindrome(this ReadOnlySpan<char> s)
    {
        var until = s.Length / 2;
        for (var index = 0; index <= until; index++)
        {
            if (s[index] != s[^(index + 1)]) return false;
        }
        return true;
    }
}