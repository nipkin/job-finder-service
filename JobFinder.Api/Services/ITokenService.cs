namespace JobFinder.Api.Services
{
    public interface ITokenService
    {
        string Generate(Guid userId, string userName);
        System.Security.Claims.ClaimsPrincipal? Validate(string token);
    }
}
