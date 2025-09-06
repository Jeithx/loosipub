namespace Business.Abstract;

public interface ILogger
{
    void LogException(Exception exception, string methodName, object? parameters = null);
    void LogInformation(string methodName, object? parameters = null);
    void LogNotFound<T>(long id, string methodName);
}