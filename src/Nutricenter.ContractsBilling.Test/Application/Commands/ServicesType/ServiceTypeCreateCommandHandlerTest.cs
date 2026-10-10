using Moq;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;

namespace Nutricenter.ContractsBilling.Test.Application.Commands.ServicesType
{
    public class ServiceTypeCreateCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateServiceTypeAndCommit()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceTypeRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ServiceTypeCreateCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);
            var command = new ServiceTypeCreateCommand("HOTE", "Hoteleria");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            repositoryMock.Verify(x => x.CreateAsync(It.Is<ServiceType>(i => i.Codigo == command.Codigo && i.Nombre == command.Nombre)), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once);
        }
    }
}
