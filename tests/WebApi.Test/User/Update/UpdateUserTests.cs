using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using WebApi.Test.Resources;

namespace WebApi.Test.User.Update;

public class UpdateUserTests : BaseIntegrationTest
{
    private const string UpdateUserEndpoint = "/api/users/profile";
    
    private readonly UserIdentityManager _user1;
    public UpdateUserTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await Put(UpdateUserEndpoint, request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var userExists = await DbContext
            .Users
            .AnyAsync(user => 
            user.Active && user.Id == _user1.GetId() 
            && user.Name == request.Name 
            && user.Email == request.Email);

        userExists.ShouldBeTrue();
    }
}
