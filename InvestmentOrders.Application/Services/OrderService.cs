using InvestmentOrders.Application.DTOs.Orders;
using InvestmentOrders.Application.Interfaces.Repositories;
using InvestmentOrders.Application.Interfaces.Services;
using InvestmentOrders.Domain.Entities;

namespace InvestmentOrders.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private const int INITIAL_STATUS_ID = 1; // En proceso

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ResponseOrderDto>> GetAllAsync()
    {
        var orders = await _repository.GetAllAsync();

        return orders.Select(order => new ResponseOrderDto
        {
            Id = order.Id,
            InvestorId = order.InvestorId,
            AssetId = order.AssetId,
            AssetTicker = order.Asset.Ticker,
            AssetName = order.Asset.Name,
            OrderStatusId = order.OrderStatusId,
            OrderStatus = order.Status.DescripcionEstado,
            Quantity = order.Quantity,
            Price = order.Price,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            OrderType = order.OrderType
        });
    }

    public async Task<ResponseOrderDto> GetByIdAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order is null)
            throw new KeyNotFoundException("No se pudo encontrar la orden");

        return new ResponseOrderDto
        {
            Id = order.Id,
            InvestorId = order.InvestorId,
            AssetId = order.AssetId,
            AssetTicker = order.Asset.Ticker,
            AssetName = order.Asset.Name,
            OrderStatusId = order.OrderStatusId,
            OrderStatus = order.Status.DescripcionEstado,
            Quantity = order.Quantity,
            Price = order.Price,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            OrderType = order.OrderType
        };
    }

    public async Task<ResponseOrderDto> CreateAsync(CreateOrderDto dto)
    {
        decimal baseAmount = 0;
        decimal commission = 0;
        decimal tax = 0;
        decimal unitPrice = 0;
        if (dto.Quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero");

        var asset = await _repository.GetAssetByIdAsync(dto.AssetId)
            ?? throw new ArgumentException("El activo no existe");

        switch (asset.AssetTypeId)
        {
            case 3: // FCI
                if (dto.Price <= 0)
                    throw new ArgumentException("Para FCI, el precio debe ser mayor a cero");

                unitPrice = dto.Price;
                baseAmount = unitPrice * dto.Quantity;
                break;

            case 1: // Acción
                unitPrice = asset.UnitPrice;
                baseAmount = unitPrice * dto.Quantity;
                commission = baseAmount * 0.006m;
                tax = commission * 0.21m;
                break;

            case 2: // Bono
                if (dto.Price <= 0)
                    throw new ArgumentException("Para Bonos, el precio debe ser mayor a cero");

                unitPrice = dto.Price;
                baseAmount = unitPrice * dto.Quantity;
                commission = baseAmount * 0.002m;
                tax = commission * 0.21m;
                break;
        }

        var totalAmount = baseAmount + commission + tax;

        var order = new Order(
            dto.InvestorId,
            asset.Id,
            INITIAL_STATUS_ID,
            dto.Quantity,
            unitPrice,
            dto.OrderType,
            totalAmount
        );

        await _repository.AddAsync(order);

        return new ResponseOrderDto
        {
            Id = order.Id,
            InvestorId = order.InvestorId,
            AssetId = order.AssetId,
            OrderStatusId = order.OrderStatusId,
            Quantity = order.Quantity,
            Price = order.Price,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            OrderType = order.OrderType
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("No se pudo encontrar la orden");

        await _repository.DeleteAsync(order);
    }

    public async Task ChangeStatusAsync(Guid orderId, int newStatusId)
    {
        var order = await _repository.GetByIdAsync(orderId)
            ?? throw new KeyNotFoundException("No se pudo encontrar la orden");

        // Estados finales: Ejecutada (2), Cancelada (3)
        if (order.OrderStatusId != INITIAL_STATUS_ID)
            throw new InvalidOperationException("El estado de la orden no se puede cambiar");

        if (newStatusId != 2 && newStatusId != 3)
            throw new ArgumentException("Estado inválido");

        if (newStatusId == order.OrderStatusId)
            throw new InvalidOperationException("El estado actual de la orden es el mismo");


        order.ChangeStatus(newStatusId);

        await _repository.UpdateAsync(order);
    }
}
