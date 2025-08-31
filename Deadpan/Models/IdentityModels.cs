using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Deadpan.Models
{
    /// <summary>
    /// Represents a user in the application, extending the default ASP.NET IdentityUser.
    /// This class includes custom properties and relationships specific to the Deadpan application,
    /// such as a user's nickname and their collections of reviews and favorite movies.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the user's public-facing display name or nickname.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews written by this user.
        /// This is a navigation property that defines a one-to-many relationship.
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// Gets or sets the collection of movies that this user has marked as a favorite.
        /// This is a navigation property that defines a many-to-many relationship.
        /// </summary>
        public virtual ICollection<Movie> FavoriteMovies { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUser"/> class.
        /// The constructor initializes the navigation property collections to prevent null reference exceptions.
        /// </summary>
        public ApplicationUser()
        {
            this.Reviews = new HashSet<Review>();
            this.FavoriteMovies = new HashSet<Movie>();
        }

        /// <summary>
        /// Generates a ClaimsIdentity for the user, which is used by the authentication system.
        /// </summary>
        /// <param name="manager">The UserManager instance used to create the identity.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user's ClaimsIdentity.</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here if needed
            return userIdentity;
        }
    }
}
