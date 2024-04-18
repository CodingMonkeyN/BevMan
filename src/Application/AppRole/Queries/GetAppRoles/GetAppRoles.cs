namespace BevMan.Application.AppRole.Queries.GetAppRoles;

public record GetAppRolesQuery : IRequest<string[]>;

public class GetAppRolesQueryHandler : IRequestHandler<GetAppRolesQuery, string[]>
{
    public Task<string[]> Handle(GetAppRolesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Enum.GetNames<Domain.Entities.AppRole>());
    }
}
