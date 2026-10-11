using Moq;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;

namespace Nutricenter.ContractsBilling.Test.Application.Commands.ServicesType
{
    public class ServiceTypeCreateCommandHandlerTest
    {
        private static ServiceTypeCreateCommand CreateCommand(
            string codigo = "HOTE",
            string nombre = "Hoteleria",
            bool isActive = true)
        {
            return new ServiceTypeCreateCommand(codigo, nombre, isActive);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateServiceTypeAndCommit()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceTypeRepository>();
            ServiceType? created = null;
            repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ServiceType>()))
                .Callback<ServiceType>(s => created = s)
                .Returns(Task.CompletedTask);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ServiceTypeCreateCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);
            var command = CreateCommand();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(created);
            Assert.Equal(command.Codigo, created.Codigo);
            Assert.Equal(command.Nombre, created.Nombre);
            Assert.True(created.IsActive);
            Assert.Equal(created.Id, result.Value);
            repositoryMock.Verify(
                x => x.CreateAsync(It.Is<ServiceType>(s =>
                    s.Codigo == command.Codigo
                    && s.Nombre == command.Nombre
                    && s.IsActive == command.IsActive)),
                Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InactiveValidCommand_ShouldPersistDisabledServiceType()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceTypeRepository>();
            ServiceType? created = null;
            repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ServiceType>()))
                .Callback<ServiceType>(s => created = s)
                .Returns(Task.CompletedTask);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ServiceTypeCreateCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);
            var command = CreateCommand(isActive: false);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(created);
            Assert.False(created.IsActive);
            repositoryMock.Verify(
                x => x.CreateAsync(It.Is<ServiceType>(s => !s.IsActive)),
                Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_BlankCodigo_ShouldThrowDomainExceptionWithoutCommit()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceTypeRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ServiceTypeCreateCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);
            var command = CreateCommand(codigo: "   ");

            // Act
            await Assert.ThrowsAsync<DomainException>(
                () => handler.Handle(command, CancellationToken.None));

            // Assert
            repositoryMock.Verify(x => x.CreateAsync(It.IsAny<ServiceType>()), Times.Never);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
