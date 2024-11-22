using System.ComponentModel.DataAnnotations;

namespace Vaccine_API.Models
{
    public class _Nationality
    {
        [Key]
        public int NationalityID { get; set; }
        public string Nationality { get; set; }
        private bool Suspended { get; set; }

    }
}
