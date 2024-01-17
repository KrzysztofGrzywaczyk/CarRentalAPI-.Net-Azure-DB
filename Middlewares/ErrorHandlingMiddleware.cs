using CarRentalAPI.Exceptions;
using System.Runtime.InteropServices;

namespace CarRentalAPI.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadHttpRequestException ex)
            {
                context.Response.StatusCode = 400;
                await CreateErrorMessageAsync(context, ex);
            }
            catch (ForbidException ex)
            {
                context.Response.StatusCode = 403;
                await CreateErrorMessageAsync(context, ex);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await CreateErrorMessageAsync(context, ex);
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = 400;
                await CreateErrorMessageAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
