namespace twentySix.EventBus.Interfaces;

public interface IEventBus
{
    void Subscribe<TMessage>(object recipient, Action<TMessage> action);

    void Unsubscribe(object recipient);

    void Unsubscribe<TMessage>(object recipient, Action<TMessage> action);

    void Send<TMessage>(TMessage message, Type messageTargetType);

    void Send<TMessage>(TMessage message);

    void Send<TMessage, TTarget>(TMessage message);
}