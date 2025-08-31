using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Deadpan.Data;
using Deadpan.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Deadpan.Controllers
{
    /// <summary>
    /// Manages all movie-related operations, including displaying, creating, editing,
    /// deleting, and fetching movie data from the TMDB API.
    /// </summary>
    public class MoviesController : Controller
    {
        private DeadpanDbContext db = new DeadpanDbContext();
        private static readonly HttpClient client = new HttpClient();

        // IMPORTANT: Your private TMDB API key.
        private readonly string _tmdbApiKey = "YOUR_API_KEY_HERE";

        #region TMDB Methods
        /// <summary>
        /// Displays the initial view for fetching movie data from TMDB.
        /// Access is restricted to users in the "Admin" role.
        /// </summary>
        /// <returns>The rendered Fetch view with an empty list of search results.</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Fetch()
        {
            return View(new List<TmdbMovieSearchResult>());
        }

        /// <summary>
        /// Handles the submission of a movie title search against the TMDB API.
        /// </summary>
        /// <param name="movieTitle">The title of the movie to search for.</param>
        /// <returns>The rendered Fetch view displaying the search results from TMDB.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Fetch(string movieTitle)
        {
            if (string.IsNullOrWhiteSpace(movieTitle))
            {
                return View(new List<TmdbMovieSearchResult>());
            }

            var searchUrl = $"https://api.themoviedb.org/3/search/movie?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(movieTitle)}";
            var response = await client.GetStringAsync(searchUrl);
            var searchResult = JsonConvert.DeserializeObject<TmdbMovieSearchResponse>(response);

            return View(searchResult.Results);
        }

        /// <summary>
        /// Fetches detailed information for a specific movie from the TMDB API using its ID,
        /// then populates and displays the movie creation form with this data.
        /// </summary>
        /// <param name="tmdbId">The unique ID of the movie on TMDB.</param>
        /// <returns>The rendered Create view, pre-filled with the fetched movie details.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetMovieDetails(int tmdbId)
        {
            var detailsUrl = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={_tmdbApiKey}&append_to_response=credits,images";
            var detailsResponse = await client.GetStringAsync(detailsUrl);
            var movieDetails = JsonConvert.DeserializeObject<TmdbMovieDetails>(detailsResponse);

            var director = movieDetails.Credits.Crew.FirstOrDefault(c => c.Job == "Director")?.Name;
            var writer = movieDetails.Credits.Crew.FirstOrDefault(c => c.Job == "Screenplay" || c.Job == "Writer")?.Name;
            var music = movieDetails.Credits.Crew.FirstOrDefault(c => c.Job == "Original Music Composer")?.Name;
            var starring = string.Join(", ", movieDetails.Credits.Cast.Take(10).Select(c => c.Name));
            int.TryParse(movieDetails.ReleaseDate?.Substring(0, 4), out int releaseYear);
            var posters = movieDetails.Images.Posters.Select(p => $"https://image.tmdb.org/t/p/original{p.FilePath}").ToList();


            var movie = new Movie
            {
                Title = movieDetails.Title,
                Director = director,
                ReleaseYear = releaseYear,
                ShortSynopsis = movieDetails.Overview,
                Synopsis = movieDetails.Overview,
                WrittenBy = writer,
                MusicBy = music,
                Starring = starring,
                PosterUrls = string.Join(",", posters)
            };

            return View("Create", movie);
        }
        #endregion

        #region Standard CRUD Methods
        /// <summary>
        /// Displays a paginated, sortable, and searchable list of all movies in the database.
        /// </summary>
        /// <param name="sortOrder">The current sort order for the movie list.</param>
        /// <param name="currentFilter">The current search filter being applied.</param>
        /// <param name="searchString">The new search string submitted by the user.</param>
        /// <returns>The rendered Index view displaying the filtered and sorted list of movies.</returns>
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DirectorSortParm = sortOrder == "Director" ? "director_desc" : "Director";
            ViewBag.YearSortParm = sortOrder == "Year" ? "year_desc" : "Year";

            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var movies = from m in db.Movies
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString)
                                       || s.Director.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    movies = movies.OrderByDescending(m => m.Title);
                    break;
                case "Director":
                    movies = movies.OrderBy(m => m.Director);
                    break;
                case "director_desc":
                    movies = movies.OrderByDescending(m => m.Director);
                    break;
                case "Year":
                    movies = movies.OrderBy(m => m.ReleaseYear);
                    break;
                case "year_desc":
                    movies = movies.OrderByDescending(m => m.ReleaseYear);
                    break;
                default:
                    movies = movies.OrderBy(m => m.Title);
                    break;
            }

            return View(await movies.ToListAsync());
        }

        /// <summary>
        /// Displays the details for a single movie, including its reviews.
        /// </summary>
        /// <param name="id">The ID of the movie to display.</param>
        /// <returns>The rendered Details view for the specified movie.</returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = await db.Movies
                                  .Include(m => m.Reviews.Select(r => r.User))
                                  .Include(m => m.FavoritedByUsers)
                                  .SingleOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserRating = 0m;
            bool isFavorited = false;

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                isFavorited = movie.FavoritedByUsers.Any(u => u.Id == userId);

                var userReview = movie.Reviews.FirstOrDefault(r => r.UserId == userId);
                if (userReview != null)
                {
                    ViewBag.UserRating = userReview.Rating;
                }
            }
            ViewBag.IsFavorited = isFavorited;

            return View(movie);
        }

        /// <summary>
        /// Displays the form for creating a new movie.
        /// Access is restricted to users in the "Admin" role.
        /// </summary>
        /// <returns>The rendered Create view.</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the submission of the new movie form.
        /// </summary>
        /// <param name="movie">The movie object to be created, bound from the form data.</param>
        /// <returns>A redirect to the movie Index page on success, or redisplays the form on failure.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "MovieId,Title,Director,ReleaseYear,ShortSynopsis,Synopsis,WrittenBy,MusicBy,Starring,PosterUrls")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        /// <summary>
        /// Displays the form for editing an existing movie.
        /// Access is restricted to users in the "Admin" role.
        /// </summary>
        /// <param name="id">The ID of the movie to edit.</param>
        /// <returns>The rendered Edit view populated with the movie's data.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// Handles the submission of the movie edit form.
        /// </summary>
        /// <param name="movie">The movie object with updated data, bound from the form.</param>
        /// <returns>A redirect to the movie Index page on success, or redisplays the form on failure.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "MovieId,Title,Director,ReleaseYear,ShortSynopsis,Synopsis,WrittenBy,MusicBy,Starring,PosterUrls")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        /// <summary>
        /// Displays a confirmation page before deleting a movie.
        /// Access is restricted to users in the "Admin" role.
        /// </summary>
        /// <param name="id">The ID of the movie to be deleted.</param>
        /// <returns>The rendered Delete confirmation view.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        /// <summary>
        /// Handles the permanent deletion of a movie and all its associated data (reviews, favorites).
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>A redirect to the movie Index page.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Movie movie = await db.Movies
                .Include(m => m.Reviews)
                .Include(m => m.FavoritedByUsers)
                .SingleOrDefaultAsync(m => m.MovieId == id);

            if (movie != null)
            {
                // Manually remove dependent records before deleting the movie
                // to ensure database integrity.
                db.Reviews.RemoveRange(movie.Reviews);
                movie.FavoritedByUsers.Clear();
                db.Movies.Remove(movie);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Disposes of the database context when the controller is disposed.
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from a finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
