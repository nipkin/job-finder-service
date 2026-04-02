namespace JobFinder.Application
{
    public record Error(string Message, int StatusCode)
    {
        public static Error Validation(string message) => new(message, 400);
        public static Error Conflict(string message) => new(message, 409);
        public static Error Unauthorized(string message) => new(message, 401);
        public static Error NotFound(string message) => new(message, 404);
    }
}
