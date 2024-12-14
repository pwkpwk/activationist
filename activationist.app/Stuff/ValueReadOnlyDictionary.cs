using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace activationist.app.Stuff;

public readonly struct ValueReadOnlyDictionary<TKey, TValue>(UnaryDictionary<TKey, TValue> dictionary)
    : IReadOnlyDictionary<TKey, TValue>
{
    private readonly IDictionary<TKey, TValue> _dictionary = dictionary;

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
        _dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => _dictionary.Count;

    bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
        _dictionary.TryGetValue(key, out value);

    TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => _dictionary[key];

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _dictionary.Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _dictionary.Values;
}