namespace Catalog.API.Products.GetProducts;

public record GetProductResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/products", handler: async (ISender sender) =>
        {
            object? result = await sender.Send(request: new GetProductsQuery());

            GetProductResponse? response = result.Adapt<GetProductResponse>();

            return Results.Ok(value: response);
        })
        .WithName(endpointName: "GetProducts")
        .Produces<GetProductResponse>(statusCode: StatusCodes.Status200OK)
        .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
        .WithSummary(summary: "Get Products")
        .WithDescription(description: "GetProducts");
    }
}