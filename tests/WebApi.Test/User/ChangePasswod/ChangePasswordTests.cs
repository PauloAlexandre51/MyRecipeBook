using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using WebApi.Test.Resources;

namespace WebApi.Test.User.ChangePasswod;

public class ChangePasswordTests : BaseIntegrationTest
{
    private const string ChangePasswordEndpoint = "/api/users/password";

    private readonly UserIdentityManager _user1;
    public ChangePasswordTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = _user1.GetPassword();

        var response = await Put(ChangePasswordEndpoint, request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
