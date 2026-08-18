using System.Net;

namespace PadelBooking.BLL.Exceptions
{
    /// <summary>
    /// Base exception for all application-specific exceptions.
    /// </summary>
    public abstract class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        protected AppException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }

    /// <summary>
    /// 404 – The requested resource was not found.
    /// </summary>
    public class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message, HttpStatusCode.NotFound) { }
    }

    /// <summary>
    /// 400 – The request contains invalid data or fails validation.
    /// </summary>
    public class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(message, HttpStatusCode.BadRequest) { }
    }

    /// <summary>
    /// 401 – Authentication failed (wrong credentials, expired token, etc.).
    /// </summary>
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message)
            : base(message, HttpStatusCode.Unauthorized) { }
    }

    /// <summary>
    /// 403 – The user is authenticated but not allowed to perform this action.
    /// </summary>
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message)
            : base(message, HttpStatusCode.Forbidden) { }
    }

    /// <summary>
    /// 409 – The request conflicts with the current state (e.g. duplicate email).
    /// </summary>
    public class ConflictException : AppException
    {
        public ConflictException(string message)
            : base(message, HttpStatusCode.Conflict) { }
    }
}
