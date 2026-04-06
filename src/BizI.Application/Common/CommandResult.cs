namespace BizI.Application.Common;

/// <summary>
/// Represents the result of a command (write operation).
/// Used as the return type for MediatR command handlers.
/// </summary>
public class CommandResult
{
    public bool Success { get; set; }

    /// <summary>Human-readable message, typically the error message on failure.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>The identifier of the entity created/modified, when applicable.</summary>
    public string? Id { get; set; }

    /// <summary>Optional payload returned with the result (e.g. a DTO or token).</summary>
    public object? Data { get; set; }

    // ── Factories ─────────────────────────────────────────────────────────────

    public static CommandResult SuccessResult(object? data = null) =>
        new() { Success = true, Data = data };

    public static CommandResult SuccessResult(string id, object? data = null) =>
        new() { Success = true, Id = id, Data = data };

    public static CommandResult FailureResult(string message) =>
        new() { Success = false, Message = message };
}
