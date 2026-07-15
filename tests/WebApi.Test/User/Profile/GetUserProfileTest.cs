using Org.BouncyCastle.Asn1.Ocsp;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.Test.Resources;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/users";
    private readonly UserIdentityManager _user1;
    public GetUserProfileTest(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var response = await Get(REQUEST_URI, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responeData = await JsonDocument.ParseAsync(responseBody);

        responeData.RootElement.GetProperty("name").GetString().ShouldBe(_user1.GetName());
        responeData.RootElement.GetProperty("email").GetString().ShouldBe(_user1.GetEmail());
    }
}
