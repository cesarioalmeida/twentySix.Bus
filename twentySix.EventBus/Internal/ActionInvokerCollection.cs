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
            var list = new List<KeyValuePair<Type, List<IActionInvoker>>>();

            foreach (var item in this)
            {
                item.Value.RemoveAll(_ => _.Target is null);
                
                if (item.Value.Count == 0)
                {
                    list.Add(item);
                }
            }

            foreach (var item in list)
            {
                Remove(item.Key);
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
                for (var index = 0; index < value.Count; index++)
                {
                    value[index].ClearIfMatch(action, recipient);
                }
            }
        }

        public void Send(object message, Type messageTargetType, Type messageType)
        {
            foreach (var value in GetValues(messageType))
            {
                for (var index = 0; index < value.Count; index++)
                {
                    value[index].ExecuteIfMatch(messageTargetType, message);
                }
            }
        }
    }
}