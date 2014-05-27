using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flasky.Response;
using NSubstitute;
using NUnit.Framework;
using Owin.Types;

namespace Flasky.Tests.Response
{
    [TestFixture, Category("Response")]
    public class JsonResponseTests
    {
        [Test]
        public void When_write_is_called_with_a_valid_object_expect_owinresponse_write_async_is_called()
        {
            var objectToWrite = new { Foo = "bar" };

            var response = objectToWrite.AsJsonResponse();

            Assert.Fail();
        }
    }
}
