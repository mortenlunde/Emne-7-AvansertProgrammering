using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;
using Newtonsoft.Json;

namespace StudentBlogg.Middleware;

public class GlobalExceptionHandling(ILogger<GlobalExceptionHandling> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        
        logger.LogError(exception, "An unexpected error occurred.");
        httpContext.Response.ContentType = "application/json";

        int statusCode = exception switch
        {
            UsernameAlreadyExistsException => StatusCodes.Status409Conflict,
            EmailAlreadyExistsException => StatusCodes.Status409Conflict,
            InvalidAuthHeaderException => StatusCodes.Status400BadRequest,
            MissingUsernameOrPasswordException => StatusCodes.Status400BadRequest,
            WrongUsernameOrPasswordException => StatusCodes.Status401Unauthorized,
            UnauthorisedOperation => StatusCodes.Status401Unauthorized,
            NoUserFoundException => StatusCodes.Status404NotFound,
            DbConnectionException => StatusCodes.Status100Continue,
            MySqlException => StatusCodes.Status503ServiceUnavailable,
            RetryLimitExceededException => StatusCodes.Status503ServiceUnavailable,
            DbUpdateException => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };
        
        httpContext.Response.StatusCode = statusCode;
        string? statusDescription = Enum.GetName(typeof(HttpStatusCode), statusCode);

        var response = new
        { 
            statusCode,
            statusDescription,
            message = exception.Message
        };
        
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), cancellationToken);
        return true;
    }

    
}

// User Exceptions
public class UsernameAlreadyExistsException(string username)
    : Exception($"Brukernavnet '{username}' er allerede i bruk.");

public class EmailAlreadyExistsException(string email) 
    : Exception($"E-posten '{email}' er allerede i bruk.");

    
// Authentication Exceptions
public class WrongUsernameOrPasswordException() 
    : Exception("Username or password is incorrect.") { }
    
public class MissingUsernameOrPasswordException()
    : Exception("Missing username and/or password.") { }
    
public class InvalidAuthHeaderException()
    : Exception("Invalid auth header.") { }
    
public class UnauthorisedOperation()
    : Exception("Unauthorised operation. You are trying to delete a useraccount that does not belong to your login") { }
    
public class NoUserFoundException(string id)
    : Exception($"User with ID {id} does not exist.") { }
    
// Database Connection Exceptions
public class DbConnectionException() 
    : Exception("Connection error.") { }