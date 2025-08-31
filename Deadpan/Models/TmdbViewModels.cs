using Newtonsoft.Json;
using System.Collections.Generic;

namespace Deadpan.Models
{
    /// <summary>
    /// Represents a single, summarized movie result from a TMDB API search query.
    /// This class is used to display the initial list of search results.
    /// </summary>
    public class TmdbMovieSearchResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the movie in the TMDB database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the movie.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the release date of the movie as a string (e.g., "1995-10-27").
        /// Mapped from the "release_date" JSON property.
        /// </summary>
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the partial path to the movie's poster image.
        /// Mapped from the "poster_path" JSON property.
        /// </summary>
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }

    /// <summary>
    /// Represents the top-level response from a TMDB movie search API call.
    /// </summary>
    public class TmdbMovieSearchResponse
    {
        /// <summary>
        /// Gets or sets the list of movie search results.
        /// </summary>
        public List<TmdbMovieSearchResult> Results { get; set; }
    }

    /// <summary>
    /// Represents the detailed information for a single movie retrieved from the TMDB API.
    /// </summary>
    public class TmdbMovieDetails
    {
        /// <summary>
        /// Gets or sets the title of the movie.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the plot overview or synopsis of the movie.
        /// </summary>
        public string Overview { get; set; }

        /// <summary>
        /// Gets or sets the release date of the movie as a string.
        /// Mapped from the "release_date" JSON property.
        /// </summary>
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the partial path to the movie's primary poster image.
        /// Mapped from the "poster_path" JSON property.
        /// </summary>
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        /// <summary>
        /// Gets or sets the credit information (cast and crew) for the movie.
        /// This data is included when "credits" is appended to the API request.
        /// </summary>
        public TmdbMovieCredits Credits { get; set; }

        /// <summary>
        /// Gets or sets the collection of images (posters, backdrops) for the movie.
        /// This data is included when "images" is appended to the API request.
        /// </summary>
        public TmdbMovieImages Images { get; set; }
    }

    /// <summary>
    /// Represents a single poster image object from the TMDB API.
    /// </summary>
    public class TmdbPoster
    {
        /// <summary>
        /// Gets or sets the partial file path for the poster image.
        /// Mapped from the "file_path" JSON property.
        /// </summary>
        [JsonProperty("file_path")]
        public string FilePath { get; set; }
    }

    /// <summary>
    /// Represents the container for all image collections associated with a movie.
    /// </summary>
    public class TmdbMovieImages
    {
        /// <summary>
        /// Gets or sets a list of poster images for the movie.
        /// </summary>
        public List<TmdbPoster> Posters { get; set; }
    }

    /// <summary>
    /// Represents a single cast member (actor) from the movie's credits.
    /// </summary>
    public class TmdbCast
    {
        /// <summary>
        /// Gets or sets the name of the actor.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents a single crew member (e.g., director, writer) from the movie's credits.
    /// </summary>
    public class TmdbCrew
    {
        /// <summary>
        /// Gets or sets the job title of the crew member (e.g., "Director").
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// Gets or sets the name of the crew member.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents the top-level credits object, containing lists of cast and crew.
    /// </summary>
    public class TmdbMovieCredits
    {
        /// <summary>
        /// Gets or sets the list of cast members.
        /// </summary>
        public List<TmdbCast> Cast { get; set; }

        /// <summary>
        /// Gets or sets the list of crew members.
        /// </summary>
        public List<TmdbCrew> Crew { get; set; }
    }
}
