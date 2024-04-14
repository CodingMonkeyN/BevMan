﻿using BevMan.Application.Common.Interfaces;

namespace BevMan.Application.Products.Queries.GetProduct;

public record GetProductQuery(int Id) : IRequest<ProductDto>;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(query => query.Id).NotNull();
    }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FindAsync(request.Id);

        Guard.Against.NotFound(request.Id, entity);

        return _mapper.Map<ProductDto>(entity);
    }
}
