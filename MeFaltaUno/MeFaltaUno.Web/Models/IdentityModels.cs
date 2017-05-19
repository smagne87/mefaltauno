using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace MeFaltaUno.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string FacebookToken { get; set; }
        public string TwitterToken { get; set; }
        public string TwitterTokenSecret { get; set; }
        public string GoogleToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int? IdCountry { get; set; }
        public int? IdState { get; set; }
        public int? IdCity { get; set; }
        public string Password { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MeFaltaUnoEntitiesUsers", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("Player").Property(p => p.Id).HasColumnName("IdPlayer");
            modelBuilder.Entity<ApplicationUser>().ToTable("Player").Property(p => p.Id).HasColumnName("IdPlayer");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}