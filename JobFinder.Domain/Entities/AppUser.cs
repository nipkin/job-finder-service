namespace JobFinder.Domain.Entities
{
    public class AppUser
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;


    }
}
