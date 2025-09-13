namespace Core.Exceptions;

public class RosterConflictException : Exception
{
    public RosterConflictException()
    {
        
    }

    public RosterConflictException(string message) : base(message)
    {
        
    }

    public RosterConflictException(string message, Exception inner) : base(message, inner)
    {
        
    }
}