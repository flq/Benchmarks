using System.Collections.Frozen;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 2)]
public class SwitchVsDelegateDict
{
    private static readonly Dictionary<string, Action> hardCodedDict = new()
    {
        { "a", SomeClass.CallForA },
        { "b", SomeClass.CallForB },
        { "c", SomeClass.CallForC },
        { "d", SomeClass.CallForD },
        { "e", SomeClass.CallForE },
        { "f", SomeClass.CallForF }
    };
    
    private static readonly Dictionary<string, Action> reflectionBuildDict = typeof(SomeClass).GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => m.Name.StartsWith("CallFor"))
        .ToDictionary(
            k => k.Name.Substring(7).ToLower(),
            v => (Action) Delegate.CreateDelegate(typeof(Action), v));

    private static readonly FrozenDictionary<string, Action> frozenDict = reflectionBuildDict.ToFrozenDictionary();
    
    [Params("a", "b", "c", "d", "e", "f")]
    public string Identifier;

    [Benchmark(Baseline = true)]
    public void RunSwitchStatement()
    {
        switch (Identifier)
        {
            case "a":
                SomeClass.CallForA();
                break;
            case "b":
                SomeClass.CallForB();
                break;
            case "c":
                SomeClass.CallForC();
                break;
            case "d":
                SomeClass.CallForD();
                break;
            case "e":
                SomeClass.CallForE();
                break;
            case "f":
                SomeClass.CallForF();
                break;
        }
    }
    
    [Benchmark]
    public void RunDelegateDictionary()
    {
        hardCodedDict[Identifier]();
    }
    
    [Benchmark]
    public void RunReflectionBuiltDictionary()
    {
        reflectionBuildDict[Identifier]();
    }
    
    [Benchmark]
    public void RunReflectionBuiltFrozenDictionary()
    {
        frozenDict[Identifier]();
    }


}

public class SomeClass
{
    public static void CallForA() {}
    public static void CallForB() {}
    public static void CallForC() {}
    public static void CallForD() {}
    public static void CallForE() {}
    public static void CallForF() {}
}