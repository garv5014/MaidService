using FluentAssertions;
using Maid.Library.Interfaces;
using MaidService.DbModels;
using MaidService.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Test
{
    public class OrderDetailsTest
    {
        private Mock<ISupabaseService> mockSupabase;
        private OrderDetailsViewModel vm;

        [SetUp]
        public void SetUp()
        {
            mockSupabase = new Mock<ISupabaseService>();
            vm = new OrderDetailsViewModel(mockSupabase.Object);
        }

        [Test]
        public void WhenNullModels_ReturnMessage()
        {
            mockSupabase.Setup(x => x.GetTable<CleaningContract>())
                .ReturnsAsync(new MyModelResponse<CleaningContract> { Models = null });
            vm.AppearCommand.ExecuteAsync(null);
            vm.CleanerName.Should().BeNull();
            vm.Price.Should().BeNull();
            vm.ScheduledTime.Should().BeNull();
            vm.CleaningType.Should().BeNull();
            vm.Location.Should().BeNull();
            vm.Notes.Should().BeNull();
        }

        [Test]
        public void WhenModelsHasOneItem_SetProperties()
        {
            mockSupabase.Setup(x => x.GetTable<CleaningContract>())
                .ReturnsAsync(new MyModelResponse<CleaningContract>
                {
                    Models = new List<CleaningContract> {
                        new CleaningContract() {
                            ScheduleDate = new DateTime(2023, 03, 26, 11, 30, 00)
                            , Cost = "50.00"
                            , Location = new Location { Address = "123 Main St" }
                            , CleaningType = new CleaningType {Type = "Maintenance"}
                            , Notes = "This is a note."
                        }
                    }
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
