using System.ComponentModel.DataAnnotations;

namespace Vaccine_API.Models
{
    public class VaccinationRecord
    {
        [Key]
        public int OrderID { get; set; } 
        public int PatientID { get; set; } 
        public int VaccineID { get; set; } 
        public DateTime VaccinationDate { get; set; } 
        public int DoseNumber { get; set; } 
        public string? AddedBy { get; set; } 
        public DateTime? AddedDate { get; set; } 
        public string? Notes { get; set; } 
        public bool? Suspended { get; set; } 
        public string? ModifiedBy { get; set; } 
        public DateTime? ModifiedDate { get; set; } 
    }
}
