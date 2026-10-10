using Moq;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.ContractsBilling.Application.Service.Commands.Contracts;
using Nutricenter.ContractsBilling.Application.Service.DTOs;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using Nutricenter.ContractsBilling.Domain.Interfaces.Services;
using Nutricenter.ContractsBilling.Domain.ValueObjects;
using ContractAggregate = Nutricenter.ContractsBilling.Domain.Aggregate.Contract;

namespace Nutricenter.ContractsBilling.Test.Application.Commands.Contract
{
    public class ContractCreateCommandHandlerTest
    {
        private static ContractCreateCommand CreateCommand() => new()
        {
            PacienteId = Guid.NewGuid(),
            FechaInicio = new DateOnly(2026, 1, 1),
            FechaFin = new DateOnly(2026, 12, 31),
            Servicios = new List<ContractServiceCreateDTO>
            {
                new(Guid.NewGuid(), "SVC-001", "Hoteleria", 100m, 1, 30)
            }
        };

        [Fact]
        public async Task Handle_WithServices_ShouldCreateContractAndCommit()
        {
            // Arrange
            var repositoryMock = new Mock<IContractRepository>();
            var numberGeneratorMock = new Mock<IContractNumberGenerator>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            numberGeneratorMock
                .Setup(x => x.GenerateAsync(It.IsAny<string>()))
                .ReturnsAsync(new SequenceNumberGenerator("CTR-2026-00001"));
            ContractAggregate? created = null;
            repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<ContractAggregate>()))
                .Callback<ContractAggregate>(c => created = c)
                .Returns(Task.CompletedTask);
            var handler = new ContractCreateCommandHandler(
                repositoryMock.Object, numberGeneratorMock.Object, unitOfWorkMock.Object);
            var command = CreateCommand();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
            Assert.NotNull(created);
            Assert.Single(created.Services);
            numberGeneratorMock.Verify(x => x.GenerateAsync("CTR"), Times.Once);
            repositoryMock.Verify(x => x.CreateAsync(It.IsAny<ContractAggregate>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
