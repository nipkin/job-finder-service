namespace JobFinder.Application
{
    public class Result<T>
    {
        public T? Value { get; }
        public Error? Err { get; }
        public int StatusCode { get; }
        public bool IsSuccess => Err is null;

        private Result(T value) { Value = value; StatusCode = 200; }
        private Result(Error error) { Err = error; StatusCode = error.StatusCode; }

        public static Result<T> Ok(T value) => new(value);
        public static Result<T> Fail(Error error) => new(error);

        public static implicit operator Result<T>(T value) => Ok(value);
        public static implicit operator Result<T>(Error error) => Fail(error);
    }
}