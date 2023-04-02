using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ICustomerService
{
    Task<CleaningContract> GetCleaningDetailsById(int contractId);
    Task<IEnumerable<CleaningContract>> GetUpcomingAppointments(int customerId);
    Task<IEnumerable<CleaningContract>> GetAllAppointments(int id);
    Task<IEnumerable<CleaningType>> GetCleaningTypes();
    Task<bool> IsScheduled(int contractId);
}
