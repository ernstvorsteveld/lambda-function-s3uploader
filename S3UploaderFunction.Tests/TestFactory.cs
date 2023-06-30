using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace S3UploaderFunction.Tests
{
    public class TestFactory
    {
        public static IEnumerable<object[]> Data()
        {
            return new List<object[]>
            {
                new object[] {"name", "Bill"},
                new object[] {"name", "Paul"},
                new object[] {"name", "Steve"}
            };
        }

        private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
        {
            var qs = new Dictionary<string, StringValues>
            {
                {key, value}
            };
            return qs;
        }

        public static HttpRequest CreateHttpRequest(string image)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            if (image != "")
            {
                request.Body = new MemoryStream(Encoding.ASCII.GetBytes(image));
            }

            return request;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}