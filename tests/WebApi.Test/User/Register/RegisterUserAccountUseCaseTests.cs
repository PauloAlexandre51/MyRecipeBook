using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Infrastructure.DataAccess;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.User.Register;

public class RegisterUserAccountUseCaseTests : IClassFixture<MyRecipeBookApplicationFactory>
{
    private const string REQUEST_URI = "/api/users";

    private readonly HttpClient _httpClient;

    private readonly MyRecipeBookDbContext _dbContext;

    public RegisterUserAccountUseCaseTests(MyRecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        var scope = factory.Services.CreateScope();

        _dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responeData = await JsonDocument.ParseAsync(responseBody);

        responeData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responeData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();

        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Name == request.Name);

        var userExists = await _dbContext.Users.AnyAsync(user => 
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

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

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

        var userExists = await _dbContext.Users.AnyAsync(user =>
        user.Active
        && user.Name.Equals(request.Name)
        && user.Name.Equals(request.Email));

        userExists.ShouldBeFalse();
    }
}