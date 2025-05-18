using Newtonsoft.Json;
using System.Net;

namespace Rzx.Crm.Api.WebApi
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            logger.LogWarning(ex, "unhandled server exception");

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
