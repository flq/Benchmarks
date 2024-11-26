using System.Collections.Frozen;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 2)]
public class HashSetVsRegex
{
    [GlobalSetup]
    public void Setup()
    {
        using var file = File.OpenRead("/Users/frankquednau/Documents/repos/ahead/Ahead.Web/wwwroot/emojis/emojis-en.json");
        HashSetValidator.Prepare(new StreamReader(file));
    }
    
    [Benchmark]
    public void HashSetBased()
    {
        var x = HashSetValidator.IsSingleEmoji("‼️");
    }
    
    [Benchmark]
    public void RegexBased()
    {
        var x = RegexEmojiValidator.IsSingleEmoji("‼️");
    }
}

public class HashSetValidator
{
    private static FrozenSet<string>? allowedEmojis;
    
    public static void Prepare(StreamReader streamReader)
    {
        HashSet<string> emojis = new();
        using var reader = new JsonTextReader(streamReader);
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName && reader.Value is "emoji")
            {
                reader.Read();
                emojis.Add(reader.Value.ToString()!);
            }
        }
        allowedEmojis = emojis.ToFrozenSet();
    }
    
    public static bool IsSingleEmoji(string emoji) => 
        allowedEmojis?.Contains(emoji) ?? throw new InvalidOperationException("Emoji set not initialized");
}

public static class RegexEmojiValidator
{
    private static readonly Regex EmojiRegex = new(@"^(?>(?>[\uD800-\uDBFF][\uDC00-\uDFFF](?:\u200D[\uD800-\uDBFF][\uDC00-\uDFFF])*|\p{So}\uFE0F?){1,5})", RegexOptions.Compiled);
	
    public static bool IsSingleEmoji(string potentialEmoji)
    {
        var stringInfo = new System.Globalization.StringInfo(potentialEmoji);
        return stringInfo.LengthInTextElements == 1 && EmojiRegex.IsMatch(potentialEmoji);
    }
}