using System.Diagnostics;

namespace CarRentalAPI.Middlewares
{
    public class RequestTimeMiddleware : IMiddleware
    {
        public readonly ILogger<RequestTimeMiddleware> logger;
        public readonly Stopwatch stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            this.logger = logger;
            this.stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            stopwatch.Start();
            await next.Invoke(context);
            stopwatch.Stop();

            var elapsedTime = stopwatch.ElapsedMilliseconds;

            if (elapsedTime >= 5000) 
            {
                var message = $"{context.Request.Method} request at {context.Request.Path} took {elapsedTime} ms";

                logger.LogWarning(message);
            }
                

        }
    }
}
