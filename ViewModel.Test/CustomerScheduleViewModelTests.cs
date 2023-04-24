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
    private Mock<ICleanerService> mockCleaner;
    private Mock<IAuthService> mockAuth;
    private Mock<INavService> mockNav;
    private CustomerScheduleViewModel vm;

    [SetUp]
    public void Setup()
    {
        mockCustomer = new Mock<ICustomerService>();
        mockCleaner = new Mock<ICleanerService>();
        mockAuth = new Mock<IAuthService>();
        mockNav = new Mock<INavService>();

        vm = new CustomerScheduleViewModel(mockCustomer.Object, mockCleaner.Object, mockAuth.Object, mockNav.Object );
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
        SetupGetCurrentCustomer();
        SetUpGetAllAppoinments(new List<CleaningContract>
                                      {
                new CleaningContract
                {
                    Id = 1,
                    ScheduleDate = new DateTime(2023, 03, 02),
                    Location =
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
        vm.Appointments[0].Id.Should().Be(1);
        vm.Appointments[0].Subject.Should().Be("Deep Clean");
        vm.Appointments[0].StartTime.Should().Be(new DateTime(2023, 03, 02));
        vm.Appointments[0].IsAllDay.Should().Be(false);
        vm.Appointments[0].Background.Should().Be(Brush.Green);
        vm.Appointments[0].EndTime.Should().Be(new DateTime(2023, 03, 02) + TimeSpan.FromHours(1));
    }

    private void SetupGetCurrentCustomer()
    {
        mockAuth.Setup(x =>
                           x.GetUserRole()
                           ).ReturnsAsync("Customer");
    }

    private void SetUpGetAllAppoinments(List<CleaningContract> list)
    {
        mockCustomer.Setup(x =>
                           x.GetAllAppointments()
                           ).ReturnsAsync(list);
    }

    private void SetUpIsScheduled(bool isScheduled)
    {
        mockCustomer.Setup(x =>
                   x.IsScheduled(It.IsAny<int>())
                          ).ReturnsAsync(isScheduled);
    }
}
