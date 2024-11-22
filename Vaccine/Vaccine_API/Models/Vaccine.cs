using System.ComponentModel.DataAnnotations;

namespace Vaccine_API.Models
{
    public class Vaccine
    {
        [Key]
        public int VaccineID { get; set; } // Corresponds to VaccineID in the table
        public string VaccineName { get; set; } // Corresponds to VaccineName in the table
        public string VaccineType { get; set; } // Corresponds to VaccineType in the table
        public int TotalDoses { get; set; } // Corresponds to TotalDoses in the table
        public string Notes { get; set; } // Corresponds to Notes in the table
        public bool Suspended { get; set; } // Corresponds to Suspended in the table, nullable because it's BIT
    }
}
