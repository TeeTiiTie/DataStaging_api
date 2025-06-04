using AutoMapper;
using DataStaging.Models;
using DataStaging_api.DTOs;
using DataStaging_api.DTOs.DataLead;

namespace DataStaging_api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /*
             * CreateMap<SampleMessage, ExampleModels>()
             *     .ForMember(_ => _.ExampleName, _ => _.MapFrom(_ => _.Name))
             *     .ReverseMap();
             *
             * CreateMap<ExampleModels, GetExampleReponseDto>();
             */

            CreateMap<usp_DataLeadBirthday_InsertResult, ResponseFromStored>();
            CreateMap<DataLeadHeader, DataLeadHeaderDto>();
        }
    }
}