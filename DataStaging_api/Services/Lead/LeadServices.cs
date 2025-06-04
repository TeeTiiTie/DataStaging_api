using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataStaging.Data;
using DataStaging_api.DTOs;
using DataStaging_api.DTOs.DataLead;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TelesaleContract;

namespace DataStaging_api.Services.Lead
{
    public class LeadServices : ILeadServices
    {
        private readonly AppDBContext _dBContext;
        private readonly IPublishEndpoint _publish;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger? _logger;

        public LeadServices(AppDBContext dBContext, IPublishEndpoint publish, IMapper mapper, Serilog.ILogger? logger = null)
        {
            _dBContext = dBContext;
            _publish = publish;
            _mapper = mapper;
            _logger = logger;
            _logger = logger?.ForContext("ServiceName", nameof(LeadServices)) ?? Serilog.Log.ForContext("ServiceName", nameof(LeadServices));
        }

        private const string _serviceName = nameof(LeadServices);

        public async Task<ResponseFromStored> LeadBirthday()
        {
            string functionName = nameof(LeadBirthday);

            var now = DateTime.Now;
            _logger.Information("{serviceName} {functionName} Stored procedure: usp_DataLeadBirthday_Insert, Param: BirthDate={now}", _serviceName, functionName, now);
            var result = await _dBContext.Procedures.usp_DataLeadBirthday_InsertAsync(now);
            _logger.Information("{serviceName} {functionName} Stored procedure: usp_DataLeadBirthday_Insert, Result: {@result}", _serviceName, functionName, result);

            var response = _mapper.Map<ResponseFromStored>(result.FirstOrDefault());

            return response;
        }

        public async Task PublishDataLeadHeader(Guid? dataLeadHeaderId)
        {
            string functionName = nameof(PublishDataLeadHeader);

            var dataLead = await _dBContext.DataLeadHeaders.FirstOrDefaultAsync(x => x.IsActive == true && x.DataLeadHeaderId == dataLeadHeaderId);

            if (dataLead == null)
            {
                _logger.Information("{serviceName} {functionName} data not found.", _serviceName, functionName);
                return;
            }

            CreateDataLeadHeader pub = new()
            {
                DataLeadHeaderId = dataLead.DataLeadHeaderId,
                DataLeadHeaderCode = dataLead.DataLeadHeaderCode,
                DataLeadHeaderName = dataLead.DataLeadHeaderName,
                DataLeadHeaderTypeId = dataLead.DataLeadHeaderTypeId,
                ItemCount = dataLead.ItemCount,
            };

            await _publish.Publish(pub);
            _logger.Information("{serviceName} {functionName} publish CreateDataLeadHeader. Message:{@pub}", _serviceName, functionName, pub);
        }

        public async Task<DataLeadHeaderDto?> GetDataLeadHeader(Guid? dataLeadHeaderId)
        {
            var qry = _dBContext.DataLeadHeaders.Where(x => x.IsActive == true && x.DataLeadHeaderId == dataLeadHeaderId).AsNoTracking();

            var result = await qry.ProjectTo<DataLeadHeaderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            return result;
        }
    }
}