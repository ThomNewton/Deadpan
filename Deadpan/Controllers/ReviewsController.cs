using Deadpan.Data;
using Deadpan.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Deadpan.Controllers
{
    /// <summary>
    /// Manages the creation and deletion of user reviews for movies.
    /// All actions in this controller require the user to be authenticated.
    /// </summary>
    [Authorize]
    public class ReviewsController : Controller
    {
        private DeadpanDbContext db = new DeadpanDbContext();

        /// <summary>
        /// Creates a new review or updates the comment of an existing review for a specific movie.
        /// If a review for the movie by the current user already exists, its comment is updated.
        /// Otherwise, a new review is created with the provided comment and a default rating of 0.
        /// </summary>
        /// <param name="comment">The text content of the review/comment.</param>
        /// <param name="movieId">The ID of the movie being reviewed.</param>
        /// <returns>A redirect to the movie's Details page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string comment, int movieId)
        {
            var userId = User.Identity.GetUserId();
            var existingReview = await db.Reviews.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (existingReview != null)
            {
                // If a review exists, just update the comment.
                existingReview.Comment = comment;
                db.Entry(existingReview).State = EntityState.Modified;
            }
            else
            {
                // If no review exists, create a new one with the comment and a default rating of 0.
                var newReview = new Review
                {
                    MovieId = movieId,
                    UserId = userId,
                    Rating = 0, // Default rating
                    Comment = comment,
                    ReviewDate = DateTime.UtcNow
                };
                db.Reviews.Add(newReview);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        /// <summary>
        /// Creates a new review or updates the rating of an existing one.
        /// This action is typically called via AJAX from the interactive star rating UI.
        /// If a review for the movie by the current user exists, its rating is updated.
        /// Otherwise, a new review is created with the provided rating.
        /// </summary>
        /// <param name="movieId">The ID of the movie being rated.</param>
        /// <param name="rating">The rating value (e.g., 0.5 to 5.0).</param>
        /// <returns>A redirect to the movie's Details page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RateMovie(int movieId, decimal rating)
        {
            var userId = User.Identity.GetUserId();
            var existingReview = await db.Reviews.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (existingReview != null)
            {
                // If a review exists, update its rating.
                existingReview.Rating = rating;
                db.Entry(existingReview).State = EntityState.Modified;
            }
            else
            {
                // If no review exists, create a new one with just the rating.
                var newReview = new Review
                {
                    MovieId = movieId,
                    UserId = userId,
                    Rating = rating,
                    Comment = "",
                    ReviewDate = DateTime.UtcNow
                };
                db.Reviews.Add(newReview);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        /// <summary>
        /// Displays a confirmation page before deleting a review.
        /// Ensures that only the user who wrote the review or an admin can access this page.
        /// </summary>
        /// <param name="id">The ID of the review to be deleted.</param>
        /// <returns>The rendered Delete confirmation view.</returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = await db.Reviews.FindAsync(id);
            if (review == null)
            {
                return HttpNotFound();
            }

            // Security check: Ensure the current user is either the author of the review or an Admin.
            var currentUserId = User.Identity.GetUserId();
            if (review.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                // If not authorized, return a Forbidden status.
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(review);
        }

        /// <summary>
        /// Handles the permanent deletion of a review.
        /// Ensures that only the user who wrote the review or an admin can perform this action.
        /// </summary>
        /// <param name="id">The ID of the review to delete.</param>
        /// <returns>A redirect to the associated movie's Details page.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Review review = await db.Reviews.FindAsync(id);

            // Security check: Re-validate that the current user is authorized to delete this review.
            var currentUserId = User.Identity.GetUserId();
            if (review.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            int movieId = review.MovieId; // Store movieId before deleting the review.
            db.Reviews.Remove(review);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Movies", new { id = movieId });
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
