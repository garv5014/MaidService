using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ICustomerService
{
    Task<CleaningContract> GetCleaningDetailsById(int contractId);
    public Task<IEnumerable<CleaningContract>> GetUpcomingAppointments();
    public Task<IEnumerable<CleaningContract>> GetAppoinmentsByUserId();

    // Option: Combine both functions to be one and pass in span of time.
}
