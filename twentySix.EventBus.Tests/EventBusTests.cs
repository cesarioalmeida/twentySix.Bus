using System;
using NUnit.Framework;
using twentySix.EventBus.Interfaces;

namespace twentySix.EventBus.Tests;

public class EventBusTests
{
    private IEventBus _target;
    
    [SetUp]
    public void Setup()
    {
        _target = new EventBus();
    }

    [Test]
    public void Subscribe_Send_Receives()
    {
        var result = false;
        
        _target.Subscribe<string>(this, s => result = s.Equals("yes"));
        _target.Send("yes");
        
        Assert.IsTrue(result);
    }
    
    [Test]
    public void Subscribe_2Subscriptions_Receives()
    {
        var result1 = false;
        var result2 = false;
        
        _target.Subscribe<string>(this, s => result1 = s.Equals("yes"));
        _target.Subscribe<string>(this, s => result2 = s.Equals("yes"));
        _target.Send("yes");
        
        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
    }
    
    [Test]
    public void Subscribe_SubscriptionToDifferentMessages_DoesNotReceive()
    {
        var result = false;

        _target.Subscribe<int>(this, s => result = s.Equals(1));
        _target.Send("yes");
        
        Assert.IsFalse(result);
    }
    
    [Test]
    public void Subscribe_2Subscriptions1MessageType_Receives()
    {
        var result1 = false;
        var result2 = false;
        
        _target.Subscribe<string>(this, s => result1 = s.Equals("yes"));
        _target.Subscribe<int>(this, s => result2 = s.Equals(1));
        _target.Send(1);
        
        Assert.IsFalse(result1);
        Assert.IsTrue(result2);
    }
    
    [Test]
    public void Subscribe_NoSubscriptions_DoesNotReceive()
    {
        _target.Send("yes");
        Assert.Pass();
    }
    
    [Test]
    public void Unsubscribe_TwoSubscriptions_DoesNotReceive()
    {
        var result1 = false;
        var result2 = false;
        
        void IntSubscription(int s) => result2 = s.Equals(1);

        _target.Subscribe<string>(this, s => result1 = s.Equals("yes"));
        _target.Subscribe(this, (Action<int>) IntSubscription);
        
        _target.Unsubscribe(this);
        
        _target.Send("yes");
        _target.Send(1);
        
        Assert.IsFalse(result1);
        Assert.IsFalse(result2);
    }
    
    [Test]
    public void Unsubscribe_OneSubscriptions_ReceivesOne()
    {
        var result1 = false;
        var result2 = false;
        
        var intSubscription = new Action<int>(s => result2 = s.Equals(1));

        _target.Subscribe<string>(this, s => result1 = s.Equals("yes"));
        _target.Subscribe(this, intSubscription);
        
        _target.Unsubscribe(this, intSubscription);
        
        _target.Send("yes");
        _target.Send(1);
        
        Assert.IsTrue(result1);
        Assert.IsFalse(result2);
    }
    
    [Test]
    public void Unsubscribe_MultipleClasses_OtherReceives()
    {
        var result1 = false;
        var result2 = false;
        void StringAction1(string s) => result1 = s.Equals("yes");
        void StringAction2(string s) => result2 = s.Equals("yes");
        var secondTarget = new Test2 { Bus = _target };
        
        _target.Subscribe(this, (Action<string>) StringAction1);
        secondTarget.Bus.Subscribe(this, (Action<string>) StringAction2);
        _target.Unsubscribe(this, (Action<string>) StringAction1);
        
        _target.Send("yes");

        Assert.IsFalse(result1);
        Assert.IsTrue(result2);
    }

    private class Test2
    {
        public IEventBus Bus { get; set; }
    }
}