using DomainLayer.Entities.Appointment_Model;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using MediCareSecurity_IdentityManagementLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
    public class MediCareDbContext(DbContextOptions<MediCareDbContext> options) : IdentityDbContext<MediCareAppUser>(options)
    {
        public DbSet<MediCareAppUser>? MediCareAppUsers { get; set; }
        public DbSet<Doctor>? Doctors { get; set; }
        public DbSet<Patient>? Patients { get; set; }
        public DbSet<Appointment>? Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MediCareAppUser>().ToTable("Users", "Security");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Security");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security"); 
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Security");


            // Configuring many - to - many relationship using Appointment as the join entity
            modelBuilder.Entity<Appointment>()
                 .HasOne(a => a.Doctor)
                 .WithMany(d => d.Appointments)
                 .HasForeignKey(a => a.DoctorId);
          //.OnDelete(DeleteBehavior.Restrict)  

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);
          // .OnDelete(DeleteBehavior.Restrict); 

        }
    }
}
