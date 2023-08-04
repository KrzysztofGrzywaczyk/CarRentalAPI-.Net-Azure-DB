using Microsoft.Extensions.Logging;

namespace CarRentalAPI.Handlers
{
    public class LogHandler : ILogHandler
    {
        private readonly ILogger logger;
        public LogHandler(ILogger<LogHandler> logger)
        {
            this.logger = logger;
        }

        public void LogNewRequest (string objectType, string requestType) 
        {
            this.logger.LogInformation("New {0} {1} request received.", objectType, requestType.ToUpper());
        }
    }
}
