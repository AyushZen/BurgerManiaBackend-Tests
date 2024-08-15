using API_BurgerManiaBackend.Controllers;
using API_BurgerManiaBackend.Data;
using API_BurgerManiaBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_BurgerManiaBackend.Tests
{
    public class BurgerOrderDatasApiTests
    {
        private readonly BurgerManiaDbContext _dbContext;
        private readonly BurgerOrderDatasApi _burgerOrderDatasApi;

        public BurgerOrderDatasApiTests()
        {
            _dbContext = new BurgerManiaDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<BurgerManiaDbContext>());
            _burgerOrderDatasApi = new BurgerOrderDatasApi(_dbContext);
        }

        [Fact]
        public async void GetBurgers_ReturnsAllBurgers()
        {
            // Execute
            var actionResult = await _burgerOrderDatasApi.GetBurgerOrderDatas();
            var objectResult = actionResult.Value;
            Assert.IsAssignableFrom<IEnumerable<BurgerOrderData>>(objectResult);
        }

        [Fact]
        public async void GetBurgerByIdExisting_ReturnsOneBurger()
        {
            // Execute
            var actionResult = await _burgerOrderDatasApi.GetBurgerOrderData(new Guid("ede4ab0e-d17f-45b6-82b2-08dcbd33ba44"));
            var objectResult = actionResult.Value;
            Assert.IsAssignableFrom<BurgerOrderData>(objectResult);
        }
        [Fact]
        public async void GetBurgerByIdNonExisting_ReturnsNotFound()
        {
            // Execute
            var actionResult = await _burgerOrderDatasApi.GetBurgerOrderData(new Guid());
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        [Fact]
        public async Task PutBurgerData_BadRequest_Test()
        {
            var burgerData = new BurgerOrderData { BurgerName = "WHOOPER", BurgerPrice = 100, BurgerImage = "./images/whooper.png", BurgerType = "Veg", BurgerDesc = "Whooper Burger", OrderId = new Guid(), BurgerCount = 5 };

            // Act
            var result = await _burgerOrderDatasApi.PutBurgerOrderData(new Guid(), burgerData);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutBurgerData_NotFound_Test()
        {
            var id = new Guid();
            var burgerData = new BurgerOrderData {BurgerId = id, BurgerName = "WHOOPER", BurgerPrice = 100, BurgerImage = "./images/whooper.png", BurgerType = "Veg", BurgerDesc = "Whooper Burger", OrderId = new Guid(), BurgerCount = 5 };

            // Act
            var result = await _burgerOrderDatasApi.PutBurgerOrderData(id, burgerData);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task PutBurgersData_Success_Test()
        {
            var id = new Guid("ede4ab0e-d17f-45b6-82b2-08dcbd33ba44");
            var burgerData = new BurgerOrderData { BurgerId = id, BurgerName = "WHOOPER", BurgerPrice = 100, BurgerImage = "./images/whooper.png", BurgerType = "Veg", BurgerDesc = "Whooper Burger", OrderId = new Guid(), BurgerCount = 5 };

            // Act
            var result = await _burgerOrderDatasApi.PutBurgerOrderData(id, burgerData);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task PostBurgerData_Test()
        {
            // Arrange
            var id = new Guid();
            var burgerData = new BurgerOrderData { BurgerId = id, BurgerName = "WHOOPER", BurgerPrice = 100, BurgerImage = "./images/whooper.png", BurgerType = "Veg", BurgerDesc = "Whooper Burger", OrderId = new Guid(), BurgerCount = 5 };

            // Act
            var result = await _burgerOrderDatasApi.PostBurgerOrderData(burgerData);

            // Assert
            var actionResult = Assert.IsType<ActionResult<BurgerOrderData>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<BurgerOrderData>(createdAtActionResult.Value);
            Assert.Equal(burgerData, returnValue);
            // Assert other properties of returnValue to make sure they match ordersData
        }

        [Fact]
        public async Task DeleteBurgerData_NotFound_Test()
        {
            // Arrange
            var id = Guid.NewGuid(); // ID of non-existent OrdersData

            // Act
            var result = await _burgerOrderDatasApi.DeleteBurgerOrderData(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //[Fact]
        //public async Task DeleteBurgerData_Test()
        //{
        //    // Arrange
        //    var id = new Guid();
        //    var burgerData = new BurgerOrderData { BurgerId = id, BurgerName = "WHOOPER", BurgerPrice = 100, BurgerImage = "./images/whooper.png", BurgerType = "Veg", BurgerDesc = "Whooper Burger", OrderId = new Guid(), BurgerCount = 6 };
        //    await _burgerOrderDatasApi.PostBurgerOrderData(burgerData);
        //    // Act
        //    var result = await _burgerOrderDatasApi.DeleteBurgerOrderData(id);

        //    // Assert
        //    Assert.IsType<NoContentResult>(result);
        //}
    }
}
