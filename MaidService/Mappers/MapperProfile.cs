using AutoMapper;
using MaidService.Library.DbModels;

namespace MaidService.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    { 
        CreateMap<CleanerModel, Cleaner>().ReverseMap();
        CreateMap<CleanerReviewCustomersModel, CleanerReviewCustomers>().ReverseMap();
        CreateMap<CleaningContractModel, CleaningContract>().ReverseMap();
        CreateMap<CleaningContractModelNoCleaners, CleaningContract>().ReverseMap();
        CreateMap<CleaningTypeModel, CleaningType>().ReverseMap();
        CreateMap<CleanerAssignmentsModel, CleanerAssignments>().ReverseMap();
        CreateMap<CustomerModel, Customer>().ReverseMap();
        CreateMap<CustomerPaymentModel, CustomerPayment>().ReverseMap();
        CreateMap<CustomerReviewCleanerModel, CustomerReviewCleaners>().ReverseMap();
        CreateMap<DayTemplateModel, DayTemplate>().ReverseMap();
        CreateMap<LocationModel, Library.DbModels.CleaningLocation>().ReverseMap();
        CreateMap<ScheduleModel, Schedule>().ReverseMap();
    }
}