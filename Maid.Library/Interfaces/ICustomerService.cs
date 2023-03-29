using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ICustomerService
{
    Task<CleaningContract> GetCleaningDetailsById(int contractId);
    public Task<IEnumerable<CleaningContract>> GetUpcomingAppointments(int customerId);
    public Task<IEnumerable<CleaningContract>> GetAllAppointments(int id);
}
