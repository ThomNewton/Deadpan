using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Deadpan.Data;
using Deadpan.Models;
using Microsoft.AspNet.Identity;

namespace Deadpan.Controllers
{
    /// <summary>
    /// Manages the primary public-facing pages of the application,
    /// such as the homepage and the "About" page.
    /// </summary>
    public class HomeController : Controller
    {
        private DeadpanDbContext db = new DeadpanDbContext();

        /// <summary>
        /// Asynchronously builds and displays the application's homepage.
        /// This action aggregates several dynamic content sections, including recent reviews,
        /// recommendations for a random director, and recommendations from a random decade.
        /// </summary>
        /// <returns>The rendered Index view populated with a HomepageViewModel.</returns>
        public async Task<ActionResult> Index()
        {
            var viewModel = new HomepageViewModel();
            var rnd = new Random();

            // --- Section: Recent Reviews ---
            // Fetches the 6 most recent reviews with an optimized query that also retrieves
            // the associated Movie, User, and the user's list of favorite movies in a single database call.
            var recentReviewsData = await db.Reviews
                .OrderByDescending(r => r.ReviewDate)
                .Take(6)
                .Select(r => new {
                    Review = r,
                    Movie = r.Movie,
                    User = r.User,
                    UserFavorites = r.User.FavoriteMovies.Select(f => f.MovieId)
                })
                .ToListAsync();

            // Maps the raw database results to a streamlined view model for the homepage feed.
            viewModel.RecentReviews = recentReviewsData.Select(data => new HomepageReviewViewModel
            {
                MovieId = data.Movie.MovieId,
                MovieTitle = data.Movie.Title,
                PosterUrls = data.Movie.PosterUrls,
                Rating = data.Review.Rating,
                UserId = data.User.Id,
                UserDisplayName = !string.IsNullOrWhiteSpace(data.User.Nickname) ? data.User.Nickname : data.User.UserName.Split('@')[0],
                UserLikedMovie = data.UserFavorites.Contains(data.Movie.MovieId)
            }).ToList();


            // --- Section: Director Recommendations ---
            // Gathers a list of all unique director names from the database.
            var allDirectors = await db.Movies.Select(m => m.Director).Distinct().ToListAsync();
            if (allDirectors.Any())
            {
                // Selects a random director from the list.
                viewModel.RecommendedDirectorName = allDirectors[rnd.Next(allDirectors.Count)];
                // Fetches up to 6 random movies by that director to display as recommendations.
                viewModel.DirectorRecommendations = await db.Movies
                    .Where(m => m.Director == viewModel.RecommendedDirectorName)
                    .OrderBy(m => Guid.NewGuid()) // Efficient way to randomize results in SQL Server.
                    .Take(6)
                    .ToListAsync();
            }

            // --- Section: Decade Recommendations ---
            // Dynamically generates recommendations from a random decade based on the movies in the database.
            if (await db.Movies.AnyAsync())
            {
                var minYear = await db.Movies.MinAsync(m => m.ReleaseYear);
                var maxYear = await db.Movies.MaxAsync(m => m.ReleaseYear);

                // Determine the start and end decades (e.g., 1964 -> 1960, 2024 -> 2020).
                int startDecade = (minYear / 10) * 10;
                int endDecade = (maxYear / 10) * 10;

                // Create a list of all possible decades present in the database.
                var availableDecades = new List<int>();
                for (int d = startDecade; d <= endDecade; d += 10)
                {
                    availableDecades.Add(d);
                }

                if (availableDecades.Any())
                {
                    // Pick a random decade from the list.
                    var chosenDecade = availableDecades[rnd.Next(availableDecades.Count)];
                    int startYear = chosenDecade;
                    int endYear = startYear + 9;

                    viewModel.RecommendedDecade = $"{startYear}s";

                    // Get up to 6 random movies from that decade.
                    viewModel.DecadeRecommendations = await db.Movies
                        .Where(m => m.ReleaseYear >= startYear && m.ReleaseYear <= endYear)
                        .OrderBy(m => Guid.NewGuid())
                        .Take(6)
                        .ToListAsync();
                }
            }

            // --- Section: Personalized Welcome Message ---
            // If the user is logged in, fetch their display name for a personalized greeting.
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = await db.Users.SingleOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    ViewBag.UserNickname = string.IsNullOrWhiteSpace(user.Nickname) ? user.UserName.Split('@')[0] : user.Nickname;
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// Displays the static "About" page of the application.
        /// </summary>
        /// <returns>The rendered About view.</returns>
        public ActionResult About()
        {
            ViewBag.Title = "About the Author";
            return View();
        }
    }
}
