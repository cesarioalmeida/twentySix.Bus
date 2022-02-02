namespace twentySix.EventBus.Internal;

internal abstract class ActionInvokerBase : IActionInvoker
{
    private WeakReference _targetReference;

    public object Target => _targetReference.With(x => x.Target);

    protected abstract string MethodName { get; }

    public ActionInvokerBase(object target) => _targetReference = target is null ? null : new WeakReference(target);

    void IActionInvoker.ExecuteIfMatch(Type messageTargetType, object parameter)
    {
        var target = Target;
        
        if (target is not null && (messageTargetType is null || messageTargetType.IsInstanceOfType(target)))
        {
            Execute(parameter);
        }
    }

    void IActionInvoker.ClearIfMatch(Delegate action, object recipient)
    {
        var target = Target;
        
        if (recipient == target && ((object)action is null || action.Method.Name == MethodName))
        {
            _targetReference = null;
            ClearCore();
        }
    }

    protected abstract void Execute(object parameter);

    protected abstract void ClearCore();
}
