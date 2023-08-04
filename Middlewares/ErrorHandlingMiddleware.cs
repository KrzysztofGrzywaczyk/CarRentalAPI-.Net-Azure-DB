using CarRentalAPI.Exceptions;
using System.Runtime.InteropServices;

namespace CarRentalAPI.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public readonly ILogger<ErrorHandlingMiddleware> logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await CreateErrorMessageAsync(context, ex);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await CreateErrorMessageAsync(context, ex);
            }
        }

        private async Task CreateErrorMessageAsync(HttpContext context, Exception ex)
        {
            await context.Response.WriteAsync(ex.Message);
        }
    }
}
