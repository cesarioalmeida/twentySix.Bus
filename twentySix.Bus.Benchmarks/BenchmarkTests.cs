using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace twentySix.Bus.Benchmarks;

[MemoryDiagnoser]
public class BenchmarkTests
{
    private const int NumberOfTrials = 1000;
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
        var sub = new Action<string>(_ => {});

        for (var i = 0; i < NumberOfTrials; i++)
        {
            _bus.Subscribe(this, sub);
            _bus.Unsubscribe(this, sub);
        }
    }
    
    [Benchmark]
    public void Subscription_Send_Unsubscription()
    {
        var sub = new Action<string>(_ => {});

        for (var i = 0; i < NumberOfTrials; i++)
        {
            _bus.Subscribe(this, sub);
            _bus.Send("test");
            _bus.Unsubscribe(this, sub);
        }
    }
    
    [Benchmark]
    public void Send()
    {
        for (var i = 0; i < NumberOfTrials; i++)
        {
            _bus.Send(1);
        }
    }
}