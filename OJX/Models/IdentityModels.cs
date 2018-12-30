using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OJX.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		public string Hometown { get; set; }
		public List<Submission>Submissions { get; set; }
		public string PreferredLanguage { set; get; } = "C";

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
			: base("OJXUser", throwIfV1Schema: false)
		{
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}

	public static class ApplicationUserModel
	{
		public static ApplicationUser GetApplicationUserOfId(string userId)
		{
			using (var db = ApplicationDbContext.Create())
			{
				return db.Users.SingleOrDefault(user => user.Id == userId);
			}
		}

		public static void ChangePreferredLanguage(ApplicationUser user, string language)
		{
			using (var db = ApplicationDbContext.Create())
			{
				db.Users.Attach(user);
				user.PreferredLanguage = language;
				db.SaveChanges();
			}
		}
	}
}