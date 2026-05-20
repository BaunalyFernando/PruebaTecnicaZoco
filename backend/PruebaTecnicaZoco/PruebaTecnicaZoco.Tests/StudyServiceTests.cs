using Microsoft.EntityFrameworkCore;
using Moq;
using PruebaTecnicaZoco.Common.Exceptions;
using PruebaTecnicaZoco.Repository;
using PruebaTecnicaZoco.Services.StudyService;
using PruebaTecnicaZoco.Services.StudyService.StudiesDto;
using PruebaTecnicaZoco.Services.UserService;

namespace PruebaTecnicaZoco.Tests
{
    public class StudyServiceTests
    {
        [Fact]
        public async Task CreateStudyAsync_ShouldCreateStudy_WhenDataIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);

            var currentUserMock = new Mock<ICurrentUserService>();

            currentUserMock.Setup(x => x.UserId).Returns(1);
            currentUserMock.Setup(x => x.IsAdmin).Returns(true);

            var service = new StudyService(context, currentUserMock.Object);

            context.Users.Add(new User
            {
                Id = 1,
                Nombre = "Test",
                Apellido = "User",
                Email = "test@test.com",
                Dni = "12345678A",
                Password = "1234"
            });

            await context.SaveChangesAsync();

            var studyDto = new StudyDTO
            {
                Nombre = "Test Study",
                Descripcion = "This is a test study.",
                UserId = 1
            };
            // Act
            await service.CreateStudyAsync(studyDto);

            // Assert
            var createdStudy = await context.Studies.FirstOrDefaultAsync(s => s.Nombre == "Test Study" && s.Descripcion == "This is a test study.");


            Assert.NotNull(createdStudy);
        }

        [Fact]
        public async Task CreateStudyAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);

            var currentUserMock = new Mock<ICurrentUserService>();

            var service = new StudyService(context, currentUserMock.Object);

            currentUserMock.Setup(x => x.IsAdmin).Returns(true);
            currentUserMock.Setup(x => x.UserId).Returns(1);
            // Act

            var studyDto = new StudyDTO
            {
                Nombre = "Test Study",
                Descripcion = "This is a test study.",
                UserId = 1
            };

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateStudyAsync(studyDto));

        }
    }
}
