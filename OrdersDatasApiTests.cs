using API_BurgerManiaBackend.Controllers;
using API_BurgerManiaBackend.Data;
using API_BurgerManiaBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace API_BurgerManiaBackend.Tests
{
    public class OrdersDatasApiTests
    {
        private readonly BurgerManiaDbContext _dbContext;
        private readonly OrdersDatasApi _ordersDatasApi;
        public OrdersDatasApiTests()
        {
            _dbContext = new BurgerManiaDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<BurgerManiaDbContext>());
            _ordersDatasApi = new OrdersDatasApi(_dbContext);
        }
        [Fact]
        public async void GetOrders_ReturnsAllOrders()
        {
            // Execute
            var actionResult = await _ordersDatasApi.GetOrdersDatas();
            var objectResult = actionResult.Value;
            Assert.IsAssignableFrom<IEnumerable<OrdersData>>(objectResult);
        }
        [Fact]
        public async void GetOrderByIdExisting_ReturnsOneOrder()
        {
            // Execute
            var actionResult = await _ordersDatasApi.GetOrdersData(new Guid("b5a70476-efea-4ddc-add5-1cabddd000bd"));
            var objectResult = actionResult.Value;
            Assert.IsAssignableFrom<OrdersData>(objectResult);
        }
        [Fact]
        public async void GetOrderByIdNonExisting_ReturnsNotFound()
        {
            // Execute
            var actionResult = await _ordersDatasApi.GetOrdersData(new Guid());
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        [Fact]
        public async Task PutOrdersData_BadRequest_Test()
        {
            var ordersData = new OrdersData { OrderDateTime = new DateTime(), UserId = new Guid(), TotalBillPrice = 400 };

            // Act
            var result = await _ordersDatasApi.PutOrdersData(new Guid(), ordersData);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutOrdersData_NotFound_Test()
        {
            var id = new Guid();
            var ordersData = new OrdersData { OrderId = id, OrderDateTime = new DateTime(), UserId = new Guid(), TotalBillPrice = 400 };

            // Act
            var result = await _ordersDatasApi.PutOrdersData(id, ordersData);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task PutOrdersData_Success_Test()
        {
            var id = new Guid("b5a70476-efea-4ddc-add5-1cabddd000bd");
            var ordersData = new OrdersData { OrderId = id, OrderDateTime = new DateTime(), UserId = new Guid("82062250-cdac-4892-a237-8cb631a7f2a9"), TotalBillPrice = 500 };

            // Act
            var result = await _ordersDatasApi.PutOrdersData(id, ordersData);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task PostOrdersData_Test()
        {
            // Arrange
            var ordersData = new OrdersData { OrderId = new Guid(), OrderDateTime = new DateTime(), UserId = new Guid("82062250-cdac-4892-a237-8cb631a7f2a9"), TotalBillPrice = 500 };

            // Act
            var result = await _ordersDatasApi.PostOrdersData(ordersData);

            // Assert
            var actionResult = Assert.IsType<ActionResult<OrdersData>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<OrdersData>(createdAtActionResult.Value);
            Assert.Equal(ordersData, returnValue);
            // Assert other properties of returnValue to make sure they match ordersData
        }

        [Fact]
        public async Task DeleteOrdersData_NotFound_Test()
        {
            // Arrange
            var id = Guid.NewGuid(); // ID of non-existent OrdersData

            // Act
            var result = await _ordersDatasApi.DeleteOrdersData(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteOrdersData_Test()
        {
            // Arrange
            var id = Guid.NewGuid();
            var ordersData = new OrdersData { OrderId = id, OrderDateTime = new DateTime(), UserId = new Guid("82062250-cdac-4892-a237-8cb631a7f2a9"), TotalBillPrice = 500 };
            await _ordersDatasApi.PostOrdersData(ordersData);

            // Act
            var result = await _ordersDatasApi.DeleteOrdersData(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}
