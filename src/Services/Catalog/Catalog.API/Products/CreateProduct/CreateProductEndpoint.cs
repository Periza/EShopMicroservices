namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price)
{
}

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            pattern: "/products",
            handler: async (CreateProductRequest request, ISender sender) =>
            {
                CreateProductCommand command = request.Adapt<CreateProductCommand>();

                CreateProductResult? result = await sender.Send(request: command);

                CreateProductResponse? response = result.Adapt<CreateProductResponse>();

                return Results.Created(uri: $"/products/{response.Id}", value: response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(statusCode: StatusCodes.Status201Created)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .WithSummary(summary: "Create Product")
            .WithDescription("Create Product");
    }
}