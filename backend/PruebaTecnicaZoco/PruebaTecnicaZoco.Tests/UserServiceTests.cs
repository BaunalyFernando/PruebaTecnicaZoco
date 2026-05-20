using Microsoft.EntityFrameworkCore;
using Moq;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Services.UserService;
using PruebaTecnicaZoco.Services.UserService.UserDTO;

namespace PruebaTecnicaZoco.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateUser_ShouldCreateUser_WhenDataIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);

            var currentUserMock = new Mock<ICurrentUserService>();
            currentUserMock.Setup(x => x.IsAdmin).Returns(true);

            var service = new UserService(context, currentUserMock.Object);

            var userDto = new UserNormalDTO
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Email = "juan@test.com",
                Password = "1234",
                Dni = "12345"
            };

            // Act
            var result = await service.CreateUser(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("juan@test.com", result.Email);
        }

        [Fact]
        public async Task Should_GiveErrorWhenCreateAUserWithAlreadyExistingMail()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);

            var currentUserMock = new Mock<ICurrentUserService>();

            var service = new UserService(context, currentUserMock.Object);



            var createUser = new UserNormalDTO
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Email = "admin@admin.com",
                Password = "1234",
                Dni = "12345"
            };

            //Act
            var resultado = await service.CreateUser(createUser);

            //Assert

            Assert.NotNull(resultado);
            await Assert.ThrowsAsync<BadRequestException>(() =>
            service.CreateUser(createUser));
        }

        [Fact]
        public async Task Should_GiveErrorWhenDeleteAUserWithoutAdmin()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);

            var service = new UserService(context, new Mock<ICurrentUserService>().Object);

            //Act
            var user = service.DeleteUserAsync(1);

            //Assert
            Assert.NotNull(user);
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.DeleteUserAsync(1));

        }
    }
}
