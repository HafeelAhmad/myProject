﻿using System.ComponentModel.DataAnnotations;

namespace Vaccine_API.Models
{
    public class Patient
    {
        [Key] 
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public DateTime DOB { get; set; }
        public int NationalityId { get; set; }
        public string? PassportNo { get; set; }
        public string? EmiratesID { get; set; }
        private bool Suspended {  get; set; }    

    }
}
