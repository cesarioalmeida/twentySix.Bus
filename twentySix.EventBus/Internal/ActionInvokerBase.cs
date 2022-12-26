using System.Runtime.InteropServices;

namespace twentySix.EventBus.Internal;

internal abstract class ActionInvokerBase : IActionInvoker
{
    private WeakReference _targetReference;

    public object Target => _targetReference.With(x => x.Target);

    protected abstract string MethodName { get; }

    protected ActionInvokerBase(object target) => _targetReference = target is null ? null : new WeakReference(target);

    void IActionInvoker.ExecuteIfMatch(Type messageTargetType, object parameter)
    {
        if (Target is not null && (messageTargetType is null || messageTargetType.IsInstanceOfType(Target)))
        {
            Execute(parameter);
        }
    }

    void IActionInvoker.ClearIfMatch(Delegate action, object recipient)
    {
        if (recipient != Target || ((object) action is not null && action.Method.Name != MethodName))
        {
            return;
        }
        
        _targetReference = null;
        ClearCore();
    }

    protected abstract void Execute(object parameter);

    protected abstract void ClearCore();
}