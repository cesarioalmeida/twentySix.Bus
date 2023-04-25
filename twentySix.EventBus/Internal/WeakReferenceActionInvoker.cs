using System.Reflection;

namespace twentySix.EventBus.Internal;

internal class WeakReferenceActionInvoker<T> : ActionInvokerBase
{
    protected MethodInfo ActionMethod { get; private set; }

    protected WeakReference ActionTargetReference { get; private set; }

    protected override string MethodName => ActionMethod.Name;

    public WeakReferenceActionInvoker(object target, Action<T> action)
        : base(target)
    {
        ActionMethod = action.Method;
        ActionTargetReference = new WeakReference(action.Target);
    }
    
    protected override void Execute(object parameter)
    {
        var actionMethod = ActionMethod;
        var target = ActionTargetReference.Target;
        
        if (actionMethod is not null && target is not null)
        {
            actionMethod.Invoke(target, new object[] { (T)parameter });
        }
    }

    protected override void ClearCore()
    {
        ActionTargetReference = null;
        ActionMethod = null;
    }
}