using System.Collections.Generic;
using Deadpan.Models;

namespace Deadpan.Models
{
    /// <summary>
    /// Represents the composite view model for the application's homepage.
    /// This class acts as a container for all the dynamic data required to render the main landing page,
    /// such as recent reviews and various movie recommendations.
    /// </summary>
    public class HomepageViewModel
    {
        /// <summary>
        /// Gets or sets a list of recently posted reviews, formatted specifically for the homepage feed.
        /// </summary>
        public List<HomepageReviewViewModel> RecentReviews { get; set; }

        /// <summary>
        /// Gets or sets a list of recommended movies from a randomly selected director.
        /// </summary>
        public List<Movie> DirectorRecommendations { get; set; }

        /// <summary>
        /// Gets or sets the name of the randomly selected director for the recommendation section.
        /// </summary>
        public string RecommendedDirectorName { get; set; }

        /// <summary>
        /// Gets or sets a list of recommended movies from a randomly selected decade.
        /// </summary>
        public List<Movie> DecadeRecommendations { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the randomly selected decade (e.g., "1990s").
        /// </summary>
        public string RecommendedDecade { get; set; }
    }

    /// <summary>
    /// Represents a streamlined, flattened view model for a single review displayed on the homepage feed.
    /// This class contains only the essential information needed for display, helping to optimize data transfer.
    /// </summary>
    public class HomepageReviewViewModel
    {
        /// <summary>
        /// Gets or sets the ID of the movie that was reviewed.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Gets or sets the title of the movie that was reviewed.
        /// </summary>
        public string MovieTitle { get; set; }

        /// <summary>
        /// Gets or sets the comma-separated list of poster URLs for the reviewed movie.
        /// </summary>
        public string PosterUrls { get; set; }

        /// <summary>
        /// Gets or sets the rating given in the review.
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who wrote the review.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the user (either their nickname or formatted username).
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user who wrote the review has also favorited the movie.
        /// </summary>
        public bool UserLikedMovie { get; set; }
    }
}
