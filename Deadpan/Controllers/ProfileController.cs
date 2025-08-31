using Deadpan.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Deadpan.Controllers
{
    /// <summary>
    /// Manages the display of user profiles and handles user-specific actions like favoriting movies.
    /// All actions in this controller require the user to be authenticated.
    /// </summary>
    [Authorize]
    public class ProfileController : Controller
    {
        private DeadpanDbContext db = new DeadpanDbContext();

        /// <summary>
        /// Asynchronously retrieves and displays the profile page for the currently logged-in user.
        /// This includes their reviews and a list of their favorite movies.
        /// </summary>
        /// <returns>The rendered Index view populated with the current user's data.</returns>
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            // Eagerly load the user's reviews (and the movie associated with each review)
            // and their list of favorite movies in a single query.
            var currentUser = await db.Users
                                      .Include(u => u.Reviews.Select(r => r.Movie))
                                      .Include(u => u.FavoriteMovies)
                                      .SingleOrDefaultAsync(u => u.Id == userId);

            if (currentUser == null)
            {
                return HttpNotFound();
            }

            return View(currentUser);
        }

        /// <summary>
        /// Toggles a movie's status as a favorite for the current user.
        /// If the movie is already a favorite, it is removed; otherwise, it is added.
        /// </summary>
        /// <param name="movieId">The ID of the movie to add or remove from favorites.</param>
        /// <returns>A redirect to the movie's Details page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleFavorite(int movieId)
        {
            var userId = User.Identity.GetUserId();
            var currentUser = await db.Users.Include(u => u.FavoriteMovies)
                                            .SingleOrDefaultAsync(u => u.Id == userId);

            var movie = await db.Movies.FindAsync(movieId);

            if (currentUser == null || movie == null)
            {
                return HttpNotFound();
            }

            bool isCurrentlyFavorite = currentUser.FavoriteMovies.Any(m => m.MovieId == movieId);

            if (isCurrentlyFavorite)
            {
                // If it's already a favorite, find and remove it.
                var movieToRemove = currentUser.FavoriteMovies.SingleOrDefault(m => m.MovieId == movieId);
                if (movieToRemove != null)
                {
                    currentUser.FavoriteMovies.Remove(movieToRemove);
                }
            }
            else
            {
                // Otherwise, add it to the favorites collection.
                currentUser.FavoriteMovies.Add(movie);
            }

            await db.SaveChangesAsync();

            // Redirect back to the movie details page to show the updated status.
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        /// <summary>
        /// A child action that provides the necessary data for the shared _LoginPartial view.
        /// It fetches the user's preferred display name (Nickname or UserName).
        /// This action is intended to be called only from within another view (e.g., the main layout).
        /// </summary>
        /// <returns>A partial view result for the _LoginPartial view.</returns>
        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult _LoginPartial()
        {
            string userDisplay = "";
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                if (user != null)
                {
                    // Use the user's Nickname if it exists, otherwise fall back to their username.
                    userDisplay = string.IsNullOrWhiteSpace(user.Nickname) ? user.UserName.Split('@')[0] : user.Nickname;
                }
            }
            ViewBag.UserDisplay = userDisplay;
            return PartialView("~/Views/Shared/_LoginPartial.cshtml");
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
    }
}
