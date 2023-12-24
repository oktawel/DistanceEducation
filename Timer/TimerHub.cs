using DistanceEducation.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace DistanceEducation.Hubs
{
    public class TimerHub : Hub
    {
        public void StartTimer(int seconds)
        {
            //TestController.UpdateTimer(seconds);
        }
    }
}