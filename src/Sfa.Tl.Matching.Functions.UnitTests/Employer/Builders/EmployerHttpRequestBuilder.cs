using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Sfa.Tl.Matching.Functions.UnitTests.Employer.Builders
{
    public class EmployerHttpRequestBuilder
    {
        public HttpRequest Build(HttpMethod method, string body)
        {
            var request = Substitute.For<HttpRequest>();

            request.Method = method.ToString();

            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();

            stream.Position = 0;

            request.Body.Returns(stream);

            return request;
        }
    }
}
