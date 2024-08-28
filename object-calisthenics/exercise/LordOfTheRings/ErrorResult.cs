namespace LordOfTheRings;

public record ErrorResult(string Message)
{
    public static ErrorResult FailedBecause(string because) => new(because);
}