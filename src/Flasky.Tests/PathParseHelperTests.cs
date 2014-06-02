using NUnit.Framework;

namespace Flasky.Tests
{
    [TestFixture]
    public class PathParseHelperTests
    {
        [Test]
        public void When_there_is_a_static_segment_followed_by_argument_segment_expect_two_tuples()
        {
            var vals = PathParseHelper.Prepare("/foo/<bar>");
            var enumerator = vals.GetEnumerator();

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.Null);
            Assert.That(enumerator.Current.Item2, Is.EqualTo("/foo/"));

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.EqualTo("default"));
            Assert.That(enumerator.Current.Item2, Is.EqualTo("bar"));

            Assert.That(enumerator.MoveNext(), Is.False);

        }

        [Test]
        public void When_there_is_a_slash_followed_by_argument_segment_followed_by_static_segment_expect_three_tuples()
        {
            var vals = PathParseHelper.Prepare("/<foo>/bar");
            var enumerator = vals.GetEnumerator();

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.Null);
            Assert.That(enumerator.Current.Item2, Is.EqualTo("/"));

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.EqualTo("default"));
            Assert.That(enumerator.Current.Item2, Is.EqualTo("foo"));

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.Null);
            Assert.That(enumerator.Current.Item2, Is.EqualTo("/bar"));

            Assert.That(enumerator.MoveNext(), Is.False);
        }

        [Test]
        public void When_there_is_a_slash_followed_by_path_argument_segment_followed_by_static_segment_expect_three_tuples()
        {
            var vals = PathParseHelper.Prepare("/<path:foo>/bar");
            var enumerator = vals.GetEnumerator();

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.Null);
            Assert.That(enumerator.Current.Item2, Is.EqualTo("/"));

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.EqualTo("path"));
            Assert.That(enumerator.Current.Item2, Is.EqualTo("foo"));

            Assert.That(enumerator.MoveNext());
            Assert.That(enumerator.Current.Item1, Is.Null);
            Assert.That(enumerator.Current.Item2, Is.EqualTo("/bar"));

            Assert.That(enumerator.MoveNext(), Is.False);
        }

    }
}