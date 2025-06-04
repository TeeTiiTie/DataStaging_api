using DataStaging_api.DTOs.DataLead;
using DataStaging_api.Models;
using DataStaging_api.Services.Lead;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataStaging_api.Controllers
{
    //TODO:[Authorize]
    [Route("api/datalead")]
    [ApiController]
    public class DataLeadController : ControllerBase
    {
        private readonly ILeadServices _leadServices;
        private readonly Serilog.ILogger? _logger;

        public DataLeadController(ILeadServices leadServices, Serilog.ILogger? logger = null)
        {
            _leadServices = leadServices;
            _logger = logger;
            _logger = logger?.ForContext("ServiceName", nameof(LeadServices)) ?? Serilog.Log.ForContext("ServiceName", nameof(LeadServices));
        }

        private const string _controllerName = nameof(DataLeadController);

        [HttpGet("header/{dataleadheaderid}", Name = "GetDataLeadHeaderById")]
        public async Task<ServiceResponse<DataLeadHeaderDto>> GetDataLeadHeaderById([FromRoute] Guid? dataleadheaderid)
        {
            const string functionName = nameof(GetDataLeadHeaderById);

            try
            {
                var result = await _leadServices.GetDataLeadHeader(dataleadheaderid);

                return ResponseResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[{ControllerName}] - {functionName} error ", _controllerName, functionName);
                return ResponseResult.Failure<DataLeadHeaderDto>("เกิดข้อผิดพลาดที่ระบบ กรุณาติดต่อแผนกพัฒนาระบบ");
            }
        }
    }
}