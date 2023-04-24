using FluentAssertions;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.ViewModels;
using Moq;
using System.Diagnostics.Contracts;

namespace ViewModel.Test;

public class OrderDetailsTest
{
    private Mock<INavService> mockNav;
    private CustomerOrderDetailsViewModel vm;

    [SetUp]
    public void SetUp()
    {
        mockNav = new Mock<INavService>();
        vm = new CustomerOrderDetailsViewModel(mockNav.Object);
    }

    [Test]
    public void WhenObjectisEmpty_ReturnMessage()
    {
        vm.ApplyQueryAttributes(new Dictionary<string, object> { {"Contract", new CleaningContract { Id = 1 } } });
        vm.CleanerName.Should().Be("No Cleaners Yet");
    }

    [Test]
    public void WhenContractExists_SetProperties()
    {
        vm.ApplyQueryAttributes(new Dictionary<string, object> { { "Contract", new CleaningContract
            {
                ScheduleDate = new DateTime(2023, 03, 26, 11, 30, 00)
                        ,
                Cost = "50.00"
                        ,
                Location = new CleaningLocation { Address = "123 Main St", City = "Manti", State = "Utah" }
                        ,
                CleaningType = new CleaningType { Type = "Maintenance" }
                        ,
                Notes = "This is a note."
                        ,
                Id = 1

            } } });

        vm.Contract.Cost.Should().Be("50.00");
        vm.Contract.ScheduleDate.Should().Be(new DateTime(2023, 03, 26, 11, 30, 00));
        vm.Contract.CleaningType.Type.Should().Be("Maintenance");
        vm.Contract.FullLocation.Should().Be("123 Main St, Manti, Utah");
        vm.Contract.Notes.Should().Be("This is a note.");
    }
}
