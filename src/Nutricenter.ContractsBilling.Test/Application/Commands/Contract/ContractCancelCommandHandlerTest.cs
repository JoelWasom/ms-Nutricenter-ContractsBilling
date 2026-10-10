using Moq;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.Commands.Contracts;
using Nutricenter.ContractsBilling.Domain.Enums;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using Nutricenter.ContractsBilling.Domain.ValueObjects;
using ContractAggregate = Nutricenter.ContractsBilling.Domain.Aggregate.Contract;

namespace Nutricenter.ContractsBilling.Test.Application.Commands.Contract
{
    public class ContractCancelCommandHandlerTest
    {
        private static ContractAggregate CreateDraftContract()
        {
            return new ContractAggregate(
                Guid.NewGuid(),
                new SequenceNumberGenerator("CTR-2026-00001"),
                new ContractPeriod(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31)));
        }

        [Fact]
        public async Task Handle_ExistingContract_ShouldCancelAndCommit()
        {
            // Arrange
            var contract = CreateDraftContract();
            var repositoryMock = new Mock<IContractRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(contract.Id, It.IsAny<bool>()))
                .ReturnsAsync(contract);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new ContractCancelCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(new ContractCancelCommand(contract.Id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            Assert.Equal(ContractStatus.Cancelled, contract.Estado);
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
            var handler = new ContractCancelCommandHandler(
                repositoryMock.Object, new Mock<IUnitOfWork>().Object);

            // Act + Assert
            await Assert.ThrowsAsync<DomainException>(() => handler.Handle(
                new ContractCancelCommand(Guid.NewGuid()), CancellationToken.None));
        }
    }
}
