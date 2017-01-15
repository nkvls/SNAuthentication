using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SNAuthentication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string ReferredBy { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool SuryaTermsAccept { get; set; }
        public bool GoldenLotusTermsAccept { get; set; }
        public bool EarthPeceTermsAccept { get; set; }

        public string PhoneNo
        {
            get; set;
        }
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
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationUserDetails : IdentityUser
    {
        public virtual UserInfo UserInfo { get; set; }
    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserDetailsDbContext : IdentityDbContext<ApplicationUserDetails>
    {
        public UserDetailsDbContext() : base("DefaultConnection") { }

        public DbSet<UserInfo> UserInfo { get; set; }
    }


}