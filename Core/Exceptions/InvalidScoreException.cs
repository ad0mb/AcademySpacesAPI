namespace Core.Exceptions;

public class InvalidScoreException : Exception
{
    public InvalidScoreException()
    {
    }

    public InvalidScoreException(string message) : base(message)
    {
    }

    public InvalidScoreException(string message, Exception inner) : base(message, inner)
    {
    }
}
