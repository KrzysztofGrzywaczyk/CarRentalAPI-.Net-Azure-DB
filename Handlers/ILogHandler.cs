namespace CarRentalAPI.Handlers
{
    public interface ILogHandler
    {
        public void LogNewRequest(string objectType, string requestType);
    }
}
