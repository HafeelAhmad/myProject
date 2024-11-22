using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vaccine.Models;
using Vaccine.Services;

namespace Vaccine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly APIClient _client;
        private readonly IValidator<VaccinationRecordDTO> _validator;
        public HomeController(ILogger<HomeController> logger, APIClient aPIClient, IValidator<VaccinationRecordDTO> validator)
        {
            _logger = logger;
            _client = aPIClient;
            _validator = validator;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> FillPatient(int PatientID)
        {
            Fill_Vaccine fill_Vaccine = new Fill_Vaccine();
            fill_Vaccine.NationalityList = await _client.LoadNationalityListAsync();
            fill_Vaccine.VaccineList = await _client.LoadVaccineListAsync();
            fill_Vaccine.VaccinationDate = null;
            fill_Vaccine.DOB = null;
            if (PatientID > 0)
            {
                fill_Vaccine.VaccinationDate = DateTime.Today;
                var patient = new PatientDTO();
                patient.PatientId = PatientID;
                var Patient = (await _client.SearchPatientAsync(patient)).FirstOrDefault();
                if (Patient != null)
                {
                    fill_Vaccine.PatientID = Patient.PatientId;
                    fill_Vaccine.PatientName = Patient.PatientName;
                    fill_Vaccine.NationalityID = Patient.NationalityID;
                    fill_Vaccine.DOB = Patient.DOB;
                    fill_Vaccine.EmiratesID = Patient.EmiratesID;
                    fill_Vaccine.PassportNo = Patient.PassportNo;
                }
                var PatVaccine = await _client.GetVaccinationViewAsync(patient);
                fill_Vaccine.VaccinationRecordList = PatVaccine;
            }
            return PartialView("_PartialPatientDetails", fill_Vaccine);
        }
        public async Task<IActionResult> LoadPatients()
        {
            var patients = await _client.LoadPatientsViewAsync();
            return PartialView("_PartialPatientList", patients);
        }
        public async Task<IActionResult> PatientVaccination()
        {
            var patients = await _client.LoadPatientsViewAsync();
            return View("PatientVaccination", patients);
        }
        public async Task<IActionResult> GetVaccination(int PatientID)
        {
            Fill_Vaccine fill_Vaccine = new Fill_Vaccine();
            var patient = new PatientDTO();
            patient.PatientId = PatientID;
            var Patient = (await _client.SearchPatientAsync(patient)).FirstOrDefault();
            var PatVaccine = await _client.GetVaccinationViewAsync(patient);
            fill_Vaccine.Patient = Patient;
            fill_Vaccine.VaccinationRecordList = PatVaccine;
            var patients = await _client.LoadPatientsViewAsync();
            return PartialView("_PartialPatVaccineView", fill_Vaccine);
        }
        public async Task<IActionResult> SaveVaccination(VaccinationRecordDTO VaccRec)
        {
            try
            {
                var validator = new VaccinationRecordDTOValidator();
                var validationResult = await validator.ValidateAsync(VaccRec);

                if (!validationResult.IsValid)
                {
                    // Return validation errors
                    return Json(new
                    {
                        ErrorCode = -1,
                        Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    });
                }
                VaccRec.AddedBy = "Benila";
                // Assuming _client has methods for saving data
                API_Response result = await _client.SaveVaccinationRecordAsync(VaccRec);
                return Json(new { ErrorCode = result.ErrorCode, Message = result.Message });
            }
            catch (Exception ex)
            {
                // Log the error (if logging is implemented)
                Console.WriteLine(ex.Message);
                return Json(new { ErrorCode = 0, Message = ex.Message });
            }
        }

        public async Task<IActionResult> GetVaccinationRecord(int OrderID)
        {
            var order = await _client.GetVaccinationRecordAsync(OrderID);
            return Json(new
            {
                order.OrderID,
                order.VaccineID,
                order.DoseNumber,
                VaccinationDate = order.VaccinationDate.ToString("yyyy-MM-dd"),
                order.Notes
            });
        }

        public async Task<IActionResult> DeleteVaccinationRecord(int OrderID)
        {
            var result = await _client.DeleteVaccinationRecordAsync(OrderID);
            return Json(new { ErrorCode = result.ErrorCode, Message = result.Message });
        }
    }
}
