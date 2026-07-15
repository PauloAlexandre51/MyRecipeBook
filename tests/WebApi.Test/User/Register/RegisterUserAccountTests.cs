using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.Register;

public class RegisterUserAccountTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/api/users";

    public RegisterUserAccountTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responeData = await JsonDocument.ParseAsync(responseBody);

        responeData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responeData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNull();

        var user = await DbContext.Users.FirstOrDefaultAsync(user => user.Name == request.Name);

        var userExists = await DbContext.Users.AnyAsync(user => 
        user.Active 
        && user.Name.Equals(request.Name) 
        && user.Email.Equals(request.Email));

        userExists.ShouldBeTrue();
    }

    [Fact]
    public async Task Validade_ShouldBeAnErrorResponse_WhenNameIsEmpty()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responeData = await JsonDocument.ParseAsync(responseBody);

        var errors = responeData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error =>
            error.GetString().IsNotEmpty() &&
            error.GetString()!.Equals(ResourceMessagesException.NAME_REQUIRED));
        });

        var userExists = await DbContext.Users.AnyAsync(user =>
        user.Active
        && user.Name.Equals(request.Name)
        && user.Name.Equals(request.Email));

        userExists.ShouldBeFalse();
    }
}