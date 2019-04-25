using System;
using Moq;
using RestSharp;
using Xunit;

namespace ekin.restsharp.wrapper.unittests
{
    public class RestSharpProxyTest
    {
        readonly Mock<IRestClient> _restClientMock;

        readonly RestSharpProxy _sut;

        public RestSharpProxyTest()
        {
            _restClientMock = new Mock<IRestClient>();


            _restClientMock.Setup(s => s.Execute<TestResponse>(It.IsAny<RestRequest>(), It.IsAny<Method>()))
                .Returns(()=> new RestResponse<TestResponse>()
                {
                    Data = new TestResponse
                    {
                        Id = 1,
                        Name = "name"
                    }
                });

            _sut = new RestSharpProxy(_restClientMock.Object);
        }

        [Fact]
        public void If_RestSharpProxyRequest_Url_IsNull_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(()=> _sut.Execute<object, TestResponse>(new RestSharpProxyRequest<object>{}));
        }

        [Fact]
        public void If_RestSharpProxyRequest_Url_IsEmpty_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Execute<object, TestResponse>(new RestSharpProxyRequest<object> { Url = string.Empty }));
        }

        [Fact]
        public void If_RestSharpProxyRequest_IsValid_MakeRequest()
        {
            _sut.Execute<object, TestResponse>(ValidRequestSample);

            _restClientMock.Verify(s => s.Execute<TestResponse>(It.IsAny<RestRequest>(), It.IsAny<Method>()), Times.Once);
        }

        [Fact]
        public void If_RestResponse_Data_IsNull_Throw_ArgumentNullException()
        {
            _restClientMock.Setup(s => s.Execute<TestResponse>(It.IsAny<RestRequest>(), It.IsAny<Method>()))
                .Returns(() => new RestResponse<TestResponse>()
                {
                    Data = null
                });

            Assert.Throws<ArgumentNullException>(()=> _sut.Execute<object, TestResponse>(ValidRequestSample));
        }

        private RestSharpProxyRequest<object> ValidRequestSample { get; } = new RestSharpProxyRequest<object>
        {
            Url = "http://url.com",
            Resource = "resourse/{id}",
            Headers = new System.Collections.Generic.Dictionary<string, string>()
            {
                {"aa", "aa"}
            },
            Parameters = new System.Collections.Generic.Dictionary<string, string>()
            {
                {"params", "params"}
            },
            Body = new
            {
                Id = 5,
                Name = "aaa"
            },
            Method = EnumHttpMethod.GET
        };
    }

    class TestResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}