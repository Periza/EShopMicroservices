namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(pattern: "/products", handler: async (UpdateProductRequest request, ISender sender) =>
            {
                UpdateProductCommand command = request.Adapt<UpdateProductCommand>();

                UpdateProductResult result = await sender.Send(request: command);

                UpdateProductResponse response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(value: response);
            })
            .WithName(endpointName: "UpdateProduct")
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .WithSummary(summary: "UpdateProduct")
            .WithDescription(description: "Update Product");
    }
}