namespace ClinicManagement.Domain.Exceptions;

/// <summary>
/// Base exception for clinic management domain
/// </summary>
public class ClinicManagementException : Exception
{
    public ClinicManagementException()
    {
    }

    public ClinicManagementException(string message) : base(message)
    {
    }

    public ClinicManagementException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when an entity is not found
/// </summary>
public class EntityNotFoundException : ClinicManagementException
{
    public EntityNotFoundException(string entityName, int id)
        : base($"{entityName} with ID {id} not found")
    {
    }
}

/// <summary>
/// Exception thrown when a validation fails
/// </summary>
public class ValidationException : ClinicManagementException
{
    public ValidationException(string message) : base(message)
    {
    }
}
