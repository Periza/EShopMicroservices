namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(expression: command => command.Id).NotEmpty().WithMessage(errorMessage: "Product ID is required");
        
        RuleFor(expression: command => command.Name).NotEmpty().WithMessage(errorMessage: "Name is required")
            .Length(min: 2, max: 150).WithMessage("Name must be between 2 and 150 characters");
        
        RuleFor(expression: command => command.Price).GreaterThan(valueToCompare: 0).WithMessage(errorMessage:"Price must be greater than 0");
    }
}

public class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "UpdateProductHandler.Handler called with {@Command}", args: command);

        Product? product = await session.LoadAsync<Product>(id: command.Id, token: cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(Id: command.Id);
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        session.Update(entities: product);

        await session.SaveChangesAsync(token: cancellationToken);

        return new UpdateProductResult(IsSuccess: true);
    }
}