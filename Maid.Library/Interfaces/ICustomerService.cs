using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ICustomerService
{
    public Task<List<CleaningContract>> GetUpcomingAppointments();
}
