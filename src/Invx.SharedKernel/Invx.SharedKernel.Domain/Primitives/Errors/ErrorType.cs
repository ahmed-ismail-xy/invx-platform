namespace Invx.SharedKernel.Domain.Primitives.Errors;
public enum ErrorType
{
    None = 0,

    // Domain Layer Errors
    Domain = 100,
    BusinessRule = 101,
    InvalidOperation = 102,

    // Application Layer Errors  
    Validation = 200,
    NotFound = 201,
    Conflict = 202,
    Unauthorized = 203,
    Forbidden = 204,

    // Infrastructure Layer Errors
    External = 300,
    Database = 301,
    Network = 302,

    // System Errors
    BadRequest = 400,
    InternalServerError = 500,
    ServiceUnavailable = 503,
    TooManyRequests = 429,
    UnprocessableEntity = 422
}