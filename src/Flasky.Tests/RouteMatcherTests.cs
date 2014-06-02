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
            var routeHandlers = new Dictionary<RouteBase, Func<OwinRequest, object>>();
            routeHandlers.Add(new RegexRoute("foo/"), r => { return "test-response"; });

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";

            Assert.That(matcher.HasMatch(request), Is.True);
        }

        [Test]
        public void When_matcher_is_asked_to_return_a_request_handler_that_is_set_up_that_handler_is_returned()
        {
            const string testResponse = "test-response";

            var routeHandlers = new Dictionary<RouteBase, Func<OwinRequest, object>>
                                    {
                                        {
                                            new RegexRoute("foo/"), r =>
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
            var routeHandlers = new Dictionary<RouteBase, Func<OwinRequest, object>>();

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";

            Assert.That(matcher.HasMatch(request), Is.False);
        }


        [Test]
        public void When_matcher_is_asked_to_return_a_request_handler_that_is_not_set_up_null_is_returned()
        {
            var matcher = new RouteMatcher(new Dictionary<RouteBase, Func<OwinRequest, object>> { });

            var request = OwinRequest.Create();
            request.Path = "foo/";

            Assert.That(matcher.HasMatch(request), Is.False);
            var handler = matcher.GetMatch(request);
            Assert.That(handler, Is.Null);
        }

        [Test] 
        public void When_matcher_is_asked_to_match_a_path_with_and_method_that_is_not_set_up_no_match_is_found()
        {
            var routeHandlers = new Dictionary<RouteBase, Func<OwinRequest, object>>();
            routeHandlers.Add(new RegexRoute("foo/", "GET"), r => { return "test-response"; });

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

            var routeHandlers = new Dictionary<RouteBase, Func<OwinRequest, object>>
                                    {
                                        {new RegexRoute("foo/"), r => { return "test-response-default"; }},
                                        {new RegexRoute("foo/", "GET"), r => { return "test-response-get"; }},
                                        {new RegexRoute("foo/", "POST"), r => { return "test-response-post"; }}
                                    };

            var matcher = new RouteMatcher(routeHandlers);

            var request = OwinRequest.Create();
            request.Path = "foo/";
            request.Method = "POST";

            Assert.That(matcher.HasMatch(request), Is.True);
            Assert.That(matcher.GetMatch(request)(request).ToString(), Is.EqualTo("test-response-post"));
        }

        [Test]
        public void When_two_args_in_route_expect_all_to_be_present_in_order_to_match()
        {
            var route = new RegexRoute("/<one>/<two>");
            Assert.That(route.HasMatch("/one/two"));
            Assert.That(route.HasMatch("/one"), Is.False);
        }

        [Test]
        public void Test_paths()
        {
            var routeHandlers = new Dictionary<RouteBase, Func<OwinRequest, object>>
                                    {
                                        {new RegexRoute("/"), r => { return "default"; }},
                                        {new RegexRoute("/special"), r => { return "special"; }},
                                        {new RegexRoute("/<name1>/silly/<name2>"), r => { return "sillypage"; }},
                                        {new RegexRoute("/<name1>/silly/<name2>/edit"), r => { return "editsillypage"; }},
                                        {new RegexRoute("/Talk:<name*>"), r => { return "talk"; }},
                                        {new RegexRoute("/User:<username>"), r => { return "user"; }},
                                        {new RegexRoute("/User:<username>/<name*>"), r => { return "userpage"; }},
                                        {new RegexRoute("/<year>"), r => { return "date"; }},
                                        {new RegexRoute("/Files/<file*>"), r => { return "files"; }},
                                        {new RegexRoute("/<name*>/edit"), r => { return "editpage"; }},
                                        {new RegexRoute("/<name*>"), r => { return "post"; }}
                                    };
            var matcher = new RouteMatcher(routeHandlers);

            AssertPathMatches(matcher, "/" , "default");
            AssertPathMatches(matcher, "/Special", "special");
            AssertPathMatches(matcher, "/2014", "date");
            AssertPathMatches(matcher, "/Some/Page", "post");
            AssertPathMatches(matcher, "/Some/Page/edit", "editpage");
            AssertPathMatches(matcher, "/Foo/silly/bar", "sillypage");
            AssertPathMatches(matcher, "/Foo/silly/bar/edit", "editsillypage");
            AssertPathMatches(matcher, "/Talk:Foo/Bar", "talk");
            AssertPathMatches(matcher, "/User:thomas", "user");
            AssertPathMatches(matcher, "/User:thomas/projects/werkzeug", "userpage");
            AssertPathMatches(matcher, "/Files/downloads/werkzeug/0.2.zip", "files");
        }

        private static void AssertPathMatches(RouteMatcher matcher, string path, string expectedResponse)
        {
            var request = OwinRequest.Create();
            request.Path = path;
            
            var match = matcher.GetMatch(request);
            Assert.That(match(request).ToString(), Is.EqualTo(expectedResponse), "invalid match for path:" + path);
        }
    }
}
