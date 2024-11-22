using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Vaccine.Models
{
    public class VaccinationRecordDTO
    {
        [Key]
        public int OrderID { get; set; } // Auto-incremented ID for each vaccination record
        public int PatientID { get; set; } // Foreign key to Patient table
        public int VaccineID { get; set; } // Foreign key to Vaccine_Master table
        public DateTime VaccinationDate { get; set; } // Date of vaccination
        public int DoseNumber { get; set; } // Dose number (1st, 2nd, etc.)
        public string? AddedBy { get; set; } // Person who added the record
        public DateTime? AddedDate { get; set; } // Date when the record was added (nullable)
        public string? Notes { get; set; } // Additional notes (optional)
        public bool? Suspended { get; set; } // Indicates if the record is suspended (nullable)
    }


public class VaccinationRecordDTOValidator : AbstractValidator<VaccinationRecordDTO>
    {
        public VaccinationRecordDTOValidator()
        {

            RuleFor(x => x.PatientID)
                .NotEmpty().WithMessage("Patient ID is required.");

            RuleFor(x => x.VaccineID)
                .NotEqual(0).WithMessage("Please select a Vaccine.");

            RuleFor(x => x.DoseNumber)
                .NotEqual(0).WithMessage("Please select a Dose.");

            RuleFor(x => x.VaccinationDate)
                .NotEmpty().WithMessage("Vaccination Date is required.")
                .LessThan(DateTime.Now).WithMessage("Vaccination Date must be in the past.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters.");
        }
    }

}
