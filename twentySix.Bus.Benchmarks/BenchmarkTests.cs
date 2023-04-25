using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace twentySix.Bus.Benchmarks;

[MemoryDiagnoser]
public class BenchmarkTests
{
    private const int NumberOfTrials = 1000;
    private const string StringValue = "test";
    private const int IntValue = 1;
    private readonly Action<string> _actionValue = _ => {};

    private EventBus.EventBus _bus;
    public static void Main()
    {
        BenchmarkRunner.Run<BenchmarkTests>();
    }
    
    [GlobalSetup]
    public void Setup()
    {
        _bus = new();
        _bus.Subscribe<int>(this, _ => { });
    }
    
    [Benchmark]
    public void Subscription_Unsubscription()
    {
        for (var i = 0; i < NumberOfTrials; i++)
        {
            _bus.Subscribe(this, _actionValue);
            _bus.Unsubscribe(this, _actionValue);
        }
    }
    
    [Benchmark]
    public void Subscription_Send_Unsubscription()
    {
        for (var i = 0; i < NumberOfTrials; i++)
        {
            _bus.Subscribe(this, _actionValue);
            _bus.Send(StringValue);
            _bus.Unsubscribe(this, _actionValue);
        }
    }
    
    [Benchmark]
    public void Send()
    {
        for (var i = 0; i < NumberOfTrials; i++)
        {
            _bus.Send(IntValue);
        }
    }
}