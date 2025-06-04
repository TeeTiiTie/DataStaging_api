using DataStaging_api.Services.Lead;
using Quartz;

namespace DataStaging_api.HostedServices
{
    public class GenarateBirthDateJob : IJob
    {
        private readonly ILeadServices _leadServices;
        private readonly Serilog.ILogger? _logger;

        public GenarateBirthDateJob(ILeadServices leadServices, Serilog.ILogger? logger = null)
        {
            _leadServices = leadServices;
            _logger = logger;
            _logger = logger?.ForContext("ServiceName", nameof(LeadServices)) ?? Serilog.Log.ForContext("ServiceName", nameof(LeadServices));
        }

        private const string _jobName = nameof(GenarateBirthDateJob);

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.Information("{jobName} Job start.", _jobName);
            try
            {
                var result = await _leadServices.LeadBirthday();

                if (result.IsResult == false)
                {
                    _logger.Warning("{jobName} call stored is not successfull.", _jobName);
                    return;
                }

                Guid dataLeadHeaderId = Guid.Empty;

                var isGuid = Guid.TryParse(result.Result, out dataLeadHeaderId);

                if (isGuid == false)
                {
                    _logger.Warning("{jobName} can't parse id is Guid.", _jobName);
                    return;
                }

                await _leadServices.PublishDataLeadHeader(dataLeadHeaderId); // publish to telesale
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{jobName} job is failed.", _jobName);
            }
            finally
            {
                _logger.Information("{jobName} Job is done.", _jobName);
            }
        }
    }
}