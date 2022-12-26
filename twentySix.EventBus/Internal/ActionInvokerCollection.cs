using System.Runtime.InteropServices;

namespace twentySix.EventBus.Internal
{
    internal class ActionInvokerCollection : FuzzyDictionary<Type, List<IActionInvoker>>
    {
        public void Register(Type messageType, IActionInvoker actionInvoker)
        {
            if (!TryGetValue(messageType, out var value))
            {
                value = new List<IActionInvoker>();
                Add(messageType, value);
            }

            value.Add(actionInvoker);
        }

        public void CleanUp()
        {
            foreach (var item in this)
            {
                item.Value.RemoveAll(_ => _.Target is null);

                if (!item.Value.Any())
                {
                    Remove(item.Key);
                }
            }
        }

        public void Unregister(object recipient, Delegate action, Type messageType)
        {
            if (recipient is null)
            {
                return;
            }

            foreach (var value in GetValues(messageType))
            {
                foreach (var item in CollectionsMarshal.AsSpan(value))
                {
                    item.ClearIfMatch(action, recipient);
                }
            }
        }

        public void Send(object message, Type messageTargetType, Type messageType)
        {
            foreach (var value in GetValues(messageType))
            {
                foreach (var item in CollectionsMarshal.AsSpan(value))
                {
                    item.ExecuteIfMatch(messageTargetType, message);
                }
            }
        }
    }
}