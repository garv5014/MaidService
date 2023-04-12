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

        public Mock<INavService> mockNavService;

        private CustomerProfileViewModel vm;

        [SetUp]
        public void Setup()
        {
            mockCustomer = new Mock<ICustomerService>();
            mockNavService = new Mock<INavService>();
            vm = new CustomerProfileViewModel(mockCustomer.Object, mockNavService.Object);
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
            mockCustomer.Setup(x => x.GetUpcomingAppointments(It.IsAny<int>()))
                .ReturnsAsync(new List<CleaningContract>
                { new CleaningContract
                        { Location = new CleaningLocation
                            {
                                Address = "123 mains street"
                            }
                        , ScheduleDate = new DateTime(2023, 03, 02)
                        }
                });
            mockCustomer.Setup(x => x.GetCurrentCustomer()).ReturnsAsync(new Customer { Id = 1 });

            vm.AppearCommand.ExecuteAsync(null);
            vm.Appointments.Should().BeEquivalentTo(
                new List<CustomerAppointmentCardViewModel>
                { new CustomerAppointmentCardViewModel(
                    new CleaningContract {
                        Location = new CleaningLocation { Address = "123 mains street" }
                        , ScheduleDate = new DateTime(2023, 03, 02)
                    }
                , mockNavService.Object
                )
                });
            vm.AppointmentsHeader.Should().Be("Upcoming Appointments");
        }
    }
}
// translation PostGrest.ModelResponse -> OurModelsResponse { models = New }