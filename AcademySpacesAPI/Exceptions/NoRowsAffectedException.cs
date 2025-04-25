namespace AcademySpacesAPI.Exceptions;

public class NoRowsAffectedException : Exception
{
    public NoRowsAffectedException()
    {
        
    }

    public NoRowsAffectedException(string message) : base(message)
    {
        
    }

    public NoRowsAffectedException(string message, Exception inner) : base(message, inner)
    {
        
    }
    
}