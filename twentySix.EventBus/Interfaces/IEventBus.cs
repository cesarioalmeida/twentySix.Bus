namespace twentySix.EventBus.Interfaces;

public interface IEventBus
{
    void Register<TMessage>(object recipient, Action<TMessage> action);

    void Unregister(object recipient);

    void Unregister<TMessage>(object recipient, Action<TMessage> action);

    void Send<TMessage>(TMessage message, Type messageTargetType);

    void Send<TMessage>(TMessage message);

    void Send<TMessage, TTarget>(TMessage message);
}