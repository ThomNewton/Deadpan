using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Deadpan.Models
{
    /// <summary>
    /// Represents a single movie in the database. This is a core domain model for the application.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the primary key for the Movie.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Gets or sets the title of the movie. This field is required.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name of the movie's director.
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Gets or sets the year the movie was released.
        /// </summary>
        [Display(Name = "Release Year")]
        public int ReleaseYear { get; set; }

        /// <summary>
        /// Gets or sets the full, detailed synopsis for the movie.
        /// Intended for the movie's details page.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Synopsis { get; set; }

        /// <summary>
        /// Gets or sets a brief, one-sentence synopsis.
        /// Intended for display in lists or tables.
        /// </summary>
        [Display(Name = "Short Synopsis")]
        [DataType(DataType.MultilineText)]
        public string ShortSynopsis { get; set; }

        /// <summary>
        /// Gets or sets the name(s) of the screenwriter(s).
        /// </summary>
        [Display(Name = "Written By")]
        public string WrittenBy { get; set; }

        /// <summary>
        /// Gets or sets the name(s) of the composer(s).
        /// </summary>
        [Display(Name = "Music By")]
        public string MusicBy { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of the main actors.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Starring { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated string of poster image URLs.
        /// </summary>
        [Display(Name = "Poster URLs (comma-separated)")]
        public string PosterUrls { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews associated with this movie.
        /// This is a navigation property for the one-to-many relationship between Movie and Review.
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// Gets or sets the collection of users who have favorited this movie.
        /// This is a navigation property for the many-to-many relationship between Movie and ApplicationUser.
        /// </summary>
        public virtual ICollection<ApplicationUser> FavoritedByUsers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Movie"/> class.
        /// The constructor initializes the navigation property collections to prevent null reference exceptions.
        /// </summary>
        public Movie()
        {
            this.Reviews = new HashSet<Review>();
            this.FavoritedByUsers = new HashSet<ApplicationUser>();
        }
    }
}
