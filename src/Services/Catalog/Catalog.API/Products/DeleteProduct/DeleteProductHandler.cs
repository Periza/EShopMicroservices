namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(expression: command => command.Id).NotEmpty().WithMessage(errorMessage: "Product ID is required");
    }
}

public class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "DeleteProductCommandHandler.Handler called with {@Command}", args: command);

        session.Delete<Product>(id: command.Id);

        await session.SaveChangesAsync(token: cancellationToken);

        return new DeleteProductResult(IsSuccess: true);
    }
}