namespace AcademySpacesAPI.Exceptions;

public class NoRowsAffectedException : Exception
{
    public NoRowsAffectedException(string message) : base (message) { }
}