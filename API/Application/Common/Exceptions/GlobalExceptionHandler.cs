using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Application.Common.Exceptions
{
    public static class GlobalExceptionHandler
    {
        public static async Task Handle(HttpContext httpContext)
        {
            var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature == null)
                return;

            var (httpStatusCode, message) = exceptionHandlerFeature.Error switch
            {
                NotFoundException exception => (HttpStatusCode.NotFound, exception.Message),
                DuplicateValueException exception => (HttpStatusCode.BadRequest, exception.Message),
                ValidationException exception => (HttpStatusCode.BadRequest, CreateErrorMessage(exception)),
                _ => (HttpStatusCode.InternalServerError, "Unexpected error.")
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)httpStatusCode;

            var jsonResponse = new
            {
                httpContext.Response.StatusCode,
                Message = message,
            };

            var jsonSerialised = JsonSerializer.Serialize(jsonResponse);
            await httpContext.Response.WriteAsync(jsonSerialised);
        }

        private static string CreateErrorMessage(ValidationException error)
        {
            var errors = error.Failures?.Values?.Select(failures => failures.First());

            return errors is null
                ? error.Message
                : string.Join(", ", errors);
        }
    }
}
