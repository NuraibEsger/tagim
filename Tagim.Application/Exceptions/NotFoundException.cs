namespace Tagim.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"\"{name}\" ({key}) tapılmadı.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}