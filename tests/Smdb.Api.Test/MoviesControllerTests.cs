using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Moq;
using Shared.Http;
using Smdb.Api.Movies;
using Smdb.Core.Movies;
using Xunit;

namespace Smdb.Api.Test;

public class TestHttpListenerRequest
{
    public NameValueCollection QueryString { get; set; } = new NameValueCollection();
}

public class TestHttpListenerResponse
{
    public void Redirect(string url) { }
}

public class MoviesControllerTests
{
    [Fact]
    public void MoviesController_CanBeInstantiated()
    {
        var service = Mock.Of<IMovieService>();
        var controller = new MoviesController(service);
        Assert.NotNull(controller);
    }
}
