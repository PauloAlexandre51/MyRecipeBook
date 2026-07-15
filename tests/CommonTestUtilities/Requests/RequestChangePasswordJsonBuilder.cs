using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int length = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(u => u.CurrentPassword, (f) => f.Internet.Password())
            .RuleFor(u => u.NewPassword, (f) => f.Internet.Password(length));
    }
}
