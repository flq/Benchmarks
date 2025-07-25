using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.ObjectPool;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(1, 1, 2)]
public class ArrayCreationVsArrayPool
{
    private static readonly JsonSerializerOptions NullSkippingSerializationOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly AmplitudePayload payload = new()
    {
        user_id = "user123",
        event_type = "test_event",
        event_properties = new { property1 = "value1", property2 = "value2" },
        groups = new { group1 = "groupValue1" },
        time = 1622547800
    };

    [Benchmark(Baseline = true)]
    public void WithArrayCreation()
    {
        for (int i = 0; i < 1000; i++)
        {
            var body = new
            {
                api_key = "api123",
                events = new[] { payload }
            };

            JsonSerializer.Serialize(body, NullSkippingSerializationOptions);
        }
        
    }
    
    private static readonly BodyPool bodyPool = new();

    [Benchmark]
    public void WithArrayPoolUsage()
    {
        for (int i = 0; i < 1000; i++)
        {
            var body = bodyPool.Get();
            try
            {
                body.events[0] = payload;
                JsonSerializer.Serialize(body, NullSkippingSerializationOptions);
            }
            finally
            {
                bodyPool.Return(body);
            }
        }
    }

    public readonly record struct AmplitudePayload
    {
        public required string user_id { get; init; }
        public required string event_type { get; init; }
        public required object event_properties { get; init; }
        public required object groups { get; init; }
        public long? time { get; init; }

        public static readonly AmplitudePayload Empty = new()
        {
            user_id = string.Empty,
            event_type = string.Empty,
            event_properties = string.Empty,
            groups = string.Empty,
            time = null
        };
    }

    private class BodyPool() : DefaultObjectPool<Body>(new BodyObjectPolicy("api123")); 
    
    public class Body
    {
        public required string api_key { get; init; }
        public AmplitudePayload[] events { get; } = new AmplitudePayload[1];
    }

    private class BodyObjectPolicy(string apiKey) : IPooledObjectPolicy<Body>
    {
        public Body Create() => new() { api_key = apiKey };

        public bool Return(Body obj)
        {
            obj.events[0] = AmplitudePayload.Empty;
            return true;
        }
    }
}



