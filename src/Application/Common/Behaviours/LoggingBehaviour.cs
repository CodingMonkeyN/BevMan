using BevMan.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BevMan.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ICurrentUser _currentUser;
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        string userId = _currentUser.Id ?? string.Empty;
        string? userName = string.Empty;

        _logger.LogInformation("strichlisten_app Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
        return Task.CompletedTask;
    }
}
