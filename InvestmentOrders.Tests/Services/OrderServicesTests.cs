using InvestmentOrders.Application.DTOs.Orders;
using InvestmentOrders.Application.Interfaces.Repositories;
using InvestmentOrders.Application.Services;
using InvestmentOrders.Domain.Entities;
using InvestmentOrders.Domain.Enums;
using Moq;

namespace InvestmentOrders.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _service = new OrderService(_repositoryMock.Object);
    }

    private void SetupValidAsset(Asset asset)
    {
        _repositoryMock
            .Setup(r => r.AssetExistsAsync(asset.Id))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.GetAssetByIdAsync(asset.Id))
            .ReturnsAsync(asset);
    }

    private Asset CreateAsset(int assetTypeId, decimal unitPrice = 0)
    {
        return new Asset(
            id: 1,
            ticker: "TEST",
            name: "Test Asset",
            unitPrice: unitPrice,
            assetTypeId: assetTypeId
        );
    }

    [Fact]
    public async Task CreateAsync_FCI_ShouldCalculateTotalWithoutFees()
    {
        var asset = CreateAsset(assetTypeId: 3);
        SetupValidAsset(asset);
        _repositoryMock
            .Setup(r => r.GetAssetByIdAsync(asset.Id))
            .ReturnsAsync(asset);

        var dto = new CreateOrderDto
        {
            InvestorId = Guid.NewGuid(),
            AssetId = asset.Id,
            Quantity = 10,
            Price = 100,
            OrderType = OrderType.Buy
        };

        var result = await _service.CreateAsync(dto);

        Assert.Equal(1000m, result.TotalAmount);
    }

    [Fact]
    public async Task CreateAsync_Stock_ShouldUseDbPriceAndApplyFeesAndTaxes()
    {
        var asset = CreateAsset(assetTypeId: 1, unitPrice: 200);
        SetupValidAsset(asset);
        _repositoryMock
            .Setup(r => r.GetAssetByIdAsync(asset.Id))
            .ReturnsAsync(asset);

        var dto = new CreateOrderDto
        {
            InvestorId = Guid.NewGuid(),
            AssetId = asset.Id,
            Quantity = 5,
            Price = 9999, // ignorado
            OrderType = OrderType.Buy
        };

        var result = await _service.CreateAsync(dto);

        Assert.Equal(1007.26m, result.TotalAmount);
    }

    [Fact]
    public async Task CreateAsync_Bond_ShouldApplyBondFeesAndTaxes()
    {
        var asset = CreateAsset(assetTypeId: 2);
        SetupValidAsset(asset);
        _repositoryMock
            .Setup(r => r.GetAssetByIdAsync(asset.Id))
            .ReturnsAsync(asset);

        var dto = new CreateOrderDto
        {
            InvestorId = Guid.NewGuid(),
            AssetId = asset.Id,
            Quantity = 10,
            Price = 50,
            OrderType = OrderType.Buy
        };

        var result = await _service.CreateAsync(dto);

        Assert.Equal(501.21m, result.TotalAmount);
    }

    [Fact]
    public async Task CreateAsync_FCI_WithInvalidPrice_ShouldThrow()
    {
        var asset = CreateAsset(assetTypeId: 3);
        SetupValidAsset(asset);
        _repositoryMock
            .Setup(r => r.GetAssetByIdAsync(asset.Id))
            .ReturnsAsync(asset);

        var dto = new CreateOrderDto
        {
            InvestorId = Guid.NewGuid(),
            AssetId = asset.Id,
            Quantity = 10,
            Price = 0,
            OrderType = OrderType.Buy
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateAsync(dto));
    }

    [Fact]
    public async Task ChangeStatusAsync_FromInProcessToExecuted_ShouldWork()
    {
        var order = new Order(
            Guid.NewGuid(),
            assetId: 1,
            orderStatusId: 1,
            quantity: 10,
            price: 100,
            orderType: OrderType.Buy,
            totalAmount: 1000
        );

        _repositoryMock
            .Setup(r => r.GetByIdAsync(order.Id))
            .ReturnsAsync(order);

        _repositoryMock
            .Setup(r => r.UpdateAsync(order))
            .Returns(Task.CompletedTask);

        await _service.ChangeStatusAsync(order.Id, 2);

        Assert.Equal(2, order.OrderStatusId);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenOrderIsFinal_ShouldThrow()
    {
        var order = new Order(
            Guid.NewGuid(),
            assetId: 1,
            orderStatusId: 2,
            quantity: 10,
            price: 100,
            orderType: OrderType.Buy,
            totalAmount: 1000
        );

        _repositoryMock
            .Setup(r => r.GetByIdAsync(order.Id))
            .ReturnsAsync(order);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ChangeStatusAsync(order.Id, 3));
    }
}
