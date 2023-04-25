namespace twentySix.EventBus.Internal;

internal interface IActionInvoker
{
    object Target { get; }

    void ExecuteIfMatch(Type messageTargetType, object parameter);

    void ClearIfMatch(Delegate action, object recipient);
}