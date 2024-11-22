using System.ComponentModel.DataAnnotations;

namespace Vaccine.Models
{
    public class PatientDTO
    {
        [Key] 
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; }
        public int NationalityID { get; set; }
        public string PassportNo { get; set; }
        public string EmiratesID { get; set; }
        private bool Suspended {  get; set; }
        public string status { get; set; }
    }
}
