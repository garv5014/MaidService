using FluentAssertions;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.ViewModels;
using Moq;

namespace ViewModel.Test
{
    public class OrderDetailsTest
    {
        private Mock<ICustomerService> mockCustomer;
        private OrderDetailsViewModel vm;

        [SetUp]
        public void SetUp()
        {
            mockCustomer = new Mock<ICustomerService>();
            vm = new OrderDetailsViewModel(mockCustomer.Object);
        }

        [Test]
        public void WhenObjectisEmpty_ReturnMessage()
        {
            mockCustomer.Setup(x => x.GetCleaningDetailsById(1))
                .ReturnsAsync(new CleaningContract { Id = 0 });
            vm.AppearCommand.ExecuteAsync(null);
            vm.CleanerName.Should().BeNull();
            vm.Price.Should().BeNull();
            vm.ScheduledTime.Should().BeNull();
            vm.TypeOfCleaning.Should().BeNull();
            vm.Location.Should().BeNull();
            vm.Notes.Should().BeNull();
        }

        [Test]
        public void WhenContractExists_SetProperties()
        {
            mockCustomer.Setup(x => x.GetCleaningDetailsById(1))
                .ReturnsAsync(new CleaningContract 
                { 
                            ScheduleDate = new DateTime(2023, 03, 26, 11, 30, 00)
                            , Cost = "50.00"
                            , Location = new Location { Address = "123 Main St" }
                            , CleaningType = new CleaningType {Type = "Maintenance"}
                            , Notes = "This is a note."
                            , Id = 1

                });
            vm.AppearCommand.ExecuteAsync(null);

            vm.Price.Should().Be("$50.00");
            vm.ScheduledTime.Should().Be(new DateTime(2023, 03, 26, 11, 30, 00).ToShortTimeString());
            vm.TypeOfCleaning.Should().Be("Maintenance");
            vm.Location.Should().Be("123 Main St");
            vm.Notes.Should().Be("This is a note.");
        }
    }
}
