using Deadpan.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Deadpan.Data
{
    /// <summary>
    /// The main Entity Framework database context for the application.
    /// It serves as the bridge between the C# domain models and the database.
    /// This context manages both the custom application data (Movies, Reviews) and the
    /// ASP.NET Identity tables (Users, Roles) by inheriting from IdentityDbContext.
    /// </summary>
    public class DeadpanDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the DeadpanDbContext, configured to use the
        /// "DefaultConnection" connection string from the Web.config file.
        /// </summary>
        public DeadpanDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for Movies. This property represents the collection
        /// of all movies in the database and maps to the "Movies" table.
        /// </summary>
        public DbSet<Movie> Movies { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Reviews. This property represents the collection
        /// of all reviews in the database and maps to the "Reviews" table.
        /// </summary>
        public DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// A static factory method used by the OWIN middleware to create a new instance
        /// of the database context per request.
        /// </summary>
        /// <returns>A new instance of DeadpanDbContext.</returns>
        public static DeadpanDbContext Create()
        {
            return new DeadpanDbContext();
        }

        /// <summary>
        /// Overrides the default model creation behavior to apply custom configurations
        /// using the Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // This line removes the default convention for cascade deletes on one-to-many relationships.
            // It helps prevent accidental deletion of parent records from causing unintended deletions of child records.
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
