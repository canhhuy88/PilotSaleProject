namespace BizI.Application.Common;

public class CommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

    public static CommandResult SuccessResult(object? data = null) => new() { Success = true, Data = data };
    public static CommandResult FailureResult(string message) => new() { Success = false, Message = message };
}
