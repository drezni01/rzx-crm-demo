using Newtonsoft.Json;
using System.Net;

namespace Rzx.Crm.Api.WebApi
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogWarning(ex, "unhandled server exception");

            var code = HttpStatusCode.InternalServerError;
            string errorMessage = ex.Message;
            string result;

            if(ex.InnerException != null)
            {
                errorMessage = ex.InnerException.Message;
            }

            result = JsonConvert.SerializeObject(new { error = new { code = code, message = errorMessage, exception = ex } });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
