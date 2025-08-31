using System;
using System.ComponentModel.DataAnnotations;

namespace Deadpan.Models
{
    /// <summary>
    /// Represents a single user review for a specific movie.
    /// This class links an ApplicationUser and a Movie together.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Gets or sets the primary key for the Review.
        /// </summary>
        public int ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the user's numerical rating for the movie.
        /// The value must be between 0.0 and 5.0. It can include half-star ratings (e.g., 3.5).
        /// </summary>
        [Required]
        [Range(0.0, 5.0, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal Rating { get; set; }

        /// <summary>
        /// Gets or sets the user's written commentary or thoughts on the movie.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the review was created or last updated.
        /// </summary>
        [Display(Name = "Review Date")]
        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }

        // --- Foreign Key Relationships ---

        /// <summary>
        /// Gets or sets the foreign key for the Movie this review belongs to.
        /// </summary>
        public int MovieId { get; set; }
        /// <summary>
        /// Gets or sets the navigation property to the associated Movie.
        /// </summary>
        public virtual Movie Movie { get; set; }

        /// <summary>
        /// Gets or sets the foreign key for the ApplicationUser who wrote this review.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the navigation property to the associated ApplicationUser.
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}
