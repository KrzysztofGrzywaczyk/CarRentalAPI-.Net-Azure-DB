using System.Diagnostics;

namespace CarRentalAPI.Middlewares
{
    public class RequestTimeMiddleware : IMiddleware
    {
        public readonly ILogger<RequestTimeMiddleware> _logger;
        public readonly Stopwatch _stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var elapsedTime = _stopwatch.ElapsedMilliseconds;

            if (elapsedTime >= 5000) 
            {
                var message = $"{context.Request.Method} request at {context.Request.Path} took {elapsedTime} ms";

                _logger.LogWarning(message);
            }
                

        }
    }
}

