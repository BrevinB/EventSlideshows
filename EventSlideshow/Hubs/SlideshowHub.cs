using Microsoft.AspNetCore.SignalR;

namespace GraduationSlideshows.Hubs;

public class SlideshowHub : Hub
{
    public Task GetImage(bool refresh)
    {
        return Clients.All.SendAsync("GetImage", refresh);
    }
}