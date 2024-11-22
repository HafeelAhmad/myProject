namespace Vaccine.Models
{
    public class Fill_Vaccine
    {
        public List<NationalityDTO> NationalityList { get; set; }
        public List<VaccineDTO> VaccineList { get; set; }
        public List<VaccinationViewDTO> VaccinationRecordList { get; set; }
        public PatientDTO Patient { get; set; }
        public int OrderID { get; set; } // Vaccination record ID
        public int PatientID { get; set; } // Patient ID
        public string PatientName { get; set; } // Patient's name
        public string EmiratesID { get; set; } // Patient's Emirates ID
        public string PassportNo { get; set; } // Patient's passport number
        public int NationalityID { get; set; }
        public DateTime? DOB { get; set; }
        public string Nationality { get; set; } // Patient's nationality
        public int VaccineID { get; set; } // Vaccine ID
        public string VaccineName { get; set; } // Vaccine name
        public string VaccineType { get; set; } // Type of vaccine
        public int TotalDoses { get; set; } // Total number of doses for the vaccine
        public DateTime? VaccinationDate { get; set; } // Date of vaccination
        public int DoseNumber { get; set; } // Dose number (1st, 2nd, etc.)
        public string AddedBy { get; set; } // Person who added the record
        public string? Notes { get; set; } // Additional notes (optional)
    }
}
