using System.Diagnostics;
using BevMan.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BevMan.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        ICurrentUser currentUser)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        long elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            string requestName = typeof(TRequest).Name;
            string userId = _currentUser.Id ?? string.Empty;
            string userName = string.Empty;

            _logger.LogWarning(
                "strichlisten_app Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName, request);
        }

        return response;
    }
}
