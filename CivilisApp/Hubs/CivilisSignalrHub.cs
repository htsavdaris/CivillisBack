using Microsoft.AspNetCore.SignalR;
namespace CivilisApp.Hubs
{
    public interface ICivilisSignalrClient
    {
        Task ReportCompletedNotify(string message);
    }
    public class CivilisSignalrHub : Hub<ICivilisSignalrClient>
    {
        public void ReportCompletedNotify(string message)
        {
            Clients.All.ReportCompletedNotify(message);
        }
    }
}
