namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "/products/{id}", handler: async (Guid id, ISender sender) =>
        {
            GetProductByIdResult result = await sender.Send(request: new GetProductByIdQuery(Id: id));

            GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(value: response);
        })
        .WithName(endpointName: "GetProductById")
        .Produces<GetProductByIdResponse>(statusCode: StatusCodes.Status200OK)
        .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
        .WithSummary(summary: "Get Product By Id")
        .WithDescription(description: "Get Product By Id");
    }
}