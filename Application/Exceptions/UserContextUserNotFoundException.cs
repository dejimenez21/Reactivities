namespace Application.Exceptions;

internal class UserContextUserNotFoundException : Exception
{
    public UserContextUserNotFoundException(string username) : base($"The UserContext username '{username}' was not found in the database")
    {
        
    }
}
