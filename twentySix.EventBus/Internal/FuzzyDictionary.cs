using System.Collections;

namespace twentySix.EventBus.Internal;

internal class FuzzyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
	private readonly Dictionary<TKey, TValue> _pairs = new();

	public IEnumerable<TValue> Values => _pairs.Select(_ => _.Value);

	public void Add(TKey key, TValue value) => _pairs.Add(key, value);

	public void Remove(TKey key) => _pairs.Remove(key);

	public bool TryGetValue(TKey key, out TValue value) => _pairs.TryGetValue(key, out value);

	public IEnumerable<TValue> GetValues(TKey key) => key is not null ? GetValuesCore(key) : Values;

	private IEnumerable<TValue> GetValuesCore(TKey key)
	{
		if (_pairs.TryGetValue(key, out var value))
		{
			yield return value;
		}
	}

	IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
	{
		foreach (var (key, value) in _pairs)
		{
			yield return new KeyValuePair<TKey, TValue>(key, value);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		foreach (var (key, value) in _pairs)
		{
			yield return new KeyValuePair<TKey, TValue>(key, value);
		}
	}
}