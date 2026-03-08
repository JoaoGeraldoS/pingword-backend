using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace pingword.src.Errors
{
    public class ExceptionHandlerError : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandlerError> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        public ExceptionHandlerError(ILogger<ExceptionHandlerError> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Error captured {Payh},{Message}", httpContext.Request.Path, exception.Message);

            var statusCode = exception switch
            {
               KeyNotFoundException => StatusCodes.Status404NotFound,
               InvalidOperationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = exception.Message,
                Detail = "Consult the logs for more details."
            };

            if (_hostEnvironment.IsDevelopment())
            {
                problemDetails.Detail = exception.ToString();
                problemDetails.Extensions.Add("stackTrace", exception.StackTrace);
                problemDetails.Extensions.Add("source", exception.Source);
            }
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
