using BussinessLogicLayer.RabbitMQ.Messages;

namespace BussinessLogicLayer.Services;

public class ProductsService(IValidator<ProductAddRequest> productAddRequestValidator,
							 IValidator<ProductUpdateRequest> productUpdateRequestValidator,
							 IProductsRepository productsRepository, IRabbitMQPublisher rabbitMQPublisher) : IProductsService
{
	private readonly IRabbitMQPublisher _rabbitMQPublisher = rabbitMQPublisher;
	private readonly IProductsRepository _productsRepository = productsRepository;
	private readonly IValidator<ProductAddRequest> _productAddRequestValidator = productAddRequestValidator;
	private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator = productUpdateRequestValidator;

	public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
	{
		if (productAddRequest is not null)
		{
			ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

			if (!validationResult.IsValid)
			{
				string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
				throw new ArgumentException(errors);
			}


			Product productInput = productAddRequest.Adapt<Product>();
			Product? addedProduct = await _productsRepository.AddProduct(productInput);

			if (addedProduct is null)
			{
				return null;
			}

			ProductResponse addedProductResponse = addedProduct.Adapt<ProductResponse>();

			return addedProductResponse;
		}

		throw new ArgumentNullException(nameof(productAddRequest));
	}

	public async Task<bool> DeleteProduct(Guid productID)
	{
		Product? existingProduct = await _productsRepository.GetProductByCondition(temp => temp.ProductID == productID);

		if (existingProduct is null)
		{
			return false;
		}

		bool isDeleted = await _productsRepository.DeleteProduct(productID);
		return isDeleted;
	}

	public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
	{
		Product? product = await _productsRepository.GetProductByCondition(conditionExpression);
		if (product is null)
		{
			return null;
		}

		ProductResponse productResponse = product.Adapt<ProductResponse>();
		return productResponse;
	}

	public async Task<List<ProductResponse?>> GetProducts()
	{
		IEnumerable<Product?> products = await _productsRepository.GetProducts();


		IEnumerable<ProductResponse?> productResponses = products.Adapt<IEnumerable<ProductResponse>>();
		return [.. productResponses];
	}

	public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
	{
		IEnumerable<Product?> products = await _productsRepository.GetProductsByCondition(conditionExpression);

		IEnumerable<ProductResponse?> productResponses = products.Adapt<IEnumerable<ProductResponse>>();
		return [.. productResponses];
	}

	public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
	{
		Product? existingProduct = await _productsRepository
			.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductID) ??
				throw new ArgumentException("Invalid Product ID");

		ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

		if (!validationResult.IsValid)
		{
			string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
			throw new ArgumentException(errors);
		}

		Product product = productUpdateRequest.Adapt<Product>();

		bool isProductNameChanged = productUpdateRequest.ProductName != existingProduct.ProductName;

		Product? updatedProduct = await _productsRepository.UpdateProduct(product);

		if (isProductNameChanged)
		{
			string routingKey = "product.update.name";

			var message = new ProductNameUpdateMessage(product.ProductID, product.ProductName);

			await _rabbitMQPublisher.PublishAsync(routingKey, message);
		}

		return updatedProduct.Adapt<ProductResponse>();
	}
}