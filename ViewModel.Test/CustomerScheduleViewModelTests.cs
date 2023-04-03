using FluentAssertions;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.ViewModels;
using Moq;
using Syncfusion.Maui.Scheduler;

namespace ViewModel.Test;

public class CustomerScheduleViewModelTests
{
    private Mock<ICustomerService> mockCustomer;
    private CustomerScheduleViewModel vm;

    [SetUp]
    public void Setup()
    {
        mockCustomer = new Mock<ICustomerService>();
        vm = new CustomerScheduleViewModel(mockCustomer.Object);
    }

    [Test]
    public void TranslationWithNoItemsReturned()
    {
        SetUpGetAllAppoinments(new List<CleaningContract> { });
        SetUpIsScheduled(true);
        vm.AppearCommand.ExecuteAsync(null);
        vm.Appointments.Count.Should().Be(0);
    }

    [Test]
    public void TranslationWithOneItemReturned()
    {
        SetUpGetAllAppoinments(new List<CleaningContract>
                                      {
                new CleaningContract
                {
                    Id = 1,
                    ScheduleDate = new DateTime(2023, 03, 02),
                    Location = new CleaningLocation
                    {
                        Address = "123 Main St"
                    },
                    CleaningType = new CleaningType { Type = "Deep Clean"},
                    RequestedHours = TimeSpan.FromHours(1)
                }
                    });
        SetUpIsScheduled(true);
        vm.AppearCommand.ExecuteAsync(null);

        vm.Appointments.Count.Should().Be(1);
        vm.Appointments[0]
            .Should().BeEquivalentTo(
                new SchedulerAppointment
                {
                    Id = 1,
                    Subject = "Deep Clean",
                    StartTime = new DateTime(2023, 03, 02),
                    IsAllDay = false,
                    Background = Brush.Green,
                    EndTime = new DateTime(2023, 03, 02) + TimeSpan.FromHours(1)
                }
            );
    }

    private void SetUpGetAllAppoinments(List<CleaningContract> list)
    {
        mockCustomer.Setup(x =>
                           x.GetAllAppointments(It.IsAny<int>())
                           ).ReturnsAsync(list);
    }

    private void SetUpIsScheduled(bool isScheduled)
    {
        mockCustomer.Setup(x =>
                   x.IsScheduled(It.IsAny<int>())
                          ).ReturnsAsync(isScheduled);
    }
}
