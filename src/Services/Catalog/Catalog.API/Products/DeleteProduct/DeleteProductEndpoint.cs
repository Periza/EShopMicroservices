namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductRequest(Guid Id);

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(pattern: "/products/{id}", handler: async (Guid id, ISender sender) =>
        {
            DeleteProductResult result = await sender.Send(request: new DeleteProductCommand(Id: id));

            DeleteProductResponse response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(value: response);
        })
        .WithName(endpointName: "DeleteProduct")
        .Produces<DeleteProductResponse>(statusCode: StatusCodes.Status200OK)
        .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
        .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
        .WithSummary(summary: "Delete Product")
        .WithDescription(description: "Delete Product");
    }
}