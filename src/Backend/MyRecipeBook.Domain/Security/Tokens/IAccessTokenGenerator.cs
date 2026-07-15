using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(User user);
}
