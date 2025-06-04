using DataStaging_api.DTOs;
using DataStaging_api.DTOs.DataLead;

namespace DataStaging_api.Services.Lead
{
    public interface ILeadServices
    {
        Task<DataLeadHeaderDto?> GetDataLeadHeader(Guid? dataLeadHeaderId);

        Task<ResponseFromStored> LeadBirthday();

        Task PublishDataLeadHeader(Guid? dataLeadHeaderId);
    }
}