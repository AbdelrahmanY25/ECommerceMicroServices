namespace BussinesLogicLayer.Services;

public class OrdersService(IOrdersRepository ordersRepository, IMapper mapper, IValidator<OrderAddRequest> orderAddRequestValidator, IValidator<OrderItemAddRequest> orderItemAddRequestValidator, IValidator<OrderUpdateRequest> orderUpdateRequestValidator, IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator) : IOrdersService
{
  private readonly IValidator<OrderAddRequest> _orderAddRequestValidator = orderAddRequestValidator;
  private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator = orderItemAddRequestValidator;
  private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator = orderUpdateRequestValidator;
  private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;
  private readonly IMapper _mapper = mapper;
  private readonly IOrdersRepository _ordersRepository = ordersRepository;

	public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
	{
		if (orderAddRequest is not null)
		{
			ValidationResult orderAddRequestValidationResult = await _orderAddRequestValidator.ValidateAsync(orderAddRequest);
			if (!orderAddRequestValidationResult.IsValid)
			{
				string errors = string.Join(", ", orderAddRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
				throw new ArgumentException(errors);
			}

			foreach (OrderItemAddRequest orderItemAddRequest in orderAddRequest.OrderItems)
			{
				ValidationResult orderItemAddRequestValidationResult = await _orderItemAddRequestValidator.ValidateAsync(orderItemAddRequest);

				if (!orderItemAddRequestValidationResult.IsValid)
				{
					string errors = string.Join(", ", orderItemAddRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
					throw new ArgumentException(errors);
				}
			}

			Order orderInput = _mapper.Map<Order>(orderAddRequest);

			//Generate values
			foreach (OrderItem orderItem in orderInput.OrderItems)
			{
				orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
			}
			orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);


			//Invoke repository
			Order? addedOrder = await _ordersRepository.AddOrder(orderInput);

			if (addedOrder is null)
			{
				return null;
			}

			OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(addedOrder); //Map addedOrder ('Order' type) into 'OrderResponse' type (it invokes OrderToOrderResponseMappingProfile).

			return addedOrderResponse;
		}

		throw new ArgumentNullException(nameof(orderAddRequest));
	}



	public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
	{
		if (orderUpdateRequest is not null)
		{
			ValidationResult orderUpdateRequestValidationResult = await _orderUpdateRequestValidator.ValidateAsync(orderUpdateRequest);

			if (!orderUpdateRequestValidationResult.IsValid)
			{
				string errors = string.Join(", ", orderUpdateRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
				throw new ArgumentException(errors);
			}

			foreach (OrderItemUpdateRequest orderItemUpdateRequest in orderUpdateRequest.OrderItems)
			{
				ValidationResult orderItemUpdateRequestValidationResult = await _orderItemUpdateRequestValidator.ValidateAsync(orderItemUpdateRequest);
				if (!orderItemUpdateRequestValidationResult.IsValid)
				{
					string errors = string.Join(", ", orderItemUpdateRequestValidationResult.Errors.Select(temp => temp.ErrorMessage));
					throw new ArgumentException(errors);
				}
			}

			Order orderInput = _mapper.Map<Order>(orderUpdateRequest);

			foreach (OrderItem orderItem in orderInput.OrderItems)
				orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

			orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);

			Order? updatedOrder = await _ordersRepository.UpdateOrder(orderInput);

			if (updatedOrder is null)
				return null;

			OrderResponse updatedOrderResponse = _mapper.Map<OrderResponse>(updatedOrder);

			return updatedOrderResponse;
		}

		throw new ArgumentNullException(nameof(orderUpdateRequest));
	}


	public async Task<bool> DeleteOrder(Guid orderID)
	  {
		FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);
		Order? existingOrder = await _ordersRepository.GetOrderByCondition(filter);

		if (existingOrder is null)
		  return false;

		bool isDeleted = await _ordersRepository.DeleteOrder(orderID);
		return isDeleted;
	  }


  public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
  {
    Order? order = await _ordersRepository.GetOrderByCondition(filter);

    if (order is null)
      return null;

    OrderResponse orderResponse = _mapper.Map<OrderResponse>(order);
    return orderResponse;
  }


  public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
  {
    IEnumerable<Order?> orders = await _ordersRepository.GetOrdersByCondition(filter);
    

    IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders); 
    return [.. orderResponses];
  }


  public async Task<List<OrderResponse?>> GetOrders()
  {
    IEnumerable<Order?> orders = await _ordersRepository.GetOrders();


    IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
    return [.. orderResponses];
  }
}