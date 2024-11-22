using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vaccine_API.Data;
using Vaccine_API.Models;
namespace Vaccine_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VaccineController : ControllerBase
    {
        private readonly APPDBContext _db;

        // Constructor to inject the ApplicationDbContext
        public VaccineController(APPDBContext db)
        {
            _db = db; // Assign the injected DbContext to _db
        }
        [HttpGet("GetPatientsList")]

        public ActionResult GetPatientsList()
        {
            var patients = _db.Patient_Master.ToList();
            return Ok(patients);
        }
        [HttpGet("GetPatientsView")]

        public ActionResult GetPatientsView()
        {
            var patients = _db.Patient_View.ToList();
            return Ok(patients);
        }
        [HttpPost("SearchPatient")]
        public ActionResult SearchPatient(Patient _pat)
        {
            // Start with the queryable list of patients
            var query = _db.Patient_View.AsQueryable();

            // Apply filters based on the provided values
            if (!string.IsNullOrEmpty(_pat.PatientName))
            {
                query = query.Where(x => x.PatientName.Contains(_pat.PatientName));
            }

            if (!string.IsNullOrEmpty(_pat.EmiratesID))
            {
                query = query.Where(x => x.EmiratesID == _pat.EmiratesID);
            }

            if (!string.IsNullOrEmpty(_pat.PassportNo))
            {
                query = query.Where(x => x.PassportNo == _pat.PassportNo);
            }

            if (_pat.PatientId != 0)
            {
                query = query.Where(x => x.PatientId == _pat.PatientId);
            }

            // Execute the query and return the result
            var patients = query.ToList();

            return Ok(patients);
        }
        [HttpGet("GetNationalityList")]

        public ActionResult GetNationalityList()
        {
            var nationalities = _db.Nationality_Master.ToList();
            return Ok(nationalities);
        }
        [HttpGet("GetVaccineList")]

        public ActionResult GetVaccineList()
        {
            var nationalities = _db.Vaccine_Master.ToList();
            return Ok(nationalities);
        }
        [HttpGet("GetVaccinationRecord")]

        public ActionResult GetVaccinationRecord(int OrderID)
        {
            var VacRec = _db.Vaccination_Record.Find(OrderID);
            return Ok(VacRec);
        }
        [HttpPost("GetVaccinationView")]
        public ActionResult GetVaccinationView(Patient _pat)
        {
            // Start with the queryable list of patients
            var query = _db.Vaccination_View.AsQueryable();

            // Apply filters based on the provided values
            if (_pat.PatientId != 0)
            {
                query = query.Where(x => x.PatientID == _pat.PatientId);
            }

            if (!string.IsNullOrEmpty(_pat.EmiratesID))
            {
                query = query.Where(x => x.EmiratesID == _pat.EmiratesID);
            }

            if (!string.IsNullOrEmpty(_pat.PassportNo))
            {
                query = query.Where(x => x.PassportNo == _pat.PassportNo);
            }
            // Execute the query and return the result
            var vaccinations = query.Where(x => x.Suspended == false).ToList();

            return Ok(vaccinations);
        }

        [HttpPost("SaveVaccinationRecord")]
        public async Task<IActionResult> SaveVaccinationRecord(VaccinationRecord formData)
        {
            string ErrorCode = "0", Message = "No record updated";
            if (formData == null)
            {
                return BadRequest("Invalid form data");
            }

            // Save the form data to the database
            if (formData.OrderID == 0)
            {
                var vac = _db.Vaccination_Record.Where(x => x.PatientID == formData.PatientID && x.VaccineID == formData.VaccineID && x.DoseNumber == formData.DoseNumber && x.Suspended == false).FirstOrDefault();
                if (vac == null)
                {
                    var VacRec = new VaccinationRecord()
                    {
                        PatientID = formData.PatientID,
                        VaccineID = formData.VaccineID,
                        VaccinationDate = formData.VaccinationDate,
                        DoseNumber = formData.DoseNumber,
                        Notes = formData.Notes,
                        AddedBy = formData.AddedBy,
                        AddedDate = DateTime.Now,
                        Suspended = false
                    };
                    _db.Entry(VacRec).State = EntityState.Added;
                    int RowsEffected = await _db.SaveChangesAsync();

                    if (RowsEffected > 0)
                    {
                        ErrorCode = "1";
                        Message = "Saved successfully.";
                    }
                }
                else
                {
                    ErrorCode = "2";
                    Message = "Vaccination is already taken";
                }
            }
            else
            {
                var VacRec = _db.Vaccination_Record.Where(x => x.PatientID == formData.PatientID && x.VaccineID == formData.VaccineID && x.DoseNumber == formData.DoseNumber && x.Suspended == false && x.OrderID != formData.OrderID).FirstOrDefault();
                if (VacRec == null)
                {
                    var _VacRec = _db.Vaccination_Record.Find(formData.OrderID);
                    if (_VacRec == null)
                        return NotFound();
                    _VacRec.DoseNumber = formData.DoseNumber;
                    _VacRec.VaccineID = formData.VaccineID;
                    _VacRec.VaccinationDate = formData.VaccinationDate;
                    _VacRec.Notes = formData.Notes;
                    _VacRec.ModifiedBy = formData.AddedBy;
                    _VacRec.ModifiedDate = DateTime.Now;

                    int RowsEffected = await _db.SaveChangesAsync();
                    if (RowsEffected > 0)
                    {
                        ErrorCode = "3";
                        Message = "Updated successfully.";
                    }
                }
                else
                {
                    ErrorCode = "2";
                    Message = "Vaccination is already taken";
                }
            }
            return Ok(new { ErrorCode = ErrorCode, Message = Message });
        }

        [HttpGet("DeleteVaccinationRecord")]
        public async Task<IActionResult> DeleteVaccinationRecord(int OrderID)
        {
            string ErrorCode = "0", Message = "No record updated";

            // Save the form data to the database
         
                var vac = _db.Vaccination_Record.Where(x => x.OrderID == OrderID && x.Suspended == false).FirstOrDefault();
                if (vac != null)
                {
                    var _VacRec = _db.Vaccination_Record.Find(OrderID);
                    if (_VacRec == null)
                        return NotFound();
                    _VacRec.Suspended = true;
                    _VacRec.ModifiedBy = "Benila";
                    _VacRec.ModifiedDate = DateTime.Now;

                    int RowsEffected = await _db.SaveChangesAsync();
                    if (RowsEffected > 0)
                    {
                        ErrorCode = "3";
                        Message = "Removed successfully.";
                    }
                }

            return Ok(new { ErrorCode = ErrorCode, Message = Message });
        }
    }
}
