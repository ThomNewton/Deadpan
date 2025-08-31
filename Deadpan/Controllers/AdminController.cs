using Deadpan.Data;
using Deadpan.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Deadpan.Controllers
{
    /// <summary>
    /// Manages administrative tasks for the application, such as user management.
    /// Access to this controller is restricted to users in the "Admin" role.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private DeadpanDbContext db = new DeadpanDbContext();

        /// <summary>
        /// Displays a list of all registered users in the application.
        /// </summary>
        /// <returns>The rendered Index view containing a list of all users.</returns>
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        /// <summary>
        /// Displays a confirmation page before deleting a user.
        /// </summary>
        /// <param name="id">The unique ID of the user to be deleted.</param>
        /// <returns>The rendered Delete confirmation view showing the user's details.</returns>
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Handles the permanent deletion of a user and all their associated data.
        /// </summary>
        /// <param name="id">The unique ID of the user to delete.</param>
        /// <returns>A redirect to the user list (Index action).</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser user = await db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                // To maintain database integrity, we must remove dependent records
                // before deleting the user itself.

                // 1. Find all reviews written by this user.
                var userReviews = db.Reviews.Where(r => r.UserId == id);

                // 2. Remove the collection of reviews from the database.
                db.Reviews.RemoveRange(userReviews);

                // 3. Now that the foreign key constraints are handled, it's safe to remove the user.
                db.Users.Remove(user);

                // 4. Commit all changes to the database in a single transaction.
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
    }
}

