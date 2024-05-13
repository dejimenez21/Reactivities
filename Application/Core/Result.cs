namespace Application.Core
{
    public class Result<T>
    {
        public Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        public Result(bool _, string error)
        {
            IsSuccess = false;
            Error = error;
        }

        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? Error { get; private set; }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(string error) => new(false, error);

        public static implicit operator Result<T>(T value) => Success(value);
    }
}