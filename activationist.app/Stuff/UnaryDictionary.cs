using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace activationist.app.Stuff;

public sealed class UnaryDictionary<TKey, TValue>(
    TKey key,
    TValue value,
    IEqualityComparer<TKey>? keyComparer = null) : IDictionary<TKey, TValue>
{
    private readonly ValueCollection<TKey> _keys = new(key, keyComparer);
    private readonly ValueCollection<TValue> _values = new(value, null);

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
        new UnaryEnumerator<KeyValuePair<TKey, TValue>>(new(_keys.Value, _values.Value));

    IEnumerator IEnumerable.GetEnumerator() =>
        new UnaryEnumerator<KeyValuePair<TKey, TValue>>(new(_keys.Value, _values.Value));

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) =>
        throw new InvalidOperationException();

    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw new InvalidOperationException();

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) =>
        _keys.Contains(item.Key) && _values.Contains(item.Value);

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
        throw new NotImplementedException();

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) =>
        throw new InvalidOperationException();

    int ICollection<KeyValuePair<TKey, TValue>>.Count => 1;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new InvalidOperationException();

    bool IDictionary<TKey, TValue>.ContainsKey(TKey key) => _keys.Contains(key);

    bool IDictionary<TKey, TValue>.Remove(TKey key) => throw new InvalidOperationException();

    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (!_keys.Contains(key))
        {
            value = default;
            return false;
        }

        value = _values.Value;
        return true;
    }

    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
        get => _keys.Contains(key) ? _values.Value : throw new KeyNotFoundException();
        set => throw new InvalidOperationException();
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => _keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values => _values;

    private readonly struct ValueCollection<T>(T value, IEqualityComparer<T>? comparer) : ICollection<T>
    {
        private static readonly IEqualityComparer<T> DefaultComparer = EqualityComparer<T>.Default;
        private readonly IEqualityComparer<T> _comparer = comparer ?? DefaultComparer;
        public readonly T Value = value;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new UnaryEnumerator<T>(Value);

        IEnumerator IEnumerable.GetEnumerator() => new UnaryEnumerator<T>(Value);

        void ICollection<T>.Add(T item) => throw new InvalidOperationException();

        void ICollection<T>.Clear() => throw new InvalidOperationException();

        public bool Contains(T item) => _comparer.Equals(item, Value);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException();

        int ICollection<T>.Count => 1;

        bool ICollection<T>.IsReadOnly => true;
    }

    private sealed class UnaryEnumerator<T>(T value) : IEnumerator<T>
    {
        private bool _exhausted;

        public void Dispose()
        {
        }

        bool IEnumerator.MoveNext() => !_exhausted && (_exhausted = true);

        void IEnumerator.Reset()
        {
            _exhausted = false;
        }

        T IEnumerator<T>.Current => GetCurrent();

        object? IEnumerator.Current => GetCurrent();

        private T GetCurrent() => _exhausted ? value : throw new InvalidOperationException();
    }
}