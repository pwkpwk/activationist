using activationist.app.Stuff;

namespace activationist.test.Stuff;

[TestFixture]
public class ValueReadOnlyDictionaryTests
{
    [Test]
    public void FromKeyAndValue_CorrectDictionary()
    {
        IReadOnlyDictionary<int, int> dictionary = new ValueReadOnlyDictionary<int, int>(new UnaryDictionary<int, int>(1, 1));
        
        Assert.Multiple(() =>
        {
            Assert.That(dictionary, Has.Count.EqualTo(1));
            Assert.That(dictionary[1], Is.EqualTo(1));
            Assert.That(dictionary.ContainsKey(1), Is.True);
            Assert.That(dictionary.TryGetValue(1, out var value), Is.True);
            Assert.That(value, Is.EqualTo(1));
            Assert.That(dictionary.Values, Is.EqualTo(new[] { 1 }));
            Assert.That(dictionary.Keys, Is.EqualTo(new[] { 1 }));
            Assert.That(dictionary, Is.EquivalentTo(new[] { new KeyValuePair<int, int>(1, 1) }));
        });
    }
    
    [Test]
    public void MissingKey_TryGetValue_ReturnsFalse()
    {
        IReadOnlyDictionary<int, int> dictionary = new ValueReadOnlyDictionary<int, int>(new UnaryDictionary<int, int>(1, 1));
        Assert.Multiple(() =>
        {
            Assert.That(dictionary.TryGetValue(2, out var value), Is.False);
            Assert.That(value, Is.Not.EqualTo(1));
        });
    }
    
    [Test]
    public void MissingKey_Index_ThrowsKeyNotFoundException()
    {
        IReadOnlyDictionary<int, int> dictionary = new ValueReadOnlyDictionary<int, int>(new UnaryDictionary<int, int>(1, 1));
        
        Assert.Throws<KeyNotFoundException>(() => { _ = dictionary[2]; });
    }
}