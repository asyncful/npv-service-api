namespace NetPresentValueService.Domain.Exceptions;

public class DomainValidationException : Exception
{
    public DomainValidationException(string message) : base(message) { }

    public static void ThrowIfNull<T>(T obj, string objectDescriptor) where T : class
    {
        if (obj == null)
        {
            throw new DomainValidationException($"{objectDescriptor} should have a value.");
        }
    }
}