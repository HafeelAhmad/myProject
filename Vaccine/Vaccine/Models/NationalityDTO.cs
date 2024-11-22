using System.ComponentModel.DataAnnotations;

namespace Vaccine.Models
{
    public class NationalityDTO
    {
        [Key]
        public int NationalityID { get; set; }
        public string Nationality { get; set; }
        private bool Suspended { get; set; }

    }
}
