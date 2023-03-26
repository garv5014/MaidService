﻿using FluentAssertions;
using Maid.Library.Interfaces;
using MaidService.DbModels;
using MaidService.ViewModels;
using Moq;
using Postgrest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Test
{
    public class CustomerProfileTest
    {
        private Mock<ISupabaseService> mockSupabase;
        private CustomerProfileViewModel vm;

        [SetUp]
        public void Setup()
        {
            mockSupabase = new Mock<ISupabaseService>();
            vm = new CustomerProfileViewModel(mockSupabase.Object);
        }

        [Test]
        public void WhenNullModels_ReturnMessage()
        {
            mockSupabase.Setup(x => x.GetTable<CleaningContract>())
                .ReturnsAsync(new MyModelResponse<CleaningContract> { Models = null });
            vm.AppearCommand.ExecuteAsync(null);
            vm.Appointments.Should().BeNull();
            vm.AppointmentsHeader.Should().Be("No Upcoming Appointments");
        }

        [Test]
        public void WhenModelHasOneItem_SetAppointmentToModel()
        {
            mockSupabase.Setup(x => x.GetTable<CleaningContract>())
                .ReturnsAsync(new MyModelResponse<CleaningContract> {
                    Models = new List<CleaningContract> {
                        new CleaningContract {
                            Location = new Location { Address = "123 mains street" }, ScheduleDate = new DateTime(2023,03,02 ) } } });
            vm.AppearCommand.ExecuteAsync(null);
            vm.Appointments.Should().BeEquivalentTo(new List<CleaningContract> {
                new CleaningContract {
                    Location = new Location { Address = "123 mains street" }, ScheduleDate = new DateTime(2023, 03, 02) } });
            vm.AppointmentsHeader.Should().Be("Upcoming Appointments");
        }
    }
}