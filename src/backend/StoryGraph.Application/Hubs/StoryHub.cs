using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace StoryGraph.Application.Hubs;

public sealed class StoryHub : Hub<IStoryHub>
{
    private readonly ILogger<StoryHub> _logger;
    
    public StoryHub(ILogger<StoryHub> logger)
    {
        _logger = logger;
    }
    
    public override async Task OnConnectedAsync()
    {
        _logger
            .LogInformation($"{GetType().Name} created connection with \"{Context.ConnectionId}\" connection ID");
       
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger
            .LogInformation($"{GetType().Name} removed connection with \"{Context.ConnectionId}\" connection ID");
        
        await base.OnDisconnectedAsync(exception);
    }
}