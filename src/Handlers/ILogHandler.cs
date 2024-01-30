namespace CarRentalAPI.Handlers;

public interface ILogHandler
{
    public enum ActionEnum
    {
        Created,
        Deleted,
        Updated
    }

    public enum RequestEnum
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public void LogNewRequest(string objectType, RequestEnum requestType);
    
    public void LogAction(ActionEnum actionType, int rentalId);

    public void LogAction(ActionEnum actionType, int rentalId, int carId);
}
