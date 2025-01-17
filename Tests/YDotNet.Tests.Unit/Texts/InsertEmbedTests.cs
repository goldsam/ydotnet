using NUnit.Framework;
using YDotNet.Document;
using YDotNet.Document.Cells;
using YDotNet.Document.Transactions;
using YDotNet.Document.Types.Texts;

namespace YDotNet.Tests.Unit.Texts;

public class InsertEmbedTests
{
    [Test]
    public void InsertBooleanEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(transaction, index: 3, Input.Boolean(value: true));

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Boolean, Is.True);
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertDoubleEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(transaction, index: 3, Input.Double(value: 24.69));

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Double, Is.EqualTo(expected: 24.69));
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertLongEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(transaction, index: 3, Input.Long(value: 2469));

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.EqualTo(expected: 2469));
        Assert.That(chunks.ElementAt(index: 1).Data.Double, Is.Null);
    }

    [Test]
    public void InsertStringEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(transaction, index: 3, Input.String("Between"));

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.String, Is.EqualTo("Between"));
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertBytesEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(transaction, index: 3, Input.Bytes(new byte[] { 2, 4, 6, 9 }));

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Bytes, Is.EqualTo(new byte[] { 2, 4, 6, 9 }));
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertCollectionEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(
            transaction, index: 3, Input.Collection(
                new[]
                {
                    Input.Boolean(value: true),
                    Input.Boolean(value: false)
                }));

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Collection.Length, Is.EqualTo(expected: 2));
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertObjectEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(
            transaction, index: 3, Input.Object(
                new Dictionary<string, Input>
                {
                    { "italics", Input.Boolean(value: true) }
                }));

        // Assert
        var chunks = text.Chunks(transaction);
        var secondChunk = chunks.ElementAt(index: 1).Data.Object;

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(secondChunk.Count, Is.EqualTo(expected: 1));
        Assert.That(secondChunk.Keys.First(), Is.EqualTo("italics"));
        Assert.That(secondChunk.Values.First().Boolean, Is.True);
        Assert.That(secondChunk.Values.First().Long, Is.Null);
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertNullEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(transaction, index: 3, Input.Null());

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Null, Is.True);
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertUndefinedEmbed()
    {
        // Arrange
        var (text, transaction) = ArrangeText();

        // Act
        text.InsertEmbed(transaction, index: 3, Input.Undefined());

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Undefined, Is.True);
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
    }

    [Test]
    public void InsertBooleanEmbedWithAttributes()
    {
        // Arrange
        var (text, transaction) = ArrangeText();
        var attributes = new Dictionary<string, Input>
        {
            { "bold", Input.Boolean(value: true) }
        };

        // Act
        text.InsertEmbed(transaction, index: 3, Input.Boolean(value: true), Input.Object(attributes));

        // Assert
        var chunks = text.Chunks(transaction);

        Assert.That(chunks.Length, Is.EqualTo(expected: 3));
        Assert.That(chunks.ElementAt(index: 1).Data.Boolean, Is.True);
        Assert.That(chunks.ElementAt(index: 1).Data.Long, Is.Null);
        Assert.That(chunks.ElementAt(index: 1).Attributes.Count(), Is.EqualTo(expected: 1));
        Assert.That(chunks.ElementAt(index: 1).Attributes.First().Key, Is.EqualTo("bold"));
        Assert.That(chunks.ElementAt(index: 1).Attributes.First().Value.Boolean, Is.True);
        Assert.That(chunks.ElementAt(index: 1).Attributes.First().Value.Long, Is.Null);
    }

    private (Text, Transaction) ArrangeText()
    {
        var doc = new Doc();
        var text = doc.Text("value");

        var transaction = doc.WriteTransaction();
        text.Insert(transaction, index: 0, "Lucas");

        return (text, transaction);
    }
}
