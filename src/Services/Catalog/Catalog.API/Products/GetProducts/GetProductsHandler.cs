namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "GetProductsQueryHandler.Handler called with {@Query}", query);

        IReadOnlyList<Product> products = await session.Query<Product>().ToListAsync(token: cancellationToken);

        return new GetProductsResult(Products: products);
    }
}