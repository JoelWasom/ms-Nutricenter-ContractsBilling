using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Enums;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;

namespace Nutricenter.ContractsBilling.Test.Domain
{
    public class ContractTest
    {
        private static Contract CreateContract(string number = "CTR-2026-00001")
        {
            return new Contract(
                Guid.NewGuid(),
                new SequenceNumberGenerator(number),
                new ContractPeriod(
                    new DateOnly(2026, 1, 1),
                    new DateOnly(2026, 12, 31)));
        }

        private static ContractedService CreateService(
            string serviceId = "SVC-001",
            string description = "Hoteleria",
            decimal unitPrice = 100m)
        {
            return new ContractedService(
                Guid.NewGuid(),
                new ServiceReference(serviceId, description),
                new QuantityValue(1),
                new AmountValue(unitPrice, "BOB"),
                new QuantityValue(30));
        }

        [Fact]
        public void Constructor_ShouldStartInDraftState()
        {
            var contract = CreateContract();

            Assert.Equal(ContractStatus.Draft, contract.Estado);
            Assert.Empty(contract.Services);
        }

        [Fact]
        public void AddService_ToDraftContract_ShouldAddService()
        {
            var contract = CreateContract();
            var service = CreateService();

            contract.AddService(service);

            Assert.Single(contract.Services);
        }

        [Fact]
        public void AddService_DuplicateReference_ShouldThrowDomainException()
        {
            var contract = CreateContract();
            contract.AddService(CreateService("SVC-001", "Hoteleria"));

            var duplicate = CreateService("SVC-001", "Hoteleria");

            Assert.Throws<DomainException>(() => contract.AddService(duplicate));
        }

        [Fact]
        public void AddService_ToNonDraftContract_ShouldThrowDomainException()
        {
            var contract = CreateContract();
            contract.Cancelled();

            Assert.Throws<DomainException>(() => contract.AddService(CreateService()));
        }

        [Fact]
        public void RemoveService_ExistingService_ShouldRemove()
        {
            var contract = CreateContract();
            var service = CreateService();
            contract.AddService(service);

            contract.RemoveService(service.Id);

            Assert.Empty(contract.Services);
        }

        [Fact]
        public void RemoveService_UnknownId_ShouldThrowDomainException()
        {
            var contract = CreateContract();
            contract.AddService(CreateService());

            Assert.Throws<DomainException>(() => contract.RemoveService(Guid.NewGuid()));
        }

        [Fact]
        public void Activate_EmptyDraftContract_ShouldThrowDomainException()
        {
            var contract = CreateContract();

            Assert.Throws<DomainException>(() => contract.Activate());
        }

        [Fact]
        public void Activate_ContractWithServices_ShouldBecomeActive()
        {
            var contract = CreateContract();
            contract.AddService(CreateService());

            contract.Activate();

            Assert.Equal(ContractStatus.Active, contract.Estado);
        }

        [Fact]
        public void Cancelled_ShouldSetCancelled()
        {
            var contract = CreateContract();

            contract.Cancelled();

            Assert.Equal(ContractStatus.Cancelled, contract.Estado);
        }

        [Fact]
        public void Terminated_FromActive_ShouldSetTerminated()
        {
            var contract = CreateContract();
            contract.AddService(CreateService());
            contract.Activate();

            contract.Terminated();

            Assert.Equal(ContractStatus.Terminated, contract.Estado);
        }

        [Fact]
        public void Terminated_FromDraft_ShouldThrowDomainException()
        {
            var contract = CreateContract();

            Assert.Throws<DomainException>(() => contract.Terminated());
        }

        [Fact]
        public void Suspended_FromActive_ShouldSetSuspended()
        {
            var contract = CreateContract();
            contract.AddService(CreateService());
            contract.Activate();

            contract.Suspended();

            Assert.Equal(ContractStatus.Suspended, contract.Estado);
        }

        [Fact]
        public void Suspended_FromDraft_ShouldThrowDomainException()
        {
            var contract = CreateContract();

            Assert.Throws<DomainException>(() => contract.Suspended());
        }

        [Fact]
        public void MontoTotal_ShouldSumUnitPrices()
        {
            var contract = CreateContract();
            contract.AddService(CreateService("SVC-001", "Hoteleria", 100m));
            contract.AddService(CreateService("SVC-002", "Limpieza", 50m));

            Assert.Equal(150m, contract.MontoTotal.Value);
        }
    }
}
