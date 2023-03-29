using FluentAssertions;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.Library.DbModels;
using MaidService.ViewModels;
using Moq;

namespace ViewModel.Test
{
    public class CustomerProfileTest
    {
        private Mock<ICustomerService> mockCustomer;
        private CustomerProfileViewModel vm;

        [SetUp]
        public void Setup()
        {
            mockCustomer = new Mock<ICustomerService>();
            vm = new CustomerProfileViewModel(mockCustomer.Object);
        }

        [Test]
        public void WhenNullModels_ReturnMessage()
        {
            mockCustomer.Setup(x => x.GetUpcomingAppointments(1)).ReturnsAsync(new List<CleaningContract> { });
            vm.AppearCommand.ExecuteAsync(null);
            vm.AppointmentsHeader.Should().Be("No Upcoming Appointments");
        }

        [Test]
        public void WhenModelHasOneItem_SetAppointmentToModel()
        {
            mockCustomer.Setup(x => x.GetUpcomingAppointments(1))
                .ReturnsAsync(new List<CleaningContract>
                { new CleaningContract
                        { Location = new Location
                            {
                                Address = "123 mains street"
                            }
                        , ScheduleDate = new DateTime(2023, 03, 02)
                        }
                });
            vm.AppearCommand.ExecuteAsync(null);
            vm.Appointments.Should().BeEquivalentTo(
                new List<AppointmentCardViewModel>
                { new AppointmentCardViewModel(
                    new CleaningContract {
                        Location = new Location { Address = "123 mains street" }
                        , ScheduleDate = new DateTime(2023, 03, 02)
                    })
                });
            vm.AppointmentsHeader.Should().Be("Upcoming Appointments");
        }
    }
}
// translation PostGrest.ModelResponse -> OurModelsResponse { models = New }