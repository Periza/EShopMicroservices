
using System.ComponentModel.DataAnnotations;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(expression: x => x.Name).NotEmpty().WithMessage(errorMessage: "Name is required");
        RuleFor(expression: x => x.Category).NotEmpty().WithMessage(errorMessage: "Category is required");
        RuleFor(expression: x => x.ImageFile).NotEmpty().WithMessage(errorMessage: "ImageFile is required");
        RuleFor(expression: x => x.Price).GreaterThan(valueToCompare: 0).WithMessage(errorMessage: "Price must be greater than 0");
    }
}
internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommand> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "CreateProductCommandHandler.Handle called with {@Command}", args: command);
        
        // Create a Product entity from command object
        Product product = new()
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        
        // Save to database
        session.Store(entities: product);
        await session.SaveChangesAsync(cancellationToken);
        
        // Return the result
        return new CreateProductResult(Id: product.Id);
    }
}