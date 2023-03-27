﻿using AutoMapper;
using MaidService.Library.DbModels;

namespace MaidService.Mappers;

public class MapperProfile : Profile
{
    MapperProfile()
    { 
        CreateMap<CleanerModel, Cleaner>().ReverseMap();
        CreateMap<CleanerReviewCustomersModel, CleanerReviewCustomers>().ReverseMap();
        CreateMap<CleaningContractModel, CleaningContract>().ReverseMap();
        CreateMap<CleaningTypeModel, CleaningType>().ReverseMap();
        CreateMap<ContractScheduleModel, ContractSchedule>().ReverseMap();
        CreateMap<CustomerModel, Customer>().ReverseMap();
        CreateMap<CustomerPaymentModel, CustomerPayment>().ReverseMap();
        CreateMap<CustomerReviewCleanerModel, CustomerReviewCleaners>().ReverseMap();
        CreateMap<DayTemplateModel, DayTemplate>().ReverseMap();
        CreateMap<LocationModel, Library.DbModels.Location>().ReverseMap();
        CreateMap<ScheduleModel, Schedule>().ReverseMap();
    }
}