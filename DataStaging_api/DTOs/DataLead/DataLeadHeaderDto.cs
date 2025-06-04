using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DataStaging_api.DTOs.DataLead
{
    public class DataLeadHeaderDto
    {
        public Guid? DataLeadHeaderId { get; set; }
        public string? DataLeadHeaderCode { get; set; }
        public string? DataLeadHeaderName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}