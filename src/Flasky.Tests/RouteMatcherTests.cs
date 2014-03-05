using System;
using System.Collections.Generic;
using NUnit.Framework;
using Owin.Types;

namespace Flasky.Tests
{
    [TestFixture]
    public class RouteMatcherTests
    {

        [Test]
        public void When_matcher_is_asked_to_match_a_path_that_is_set_up_match_is_found()
        {
            var routeHandlers = new Dictionary<Route, Func<OwinRequest, object>>();
            routeHandlers.Add(new Route("foo/"), r => { return "test-response"; });

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";

            Assert.That(matcher.HasMatch(request), Is.True);
        }

        [Test]
        public void When_matcher_is_asked_to_return_a_request_handler_that_is_set_up_that_handler_is_returned()
        {
            const string testResponse = "test-response";

            var routeHandlers = new Dictionary<Route, Func<OwinRequest, object>>
                                    {
                                        {
                                            new Route("foo/"), r =>
                                                        {
                                                            return testResponse;
                                                        }
                                        }
                                    };

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";

            Assert.That(matcher.HasMatch(request), Is.True);
            var handler = matcher.GetMatch(request);
            Assert.That(handler(request).ToString(), Is.EqualTo(testResponse));
        }


        [Test]
        public void When_matcher_is_asked_to_match_a_path_that_is_not_set_up_no_match_is_found()
        {
            var routeHandlers = new Dictionary<Route, Func<OwinRequest, object>>();

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";

            Assert.That(matcher.HasMatch(request), Is.False);
        }


        [Test]
        public void When_matcher_is_asked_to_return_a_request_handler_that_is_not_set_up_null_is_returned()
        {
            const string testResponse = "test-response";

            var matcher = new RouteMatcher(new Dictionary<Route, Func<OwinRequest, object>> { });

            var request = OwinRequest.Create();
            request.Path = "foo/";

            Assert.That(matcher.HasMatch(request), Is.False);
            var handler = matcher.GetMatch(request);
            Assert.That(handler, Is.Null);
        }

        [Test] 
        public void When_matcher_is_asked_to_match_a_path_with_and_method_that_is_not_set_up_no_match_is_found()
        {
            var routeHandlers = new Dictionary<Route, Func<OwinRequest, object>>();
            routeHandlers.Add(new Route("foo/", "GET"), r => { return "test-response"; });

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";
            request.Method = "POST";

            Assert.That(matcher.HasMatch(request), Is.False);
        }

        //need levels.  all that match the path together
        //then ordered in terms of specificity
        //the most defined ones to come first and then
        //more permissive ones

        [Test]
        public void When_matcher_is_asked_to_match_a_request_that_has_many_paths_set_up_the_most_specific_route_which_matches_wins()
        {

            var routeHandlers = new Dictionary<Route, Func<OwinRequest, object>>();
            routeHandlers.Add(new Route("foo/"), r => { return "test-response-default"; });
            routeHandlers.Add(new Route("foo/", "GET"), r => { return "test-response-get"; });
            routeHandlers.Add(new Route("foo/", "POST"), r => { return "test-response-post"; });

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";
            request.Method = "POST";

            Assert.That(matcher.HasMatch(request), Is.True);
            Assert.That(matcher.GetMatch(request)(request).ToString(), Is.EqualTo("test-response-post"));
        }

    }
}
