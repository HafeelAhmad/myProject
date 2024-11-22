using System.ComponentModel.DataAnnotations;

namespace Vaccine.Models
{
    public class ExceptionLoggerDTO
    {
        [Key]
        public int ExceptionID { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string TargetSite { get; set; }
        public string? Endpoint { get; set; }
        public string URL { get; set; }
        public string HostName { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
