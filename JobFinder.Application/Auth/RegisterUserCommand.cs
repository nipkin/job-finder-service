namespace JobFinder.Application.Auth
{
    public record RegisterUserCommand(string UserName, string Password, string ConfirmPassword);
}
