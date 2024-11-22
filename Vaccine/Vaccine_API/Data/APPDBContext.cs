using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Vaccine_API.Models;
namespace Vaccine_API.Data
{
    public class APPDBContext : DbContext
    {
        public APPDBContext(DbContextOptions<APPDBContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patient_Master { get; set; }
        public DbSet<_Nationality> Nationality_Master { get; set; }
        public DbSet<Vaccine> Vaccine_Master { get; set; }
        public DbSet<VaccinationRecord> Vaccination_Record { get; set; }
        public DbSet<VaccinationView> Vaccination_View { get; set; }
        public DbSet<PatientView> Patient_View { get; set; }
    }

}
