using System.Collections;
using System.Net;
using NSubstitute;
using Shared.Http;
using Xunit;

namespace Smdb.Csr.Test;

public class AppTests
{
    [Fact]
    public void App_CanBeInstantiated()
    {
        var app = new Smdb.Csr.App();
        Assert.NotNull(app);
    }
}
