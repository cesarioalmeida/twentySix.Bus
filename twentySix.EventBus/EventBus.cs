using twentySix.EventBus.Interfaces;
using twentySix.EventBus.Internal;

namespace twentySix.EventBus;

public class EventBus : IEventBus
{
    private readonly ActionInvokerCollection _actions = new();

    public void Subscribe<TMessage>(object recipient, Action<TMessage> action)
    {
        try
        {
            Monitor.Enter(_actions);

            var actionInvoker = new WeakReferenceActionInvoker<TMessage>(recipient, action);
            RegisterCore(typeof(TMessage), actionInvoker);
        }
        finally
        {
            Monitor.Exit(_actions);
        }
    }

    public void Unsubscribe(object recipient)
        => UnregisterCore(recipient, null, null);

    public void Unsubscribe<TMessage>(object recipient, Action<TMessage> action)
        => UnregisterCore(recipient, action, typeof(TMessage));

    public void Send<TMessage>(TMessage message, Type messageTargetType)
    {
        try
        {
            Monitor.Enter(_actions);

            _actions.Send(message, messageTargetType, typeof(TMessage));
            _actions.CleanUp();
        }
        finally
        {
            Monitor.Exit(_actions);
        }
    }

    public void Send<TMessage>(TMessage message) => Send(message, null);

    public void Send<TMessage, TTarget>(TMessage message) => Send(message, typeof(TTarget));

    private void RegisterCore(Type messageType, IActionInvoker actionInvoker)
        => _actions.Register(messageType, actionInvoker);

    private void UnregisterCore(object recipient, Delegate action, Type messageType)
    {
        try
        {
            Monitor.Enter(_actions);

            _actions.Unregister(recipient, action, messageType);
            _actions.CleanUp();
        }
        finally
        {
            Monitor.Exit(_actions);
        }
    }
}