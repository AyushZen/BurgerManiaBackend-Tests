using API_BurgerManiaBackend.Controllers;
using API_BurgerManiaBackend.Data;
using API_BurgerManiaBackend.Models;
using API_BurgerManiaBackend.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SecureWebApiSample.Services;

namespace API_BurgerManiaBackend.Tests
{
    public class UserDataTests
    {
        private readonly BurgerManiaDbContext _dbContext;
        private readonly UserDatasApi _userDatasApi;

        public UserDataTests()
        {
            _dbContext = new BurgerManiaDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<BurgerManiaDbContext>());
            var jwtSettings = new JwtSettings();
            jwtSettings.Audience = "Internal1";
            jwtSettings.Issuer = "Zensar1";
            jwtSettings.SecretKey = "Key: { AA111111 - 1111 - 1111 - 1111 - 512CA256E145}";

            _userDatasApi = new UserDatasApi(_dbContext, new TokenService(jwtSettings));
        }

        [Fact]
        public async void GetUsers_ReturnsAllUsers()
        {
            // Execute
            var actionResult = await _userDatasApi.GetUserDatas();
            var objectResult = actionResult.Value;
            Assert.IsAssignableFrom<IEnumerable<UserData>>(objectResult);
        }
        [Fact]
        public async void GetUserByNumberExisting_ReturnsOneUser()
        {
            // Execute
            var actionResult = _userDatasApi.GetUserByMobileNo("7300340940");
            Assert.IsType<OkObjectResult>(actionResult);
        }
        [Fact]
        public async void GetUserByNumberNonExisting_ReturnsNotFound()
        {
            // Execute
            var actionResult = _userDatasApi.GetUserByMobileNo("1234567890");
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }
    }
}
