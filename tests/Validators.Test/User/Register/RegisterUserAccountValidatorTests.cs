using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exception;
using Shouldly;

namespace Validators.Test.User.Register;

public class RegisterUserAccountValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;
        
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_REQUIRED));
        });
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new RegisterUserAccountValidator();

        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();


        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_REQUIRED));
        });
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new RegisterUserAccountValidator();

        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();


        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_NOT_EMPTY));
        });
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserAccountValidator();

        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.Errors.ShouldSatisfyAllConditions(erros =>
        {
            erros.Count.ShouldBe(1);
            erros.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
        });
    }
}
