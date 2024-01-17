using CarRentalAPI.Entities;
using Microsoft.Extensions.Logging;

namespace CarRentalAPI.Handlers
{
    public class LogHandler : ILogHandler
    {

        private readonly ILogger _logger;

        public LogHandler(ILogger<LogHandler> logger)
        {
            _logger = logger;
        }

        public void LogNewRequest(string objectType, ILogHandler.RequestEnum requestType)
        {
            _logger.LogInformation("New {0} {1} request received.", objectType, requestType);
        }

        public void LogAction(ILogHandler.ActionEnum actionType, int rentalId)
        {
            _logger.LogInformation("Rental with id {0} {1}", rentalId, actionType);
        }

        public void LogAction(ILogHandler.ActionEnum actionType, int rentalId, int carId)
        {
            _logger.LogInformation("Rental with id {0} car: {1} {2}", rentalId, carId, actionType);
        }
    }
}
