using CarRentalAPI.Entities;
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

        public void LogNewRequest(string objectType, ILogHandler.RequestEnum requestType)
        {
            this.logger.LogInformation("New {0} {1} request received.", objectType, requestType);
        }

        public void LogAction(ILogHandler.ActionEnum actionType, int entityId)
        {
            this.logger.LogInformation("Rental with id {0} {1}", entityId, actionType);
        }
    }
}
