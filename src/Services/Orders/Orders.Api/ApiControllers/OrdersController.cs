namespace Orders.Api.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IOrdersService ordersService) : ControllerBase
{
	private readonly IOrdersService _ordersService = ordersService;

	[HttpGet]
	public async Task<IEnumerable<OrderResponse?>> Get()
	{
	List<OrderResponse?> orders = await _ordersService.GetOrders();
	return orders;
	}

	[HttpGet]
	public async Task<OrderResponse?> GetOrderByOrderID(Guid orderID)
	{
	FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);

	OrderResponse? order = await _ordersService.GetOrderByCondition(filter);
	return order;
	}

	[HttpGet]
	public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductID(Guid productID)
	{
	FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(temp => temp.OrderItems, 
		Builders<OrderItem>.Filter.Eq(tempProduct => tempProduct.ProductID, productID)
		);

	List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
	return orders;
	}

	[HttpGet]
	public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime orderDate)
	{
	FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderDate.ToString("yyyy-MM-dd"), orderDate.ToString("yyyy-MM-dd")
		);

	List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
	return orders;
	}

	[HttpPost]
	public async Task<IActionResult> Post(OrderAddRequest orderAddRequest)
	{
		if (orderAddRequest == null)
		{
			return BadRequest("Invalid order data");
		}

		OrderResponse? orderResponse = await _ordersService.AddOrder(orderAddRequest);

		if (orderResponse == null)
		{
			return Problem("Error in adding product");
		}


		return Created($"api/Orders/search/orderid/{orderResponse?.OrderID}", orderResponse);
	}


	[HttpPut("{orderID}")]
	public async Task<IActionResult> Put(Guid orderID, OrderUpdateRequest orderUpdateRequest)
	{
		if (orderUpdateRequest == null)
		{
			return BadRequest("Invalid order data");
		}

		if (orderID != orderUpdateRequest.OrderID)
		{
			return BadRequest("OrderID in the URL doesn't match with the OrderID in the Request body");
		}

		OrderResponse? orderResponse = await _ordersService.UpdateOrder(orderUpdateRequest);

		if (orderResponse == null)
		{
			return Problem("Error in adding product");
		}


		return Ok(orderResponse);
	}


	[HttpDelete("{orderID}")]
	public async Task<IActionResult> Delete(Guid orderID)
	{
		if (orderID == Guid.Empty)
		{
			return BadRequest("Invalid order ID");
		}

		bool isDeleted = await _ordersService.DeleteOrder(orderID);

		if (!isDeleted)
		{
			return Problem("Error in adding product");
		}

		return Ok(isDeleted);
	}
}
