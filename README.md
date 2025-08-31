# Deadpan

*Your personal, minimalist film diary.*

Deadpan is a fully-featured, market-ready MVP (Minimum Viable Product) for a niche film logging web application. It is designed for cinephiles who value aesthetics and a focused user experience over the feature-cluttered interfaces of mainstream platforms. Inspired by the functionality of Letterboxd and the stark, minimalist style of director Jim Jarmusch's filmography, Deadpan offers a unique and opinionated space for film lovers.

---

## Commercial Viability & Market Positioning

While currently a portfolio project, Deadpan is built on a solid foundation that can be extended for commercial use. Its strong, curated identity is its key market differentiator.

**Target Audience:**

* **Art-house Cinephiles:** Users who frequent services like The Criterion Channel or MUBI and appreciate a more curated, less mainstream experience.

* **Design-Conscious Users:** Individuals who are drawn to minimalist aesthetics, typography, and clean interfaces.

* **Users Experiencing "Platform Fatigue":** People looking for a focused, uncluttered alternative to existing social media-heavy film apps.

**Potential Monetization Strategies:**

* **Freemium "Pro" Tier:** The core logging and reviewing features remain free, while a premium subscription could unlock advanced features like detailed personal statistics, custom poster selection, an ad-free experience, or the ability to create and share custom lists.

* **Affiliate Partnerships:** Integrate "Where to Watch" links for films, partnering with streaming services (Amazon Prime, MUBI, Apple TV) or physical media retailers to earn affiliate commissions.

* **Curated Content & Sponsorships:** Partner with independent film distributors, festivals, or magazines to feature sponsored, curated film collections on the homepage.

* **API Access:** Develop and offer a paid public API for developers who wish to use Deadpan's curated database.

---

## ✨ Key Features

Deadpan is a feature-complete application supporting the entire user journey.

* **User Authentication:** Secure user registration, login, and account management powered by ASP.NET Identity.

* **Dynamic Homepage:** A curated landing page featuring the latest user reviews and dynamic carousels recommending films from a random director and a random decade.

* **Comprehensive Film Logbook:** A master list of all films in the database, complete with searching and sorting functionality.

* **TMDB API Integration:** Administrators can seamlessly search for and import detailed movie data (synopsis, cast, crew, posters) from The Movie Database with a single click.

* **Interactive Reviews & Ratings:** Users can give films a rating (from 1-5, including half-stars) and write detailed reviews.

* **"Likes" System:** A simple "like" feature allows users to mark their favorite films.

* **Personal Profile Pages:** Each user has a personal dashboard that aggregates their activity, showcasing galleries of their liked and rated films, and a complete history of their written reviews.

* **Administrator Panel:** A role-protected admin dashboard for managing the application's users and content (films).

---

## 🎨 Design Philosophy

The application's visual identity is a direct translation of its "deadpan" ethos.

* **Aesthetic:** The UI is built on a stark, high-contrast monochrome theme with a focus on negative space and readability.

* **Typography:** The 'Crimson Text' serif font is used throughout to evoke a classic, literary feel, akin to a physical logbook.

* **User Experience:** Interactions are designed to be simple and direct, avoiding unnecessary clutter and focusing the user on the content.

---

## 🗄️ Database Schema

The application's data is organized around three core models: Users, Movies, and Reviews, with many-to-many relationships for "likes" and user roles.

```
 erDiagram
    AspNetUsers {
        string Id PK "User ID"
        string Nickname
        string Email
    }
    Movies {
        int MovieId PK "Movie ID"
        string Title
        string Director
        int ReleaseYear
    }
    Reviews {
        int ReviewId PK "Review ID"
        decimal Rating
        string Comment
        string UserId FK
        int MovieId FK
    }
    ApplicationUserMovies {
        string ApplicationUser_Id FK "User ID"
        int Movie_MovieId FK "Movie ID"
    }
    AspNetUsers ||--o{ Reviews : "writes"
    Movies ||--o{ Reviews : "has"
    AspNetUsers }o--o{ Movies : "favorites (likes)"
```

---

## 🛠️ Technology Stack

* **Backend:** ASP.NET MVC 5 (.NET Framework 4.7.2) with C#

* **Database:** SQL Server (LocalDB) with Entity Framework 6 (Code First)

* **Authentication:** ASP.NET Identity

* **Frontend:** HTML5, Razor View Engine, CSS3, Bootstrap 5, jQuery

* **APIs:** The Movie Database (TMDb) for film data import

---

## 🚀 Getting Started

To get a local copy up and running, follow these steps.

**Prerequisites**

* Visual Studio 2022 (or later) with the "ASP.NET and web development" workload.

* .NET Framework 4.7.2 Developer Pack.

* SQL Server Express LocalDB (installed by default with Visual Studio).

**Installation**


1. Get a free API Key from [themoviedb.org](https://themoviedb.org/).
2. Clone the repo:
   ```sh
    git clone https://github.com/ThomNewton/Deadpan
   ```
3. Open the `Deadpan.sln` file in Visual Studio.
4. Enter your TMDB API Key in `Controllers/MoviesController.cs`:
   ```csharp
   private readonly string _tmdbApiKey = "YOUR_API_KEY_HERE";
   ```
5. Open the Package Manager Console and update the database, which will also run the seeder:
   ```sh
   Update-Database
   ```
6. Run the project (F5).

7. You can log in with the pre-seeded administrator account:
   ```
   Email: admin@deadpan.com
   Password: AdminPassword123!
   ```
---

## 👤 Author & Contact

**Mail:** [m.t.jedynak@gmail.com](mailto:m.t.jedynak@gmail.com)

**X (Twitter):** [https://x.com/maciejtjedynak](@maciejtjedynak)
