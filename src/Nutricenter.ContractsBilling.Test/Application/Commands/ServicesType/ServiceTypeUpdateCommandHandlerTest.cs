using Moq;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;

namespace Nutricenter.ContractsBilling.Test.Application.Commands.ServicesType
{
    public class ServiceTypeUpdateCommandHandlerTest
    {
        private static readonly Guid UnknownServiceTypeId = new("11111111-2222-3333-4444-555555555555");

        private static ServiceType CreateServiceType()
        {
            return new ServiceType("HOTE", "Hoteleria");
        }

        private static ServiceTypeUpdateCommand CreateCommand(Guid id)
        {
            return new ServiceTypeUpdateCommand(
                id,
                "Hoteleria Renovada",
                "Hoteleria y hospedaje",
                "ms-hoteleria",
                "https://hoteleria.internal",
                "/api/rooms",
                IsService: true,
                IsActive: true);
        }

        [Fact]
        public async Task Handle_ExistingServiceType_ShouldUpdateAndCommit()
        {
            // Arrange
            var serviceType = CreateServiceType();
            var repositoryMock = new Mock<IServiceTypeRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(serviceType.Id, It.IsAny<bool>()))
                .ReturnsAsync(serviceType);
            ServiceType? updated = null;
            repositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<ServiceType>()))
                .Callback<ServiceType>(e => updated = e)
                .Returns(Task.CompletedTask);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ServiceTypeUpdateCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);
            var command = CreateCommand(serviceType.Id);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(serviceType.Id, result.Value);
            Assert.NotNull(updated);
            Assert.Equal(command.Nombre, updated.Nombre);
            Assert.Equal(command.Descripcion, updated.Descripcion);
            Assert.Equal(command.NombreMicroservicio, updated.NombreMicroservicio);
            Assert.Equal(command.BaseUrl, updated.BaseUrl);
            Assert.Equal(command.Endpoint, updated.Endpoint);
            Assert.Equal(command.IsService, updated.IsService);
            repositoryMock.Verify(
                x => x.UpdateAsync(It.Is<ServiceType>(e =>
                    e.Id == serviceType.Id
                    && e.Nombre == command.Nombre
                    && e.IsService == command.IsService)),
                Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UnknownServiceType_ShouldFailWithoutCommit()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceTypeRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(UnknownServiceTypeId, It.IsAny<bool>()))
                .ReturnsAsync((ServiceType?)null);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ServiceTypeUpdateCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);
            var command = CreateCommand(UnknownServiceTypeId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("ServiceType", result.Error.Code);
            Assert.Equal("ServiceType not found", result.Error.Description);
            Assert.Equal(ErrorType.Failure, result.Error.Type);
            repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ServiceType>()), Times.Never);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
