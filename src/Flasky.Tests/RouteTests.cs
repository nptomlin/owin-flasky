using NUnit.Framework;

namespace Flasky.Tests
{
    [TestFixture]
    public class RouteTests
    {
        [Test]
        public void When_route_path_same_as_request_path_match_is_successful()
        {
            var route = new RegexRoute("foo/");
            Assert.That(route.HasMatch("foo/"), Is.True);
        }

        [Test]
        public void When_route_path_different_to_request_path_match_is_not_successful()
        {
            var route = new RegexRoute("foo/");
            Assert.That(route.HasMatch("bar/"), Is.False);
        }

        [Test]
        public void When_route_path_contains_regex_and_path_that_matches_regex_match_is_successful()
        {
            var route = new RegexRoute(@"foo/(?<foo>[A-Za-z0-9\-]+)/");
            Assert.That(route.HasMatch("foo/bar/"), Is.True);
        }
    }
}