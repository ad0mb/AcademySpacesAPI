namespace Core.Exceptions;

public class SchedulingConflictException : Exception
{
    public SchedulingConflictException()
    {
        
    }

    public SchedulingConflictException(string message) : base(message)
    {
        
    }

    public SchedulingConflictException(string message, Exception inner) : base(message, inner)
    {
        
    }
}