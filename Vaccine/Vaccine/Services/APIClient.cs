using Newtonsoft.Json;
using System.Text;
using Vaccine.Models;
namespace Vaccine.Services
{
    public class APIClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;


        public APIClient(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;

            _httpClient = httpClientFactory.CreateClient("VACCINE_API");
        }

        public async Task<string> HandleApiResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(); // Return response content directly as string
            }
           
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                LogErrorAsync($"Error: {response.StatusCode}. Details: {errorContent}", response.RequestMessage.RequestUri.ToString());
                throw new Exception($"Error: {response.StatusCode}. Details: {errorContent}");
            }
        }
        public async Task<T> HandleApiResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                if (typeof(T) == typeof(int))
                {
                    if (int.TryParse(responseContent, out int result))
                    {
                        return (T)(object)result; // Cast result to T
                    }
                    throw new Exception($"Could not convert response to {typeof(T)}");
                }
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {response.StatusCode}. Details: {errorContent}");
            }
        }

        public async Task<string> LogErrorAsync(string errorMessage, string URL)
        {
            try
            {
                var exception = new ExceptionLoggerDTO
                {
                    Message = errorMessage,
                    URL = URL,
                    //RoleId = userData?.RoleId.ToString() ?? null,  // Set RoleId to null if userData or RoleId is null
                   // UserId = userData?.UserId ?? null,
                    HostName = System.Net.Dns.GetHostName().ToString(),
                    TargetSite = "",
                    StackTrace = "",
                    Source = ""
                };
                var content = new StringContent(JsonConvert.SerializeObject(exception), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Request/LogError", content);
                return await response.Content.ReadAsStringAsync(); // Return response content directly as string
            }
            catch (Exception e) { return null; }
        }

        public async Task<List<PatientDTO>> LoadPatientsViewAsync()
        {
            var response = await _httpClient.GetAsync($"Vaccine/GetPatientsView");
            return await HandleApiResponse<List<PatientDTO>>(response);
        }
        public async Task<List<NationalityDTO>> LoadNationalityListAsync()
        {
            var response = await _httpClient.GetAsync($"Vaccine/GetNationalityList");
            return await HandleApiResponse<List<NationalityDTO>>(response);
        }
        public async Task<List<VaccineDTO>> LoadVaccineListAsync()
        {
            var response = await _httpClient.GetAsync($"Vaccine/GetVaccineList");
            return await HandleApiResponse<List<VaccineDTO>>(response);
        }
        public async Task<List<PatientDTO>> SearchPatientAsync(PatientDTO _patient)
        {
            // Serialize _patient into JSON to send as POST data
            var content = new StringContent(JsonConvert.SerializeObject(_patient), Encoding.UTF8, "application/json");

            // Send POST request with patient data
            var response = await _httpClient.PostAsync("Vaccine/SearchPatient", content);

            // Handle the response and return the patient data
            return await HandleApiResponse<List<PatientDTO>>(response);
        }
        public async Task<List<VaccinationViewDTO>> GetVaccinationViewAsync(PatientDTO _patient)
        {
            // Serialize _patient into JSON to send as POST data
            var content = new StringContent(JsonConvert.SerializeObject(_patient), Encoding.UTF8, "application/json");

            // Send POST request with patient data
            var response = await _httpClient.PostAsync("Vaccine/GetVaccinationView", content);

            // Handle the response and return the patient data
            return await HandleApiResponse<List<VaccinationViewDTO>>(response);
        }
        public async Task<API_Response> SaveVaccinationRecordAsync(VaccinationRecordDTO VacRec)
        {
            // Serialize _patient into JSON to send as POST data
            var content = new StringContent(JsonConvert.SerializeObject(VacRec), Encoding.UTF8, "application/json");

            // Send POST request with patient data
            var response = await _httpClient.PostAsync("Vaccine/SaveVaccinationRecord", content);

            // Handle the response and return the patient data
            return await HandleApiResponse<API_Response>(response);

        }
        public async Task<VaccinationRecordDTO> GetVaccinationRecordAsync(int OrderID)
        {
            var response = await _httpClient.GetAsync($"Vaccine/GetVaccinationRecord?OrderID={OrderID}");
            return await HandleApiResponse<VaccinationRecordDTO>(response);
        }
        public async Task<API_Response> DeleteVaccinationRecordAsync(int OrderID)
        {
            var response = await _httpClient.GetAsync($"Vaccine/DeleteVaccinationRecord?OrderID={OrderID}");
            return await HandleApiResponse<API_Response>(response);
        }
    }
}
