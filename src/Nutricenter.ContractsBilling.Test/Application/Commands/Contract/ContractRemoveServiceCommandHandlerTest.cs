using Moq;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.Commands.Contracts;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;
using ContractAggregate = Nutricenter.ContractsBilling.Domain.Aggregate.Contract;

namespace Nutricenter.ContractsBilling.Test.Application.Commands.Contract
{
    public class ContractRemoveServiceCommandHandlerTest
    {
        private static ContractAggregate CreateDraftContractWithService()
        {
            var contract = new ContractAggregate(
                Guid.NewGuid(),
                new SequenceNumberGenerator("CTR-2026-00001"),
                new ContractPeriod(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31)));
            contract.AddService(new ContractedService(
                Guid.NewGuid(),
                new ServiceReference("SVC-001", "Hoteleria"),
                new QuantityValue(1),
                new AmountValue(100m, "BOB"),
                new QuantityValue(30)));
            return contract;
        }

        private static ContractAggregate CreateActiveContractWithService()
        {
            var contract = CreateDraftContractWithService();
            contract.Activate();
            return contract;
        }

        private static ContractRemoveServiceCommand CreateCommand(Guid contractId, Guid contractedServiceId) =>
            new(contractId, contractedServiceId);

        [Fact]
        public async Task Handle_ExistingService_ShouldRemoveAndCommit()
        {
            // Arrange
            var contract = CreateDraftContractWithService();
            var serviceId = contract.Services.Single().Id;
            var repositoryMock = new Mock<IContractRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(contract.Id, It.IsAny<bool>()))
                .ReturnsAsync(contract);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ContractRemoveServiceCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(
                CreateCommand(contract.Id, serviceId), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            Assert.Empty(contract.Services);
            repositoryMock.Verify(x => x.GetByIdAsync(contract.Id, It.IsAny<bool>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UnknownContract_ShouldThrowDomainException()
        {
            // Arrange
            var repositoryMock = new Mock<IContractRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((ContractAggregate?)null);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ContractRemoveServiceCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);

            // Act + Assert
            await Assert.ThrowsAsync<DomainException>(() => handler.Handle(
                CreateCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None));
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ActiveContract_ShouldThrowDomainExceptionWithoutCommitting()
        {
            // Arrange
            var contract = CreateActiveContractWithService();
            var serviceId = contract.Services.Single().Id;
            var repositoryMock = new Mock<IContractRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(contract.Id, It.IsAny<bool>()))
                .ReturnsAsync(contract);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ContractRemoveServiceCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);

            // Act + Assert
            await Assert.ThrowsAsync<DomainException>(() => handler.Handle(
                CreateCommand(contract.Id, serviceId), CancellationToken.None));
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
