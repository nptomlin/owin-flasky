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
            var route = new RegexRoute(@"foo/<bar>/");
            Assert.That(route.HasMatch("foo/bar/"), Is.True);
        }

        [Test]
        public void When_route_contains_parameters_parameters_are_populated()
        {
            var route = new RegexRoute(@"foo/<bar>/");

            Assert.That(route.Parameters, Is.Not.Null);
            Assert.That(route.Parameters, Is.Not.Empty);
            Assert.That(route.Parameters.Length, Is.EqualTo(1));
            Assert.That(route.Parameters, Contains.Item("bar"));
        }

        [Test]
        public void When_route_that_has_parameter_has_parameter_queried_expect_value_is_returned()
        {
            var route = new RegexRoute(@"foo/<bar>/");

            Assert.That(route.GetParameterValue("foo/moishere/", "bar"), Is.EqualTo("moishere"));
        }
    }
}