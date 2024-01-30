namespace CarRentalAPI.Handlers;

public class LogHandler(ILogger<LogHandler> logger) : ILogHandler
{
    public void LogNewRequest(string objectType, ILogHandler.RequestEnum requestType)
    {
        logger.LogInformation("New {0} {1} request received.", objectType, requestType);
    }

    public void LogAction(ILogHandler.ActionEnum actionType, int rentalId)
    {
        logger.LogInformation("Rental with id {0} {1}", rentalId, actionType);
    }

    public void LogAction(ILogHandler.ActionEnum actionType, int rentalId, int carId)
    {
        logger.LogInformation("Rental with id {0} car: {1} {2}", rentalId, carId, actionType);
    }
}
