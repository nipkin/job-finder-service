namespace JobFinder.Application.Auth
{
    public record RegisterUserRequest(string UserName, string Password, string ConfirmPassword);
}
