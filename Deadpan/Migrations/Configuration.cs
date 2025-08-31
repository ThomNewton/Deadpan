namespace Deadpan.Migrations
{
    using Deadpan.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Configuration class for Entity Framework Code First Migrations.
    /// This class controls how database migrations are handled and allows for
    /// seeding the database with initial data.
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<Deadpan.Data.DeadpanDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the Configuration class.
        /// </summary>
        public Configuration()
        {
            // Automatic migrations are disabled. This is a best practice that requires
            // developers to explicitly create a migration script (using Add-Migration)
            // for every change to the data model, providing more control over schema changes.
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// This method is called after migrating to the latest version.
        /// It is used to populate the database with initial or default data.
        /// </summary>
        /// <param name="context">The database context to be seeded.</param>
        protected override void Seed(Deadpan.Data.DeadpanDbContext context)
        {
            // This method will be called every time the 'Update-Database' command is run.
            // The AddOrUpdate method prevents creating duplicate data on subsequent seeds.

            #region Seed Admin User and Role
            // Create instances of RoleManager and UserManager to interact with ASP.NET Identity.
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Check if the "Admin" role exists, and create it if it doesn't.
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }

            // Check if the default admin user exists by their username/email.
            var user = userManager.FindByName("admin@deadpan.com");
            if (user == null)
            {
                // If the user doesn't exist, create a new ApplicationUser object.
                user = new ApplicationUser
                {
                    UserName = "admin@deadpan.com",
                    Email = "admin@deadpan.com",
                    Nickname = "Admin"
                };
                // Create the user with a default password.
                var result = userManager.Create(user, "AdminPassword123!");
                // If the user was created successfully, assign them to the "Admin" role.
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
            #endregion

            #region Seed Movies
            // Use AddOrUpdate to seed the database with an initial list of movies.
            // The first parameter (m => m.Title) specifies that the 'Title' property
            // should be used to check for the existence of a movie before adding or updating it.
            context.Movies.AddOrUpdate(m => m.Title,
                // Jim Jarmusch
                new Movie
                {
                    Title = "Stranger Than Paradise",
                    Director = "Jim Jarmusch",
                    ReleaseYear = 1984,
                    ShortSynopsis = "A self-styled New York hipster is paid a surprise visit by his younger cousin from Cleveland, and the two, joined by a friend, proceed to have a series of deadpan adventures.",
                    Synopsis = "Willie, a surly downtown New Yorker, has his life of listless hustling disrupted by the arrival of his 16-year-old Hungarian cousin, Eva. Initially dismissive, Willie's cynical exterior begins to crack as he and his best friend, Eddie, are drawn into Eva's quiet, observant world. Their journey takes them from the grimy streets of the Lower East Side to the frozen, desolate landscape of Cleveland, and finally to a comically bleak vision of Florida's 'paradise'.\n\nStructured in three distinct acts and shot in stark, minimalist black-and-white, the film is a deadpan masterpiece of observational humor. It captures the strange, funny, and often aimless moments that define the lives of outsiders adrift in a peculiar vision of America, all set to the unforgettable sounds of Screamin' Jay Hawkins.",
                    WrittenBy = "Jim Jarmusch",
                    MusicBy = "John Lurie",
                    Starring = "John Lurie, Eszter Balint, Richard Edson",
                    PosterUrls = "https://image.tmdb.org/t/p/original/Q9wdlNjBOti4JhAjAQXphzShw3.jpg,https://image.tmdb.org/t/p/original/xo3gzcdeyC6YO3dpW1K875my48p.jpg,https://image.tmdb.org/t/p/original/tpXICbUS509usYHKRBF3V8Rx454.jpg,https://image.tmdb.org/t/p/original/6GCzwkZ4BrEuiXJbvPHY8qPGNpz.jpg"
                },
                new Movie
                {
                    Title = "Mystery Train",
                    Director = "Jim Jarmusch",
                    ReleaseYear = 1989,
                    ShortSynopsis = "A trio of stories unfolds over a single night in a Memphis steeped in the mythic ghosts of rock 'n' roll.",
                    Synopsis = "A trio of stories unfolds over a single night in a Memphis steeped in the mythic ghosts of rock 'n' roll. In a rundown hotel, presided over by a wry night clerk and his bellboy, the paths of three sets of outsiders briefly intersect, linked by the city's musical legacy, a shared radio broadcast, and the sound of a mysterious gunshot.\n\nA young Japanese couple, obsessed with Carl Perkins and Elvis, makes a pilgrimage to the hallowed grounds of Sun Studio. An Italian widow, stranded in the city while escorting her husband's coffin home, finds herself sharing a room with a talkative woman and receiving a late-night visit from the King himself. And a down-on-his-luck Englishman, his cynical brother-in-law, and their friend find their drunken night spiraling into comic desperation.\n\nFilmed with a languid, observant cool and punctuated by deadpan humor, this cinematic triptych is a love letter to the soulful, dilapidated corners of American culture and the foreigners who cherish its legends more than the locals.",
                    WrittenBy = "Jim Jarmusch",
                    Starring = "Masatoshi Nagase, Youki Kudoh, Screamin' Jay Hawkins, Cinqué Lee, Nicoletta Braschi, Joe Strummer, Steve Buscemi",
                    PosterUrls = "https://image.tmdb.org/t/p/original/f11xq7dBGhz9UDc3dabldAGeXVH.jpg,https://image.tmdb.org/t/p/original/1s2Ofz0fT1Nng7Sv8V7fuu72d3k.jpg,https://image.tmdb.org/t/p/original/jDS7p86ekb91YqVvutxyjnP7lT9.jpg,https://image.tmdb.org/t/p/original/jI5cUViehlEvlrsvN7ZuYTRRxSQ.jpg"
                },
                new Movie
                {
                    Title = "Dead Man",
                    Director = "Jim Jarmusch",
                    ReleaseYear = 1995,
                    ShortSynopsis = "On the run after murdering a man, accountant William Blake encounters a strange Native American man named Nobody who prepares him for his journey into the spiritual world.",
                    Synopsis = "A lone traveler, a figure of fragile civility utterly out of place, journeys by rail to the furthest edge of the American West seeking a new beginning. He arrives not in a land of opportunity, but in a grim, industrial town that feels like a terminal station at the end of the world. Here, a single night of shocking violence severs him from the life he knew, leaving him wounded and adrift in a landscape that offers no shelter—a desolate canvas for the spirits and outcasts who haunt its fringes.\n\nStripped of his past and identity, he is found by another wanderer, an outcast himself, who sees in the wounded man not a victim, but the living embodiment of a myth. The traveler's name, a mere coincidence, is treated as a prophecy. With this strange new purpose bestowed upon him, his flight from bounty hunters transforms into a different kind of journey altogether: no longer a desperate escape for survival, but a slow, ceremonial passage toward an unknown horizon.\n\nShot entirely in stark, mesmerizing black-and-white and driven by Neil Young’s iconic, largely improvised electric guitar score, this passage into a deeper frontier is a dreamlike and darkly humorous exploration of a man slowly becoming the violent, poetic ghost of his own mistaken identity.",
                    WrittenBy = "Jim Jarmusch",
                    MusicBy = "Neil Young",
                    Starring = "Johnny Depp, Gary Farmer, Crispin Glover, Robert Mitchum, John Hurt, Lance Henriksen, Iggy Pop, Billy Bob Thornton, Michael Wincott",
                    PosterUrls = "https://image.tmdb.org/t/p/original/3LZw6p3ldpxGlxaBskoRVfv6kIB.jpg,https://image.tmdb.org/t/p/original/65iVwc0t5eTDhQJsddsR14uBuGr.jpg,https://image.tmdb.org/t/p/original/jX3wGBVoYoAY3IixBpwYk1fjT4z.jpg,https://image.tmdb.org/t/p/original/kppoftppINL5O96hDUhUR8QlJQh.jpg"
                },
                new Movie
                {
                    Title = "Coffee and Cigarettes",
                    Director = "Jim Jarmusch",
                    ReleaseYear = 2003,
                    ShortSynopsis = "A series of eleven black-and-white vignettes featuring an eclectic cast of actors and musicians sharing coffee and cigarettes.",
                    Synopsis = "A collection of eleven short, black-and-white vignettes, each featuring a different cast of characters sitting around a table drinking coffee and smoking cigarettes. The conversations are a mix of the mundane, the absurd, and the philosophical, touching on topics from caffeine-laced ice pops to the genius of Nikola Tesla.\n\nFilmed over a period of 17 years, each segment captures a unique, deadpan interaction between an eclectic mix of actors, musicians, and comedians, often playing versions of themselves. The film is a celebration of the small, awkward, and often amusing moments that happen during the simple ritual of a shared break.",
                    WrittenBy = "Jim Jarmusch",
                    MusicBy = "Various Artists",
                    Starring = "Roberto Benigni, Steven Wright, Iggy Pop, Tom Waits, Cate Blanchett, Bill Murray, Alfred Molina, Steve Coogan, GZA, RZA, Jack White, Meg White",
                    PosterUrls = "https://image.tmdb.org/t/p/original/pfG02QCsutx3PIxFS8UY4iM9AsS.jpg,https://image.tmdb.org/t/p/original/r9MSKYQkxf24d75ew4lNCfHjXSE.jpg,https://image.tmdb.org/t/p/original/tCTt9ZK2CIFrnJOMaU65w3CDyMm.jpg,https://image.tmdb.org/t/p/original/t1PmNh692frYVch9m8X4p9eCOQB.jpg"
                },
                new Movie
                {
                    Title = "Broken Flowers",
                    Director = "Jim Jarmusch",
                    ReleaseYear = 2005,
                    ShortSynopsis = "As the devoutly single Don Johnston is dumped by his latest girlfriend, he receives an anonymous pink letter informing him that he has a son who may be looking for him.",
                    Synopsis = "A resolute bachelor, Don Johnston, has just been left by his latest lover. His quiet, regimented life is thrown into turmoil by the arrival of an anonymous pink letter, informing him he has a 19-year-old son who may be setting out to find him. Pressured by his neighbor, an amateur detective, Don embarks on a cross-country road trip to track down four of his former flames who could be the mother.\n\nHis series of surprise visits to these distinctive women forces him to confront his past and the different lives he might have lived. The journey is a deadpan, melancholic, and subtly comic odyssey through the landscapes of America and his own history of detached relationships, leading to an ambiguous and poignant conclusion.",
                    WrittenBy = "Jim Jarmusch",
                    MusicBy = "Mulatu Astatke",
                    Starring = "Bill Murray, Jeffrey Wright, Sharon Stone, Frances Conroy, Jessica Lange, Tilda Swinton, Julie Delpy",
                    PosterUrls = "https://image.tmdb.org/t/p/original/dvSfsBXblVr1MzrwV0WHSjp2gJ4.jpg,https://image.tmdb.org/t/p/original/poHXcjXxSeNHOL1Eutq1BF9SMLP.jpg,https://image.tmdb.org/t/p/original/cjW9jnjzneosL0tcJcomac4LwP6.jpg,https://image.tmdb.org/t/p/original/78M9w3e7Vdz4g7QGsfvRos1tWzU.jpg"
                },
                // Wes Anderson
                new Movie
                {
                    Title = "Fantastic Mr. Fox",
                    Director = "Wes Anderson",
                    ReleaseYear = 2009,
                    ShortSynopsis = "An urbane fox cannot resist returning to his farm-raiding ways and then must help his community survive the farmers' retaliation.",
                    Synopsis = "Mr. Fox is a clever, well-dressed fox who has settled down into a quiet life as a newspaper columnist with his wife and son. However, the pull of his old, wild ways is too strong, and he plans one last, spectacular raid on the three meanest farmers in the valley: Boggis, Bunce, and Bean. The heist is a success, but the farmers retaliate with overwhelming force, trapping Mr. Fox and his entire community of underground animals.\n\nForced to use his natural cunning to survive, Mr. Fox must rally his friends and family to outwit the farmers in a battle of wits and wills. Rendered in meticulous stop-motion animation, the film is a charming, witty, and visually inventive adaptation of Roald Dahl's classic story, filled with Anderson's signature symmetrical style and dysfunctional family dynamics.",
                    WrittenBy = "Wes Anderson, Noah Baumbach",
                    MusicBy = "Alexandre Desplat",
                    Starring = "George Clooney, Meryl Streep, Jason Schwartzman, Bill Murray, Willem Dafoe, Owen Wilson",
                    PosterUrls = "https://image.tmdb.org/t/p/original/pYbIT04CMXAbVEPj9mhFzcM73XS.jpg,https://image.tmdb.org/t/p/original/jAFvXFcup7pQOyofJlxPr6rcFaa.jpg,https://image.tmdb.org/t/p/original/hhhJN8aJdTlzGmARCbwWflHXhwI.jpg,https://image.tmdb.org/t/p/original/njbTizADSZg4PqeyJdDzZGooikv.jpg"
                },
                new Movie
                {
                    Title = "The Royal Tenenbaums",
                    Director = "Wes Anderson",
                    ReleaseYear = 2001,
                    ShortSynopsis = "The eccentric members of a dysfunctional family of former child prodigies reluctantly gather under the same roof for various reasons.",
                    Synopsis = "Royal Tenenbaum and his wife, Etheline, had three children—Chas, Margot, and Richie—and then they separated. All three children were extraordinary geniuses. Virtually all memory of the brilliance of the young Tenenbaums was subsequently erased by two decades of betrayal, failure, and disaster. Most of this was generally considered to be their father's fault.\n\nWhen Royal is kicked out of the hotel where he's been living, he feigns a terminal illness to win back the affections of his estranged family. The ruse brings the entire dysfunctional clan back together in their childhood home, forcing them to confront their shared history of disappointment and faded glory in a series of poignant and darkly hilarious encounters. The film is a storybook-like exploration of melancholy, forgiveness, and the peculiar bonds of family.",
                    WrittenBy = "Wes Anderson, Owen Wilson",
                    MusicBy = "Mark Mothersbaugh",
                    Starring = "Gene Hackman, Anjelica Huston, Ben Stiller, Gwyneth Paltrow, Luke Wilson, Owen Wilson, Danny Glover, Bill Murray",
                    PosterUrls = "https://image.tmdb.org/t/p/original/nG7hZJn7wQTSDCQT39Gy3s3tbrp.jpg,https://image.tmdb.org/t/p/original/AmtiqBp7r15JuEqmyWM2dIqqtwH.jpg,https://image.tmdb.org/t/p/original/6QCpsN1AWiVDft2TyaK2PhouaFj.jpg,https://image.tmdb.org/t/p/original/6x76tjWEpFMcCVskgs6UIPntSTY.jpg"
                },
                new Movie
                {
                    Title = "The Grand Budapest Hotel",
                    Director = "Wes Anderson",
                    ReleaseYear = 2014,
                    ShortSynopsis = "The adventures of Gustave H, a legendary concierge at a famous hotel from the fictional Republic of Zubrowka between the first and second World Wars, and Zero Moustafa, the lobby boy who becomes his most trusted friend.",
                    Synopsis = "In the 1930s, the Grand Budapest Hotel is a popular European ski resort, presided over by concierge Gustave H. Zero, a junior lobby boy, becomes Gustave's friend and protege. Gustave prides himself on providing first-class service to the hotel's guests, including satisfying the sexual needs of the many elderly women who stay there. When one of Gustave's lovers dies mysteriously, Gustave finds himself the main recipient of her will, and the chief suspect in her murder.\n\nWhat follows is a madcap caper involving the theft of a priceless Renaissance painting, a battle for a family fortune, and a desperate chase across a continent on the verge of war. Told through a series of nested flashbacks, the film is a visually dazzling, witty, and surprisingly melancholic tale of friendship, loyalty, and the end of an era.",
                    WrittenBy = "Wes Anderson, Hugo Guinness",
                    MusicBy = "Alexandre Desplat",
                    Starring = "Ralph Fiennes, Tony Revolori, F. Murray Abraham, Mathieu Amalric, Adrien Brody, Willem Dafoe, Jeff Goldblum, Jude Law, Bill Murray, Edward Norton, Saoirse Ronan, Tilda Swinton",
                    PosterUrls = "https://image.tmdb.org/t/p/original/eWdyYQreja6JGCzqHWXpWHDrrPo.jpg,https://image.tmdb.org/t/p/original/lGJWIUSKmcnnxeGTtxb0Qob05Bv.jpg,https://image.tmdb.org/t/p/original/1WicOAzPkpBWajjvcFazPLIuu0A.jpg,https://image.tmdb.org/t/p/original/stc0zLGQEEdmsvD4wpAB062gTFu.jpg"
                },
                new Movie
                {
                    Title = "Moonrise Kingdom",
                    Director = "Wes Anderson",
                    ReleaseYear = 2012,
                    ShortSynopsis = "A pair of young lovers flee their New England town, which causes a local search party to fan out and find them.",
                    Synopsis = "Set on an island off the coast of New England in the summer of 1965, two twelve-year-olds, Sam and Suzy, fall in love, make a secret pact, and run away together into the wilderness. As a violent storm brews off-shore, the island's quirky residents, including the local sheriff, Suzy's parents, and Sam's Khaki Scout troop master, mobilize a search party to find them.\n\nThe film is a whimsical and heartfelt tale of first love and childhood adventure, told with Anderson's signature symmetrical visuals, deadpan humor, and a deep sense of nostalgia. It captures the feeling of being an outsider and the powerful, all-consuming nature of a childhood pact against the world of adults.",
                    WrittenBy = "Wes Anderson, Roman Coppola",
                    MusicBy = "Alexandre Desplat",
                    Starring = "Jared Gilman, Kara Hayward, Bruce Willis, Edward Norton, Bill Murray, Frances McDormand, Tilda Swinton, Jason Schwartzman",
                    PosterUrls = "https://image.tmdb.org/t/p/original/y4SXcbNl6CEF2t36icuzuBioj7K.jpg,https://image.tmdb.org/t/p/original/2iBnOt5NO9wEHQUWo82IvdoCWTR.jpg,https://image.tmdb.org/t/p/original/xVGCI7UZkS9j80XJs21P0CzWDX3.jpg,https://image.tmdb.org/t/p/original/vKuQkoHM8HlqbQjSGc5d432LSre.jpg"
                },
                new Movie
                {
                    Title = "The Darjeeling Limited",
                    Director = "Wes Anderson",
                    ReleaseYear = 2007,
                    ShortSynopsis = "Three estranged American brothers reunite for a spiritual journey across India by train a year after their father's funeral.",
                    Synopsis = "A year after the accidental death of their father, three estranged brothers—Francis, Peter, and Jack—reunite for what Francis claims will be a spiritual journey across the vibrant landscape of India. Armed with a meticulously laminated itinerary and a mountain of emotional baggage, they set out to bond with one another and find themselves. However, their trip soon veers off course amidst sibling rivalries, unresolved resentments, and a series of comic mishaps.\n\nThe journey is a stylish, funny, and poignant exploration of grief, family dysfunction, and the search for meaning in a foreign land. As they shed their literal and metaphorical baggage, the brothers are forced to confront their past and their relationships with each other in a way they never have before.",
                    WrittenBy = "Wes Anderson, Roman Coppola, Jason Schwartzman",
                    MusicBy = "Various Artists",
                    Starring = "Owen Wilson, Adrien Brody, Jason Schwartzman, Anjelica Huston, Bill Murray",
                    PosterUrls = "https://image.tmdb.org/t/p/original/oSW5OVXTulaIXcoNwJAp5YEKpbP.jpg,https://image.tmdb.org/t/p/original/xV0Fhz0woLjwiQWfauXldR3MXKJ.jpg,https://image.tmdb.org/t/p/original/p9T1dkNvhjjqlMAp5gfSVbQQxIY.jpg,https://image.tmdb.org/t/p/original/H859Q353IdDrv7ifHLGFuGq4II.jpg"
                },
                // Wong Kar-wai
                new Movie
                {
                    Title = "In The Mood For Love",
                    Director = "Wong Kar-wai",
                    ReleaseYear = 2000,
                    ShortSynopsis = "Two neighbors form a strong bond after both suspect extramarital activities of their spouses. However, they agree to keep their bond platonic so as not to commit similar wrongs.",
                    Synopsis = "A lush, melancholic, and visually stunning story set in 1960s Hong Kong. A man and a woman, neighbors in a crowded apartment building, discover that their respective spouses are having an affair. Drawn together by their shared loneliness and betrayal, they form a deep, intimate bond, but vow to keep their relationship from becoming what they despise. The film is a masterpiece of unspoken emotion, capturing longing and missed opportunities through gorgeous cinematography, elegant costumes, and a haunting score.",
                    WrittenBy = "Wong Kar-wai",
                    MusicBy = "Michael Galasso, Shigeru Umebayashi",
                    Starring = "Tony Leung, Maggie Cheung",
                    PosterUrls = "https://image.tmdb.org/t/p/original/8BgGbbWiLNhPtkMkN0gGTnbtvBv.jpg,https://image.tmdb.org/t/p/original/hc3dg28ozvLmhJJMH0Ka8UGvO0D.jpg,https://image.tmdb.org/t/p/original/2XnhBaCzaP6z9sTKRhxwKRJVrlT.jpg,https://image.tmdb.org/t/p/original/4ZPL7H93OpbHjmbP1S9oeu9wk3D.jpg"
                },
                new Movie
                {
                    Title = "2046",
                    Director = "Wong Kar-wai",
                    ReleaseYear = 2004,
                    ShortSynopsis = "A science-fiction author, who is also a journalist, has a series of failed relationships with women who drift in and out of his life.",
                    Synopsis = "A loose sequel to 'In the Mood for Love,' this film follows Chow Mo-wan, a writer and ladies' man drifting through 1960s Hong Kong. He writes a science fiction story about a mysterious place called 2046, where people go to recapture lost memories. The story becomes a vessel for his own romantic entanglements and unresolved feelings from his past. The film is a visually dazzling, non-linear exploration of memory, love, and time, weaving together multiple storylines and characters in a dreamlike, melancholic tapestry.",
                    WrittenBy = "Wong Kar-wai",
                    MusicBy = "Shigeru Umebayashi",
                    Starring = "Tony Leung, Gong Li, Faye Wong, Takuya Kimura, Zhang Ziyi",
                    PosterUrls = "https://image.tmdb.org/t/p/original/jIN65qw0Giplo4CshzMrxz204Wn.jpg,https://image.tmdb.org/t/p/original/AfQBukCdjzuLNvjyE4PWznOuiLQ.jpg,https://image.tmdb.org/t/p/original/1hzxKbCa71UcPWXISchMevtBRq3.jpg,https://image.tmdb.org/t/p/original/3N4ciArV0he1I4JduOPTvPkYx1K.jpg"
                },
                new Movie
                {
                    Title = "Fallen Angels",
                    Director = "Wong Kar-wai",
                    ReleaseYear = 1995,
                    ShortSynopsis = "A disillusioned hitman attempts to escape from his violent lifestyle against the wishes of his partner, who is infatuated with him.",
                    Synopsis = "Originally conceived as part of 'Chungking Express,' this film follows two interconnected stories set in the neon-drenched nights of Hong Kong. One story follows a cool, detached hitman who wants to quit the business, and his enigmatic female partner who cleans his apartment and pines for him from a distance. The other follows a mute, recently released convict who breaks into businesses at night and forces people to be his customers. The film is a frantic, stylish, and poignant look at urban alienation, missed connections, and the desperate search for human contact in a hyper-modern world.",
                    WrittenBy = "Wong Kar-wai",
                    MusicBy = "Frankie Chan, Roel A. Garcia",
                    Starring = "Leon Lai, Michelle Reis, Takeshi Kaneshiro, Charlie Yeung, Karen Mok",
                    PosterUrls = "https://image.tmdb.org/t/p/original/yyM9BPdwttK5LKZSLvHae7QPKo1.jpg,https://image.tmdb.org/t/p/original/cxPPU41kVizuyqoDE58som2RRzR.jpg,https://image.tmdb.org/t/p/original/nxsnArFgyCNzUoL4u7OysSuzfEV.jpg,https://image.tmdb.org/t/p/original/kJ5seTwXqVaRSpYWqGoenSJhJXi.jpg"
                },
                new Movie
                {
                    Title = "Chungking Express",
                    Director = "Wong Kar-wai",
                    ReleaseYear = 1994,
                    ShortSynopsis = "Two melancholic Hong Kong policemen fall in love: one with a mysterious female underworld figure, the other with a beautiful and ethereal server at a late-night restaurant he frequents.",
                    Synopsis = "The film tells two sequential stories about two lonely, lovelorn Hong Kong policemen. The first, Cop 223, is obsessed with his recent breakup and the expiration dates on cans of pineapple. His path crosses with a mysterious, blonde-wigged drug smuggler. The second story follows Cop 663, who is grieving a breakup with his flight attendant girlfriend. His life is quietly and whimsically turned upside down by a quirky snack bar worker who secretly breaks into his apartment to 'redecorate' his life. The film is a vibrant, energetic, and deeply romantic exploration of love, chance, and heartbreak in the bustling, transient world of Hong Kong.",
                    WrittenBy = "Wong Kar-wai",
                    MusicBy = "Frankie Chan, Roel A. Garcia",
                    Starring = "Brigitte Lin, Tony Leung, Faye Wong, Takeshi Kaneshiro",
                    PosterUrls = "https://image.tmdb.org/t/p/original/su10ww5eTaGV5Honll9AEPTblxW.jpg,https://image.tmdb.org/t/p/original/43I9DcNoCzpyzK8JCkJYpHqHqGG.jpg,https://image.tmdb.org/t/p/original/gLD2VQ3NatgLYITE65ucubdz2hl.jpg,https://image.tmdb.org/t/p/original/a5sOqIOL18Mbuy8BfmGiT79Mwud.jpg"
                },
                new Movie
                {
                    Title = "Happy Together",
                    Director = "Wong Kar-wai",
                    ReleaseYear = 1997,
                    ShortSynopsis = "A couple take a trip to Argentina but both men find their lives drifting apart in opposite directions.",
                    Synopsis = "A turbulent gay couple from Hong Kong, Ho Po-Wing and Lai Yiu-Fai, travel to Argentina seeking adventure and a new start. Their volatile, on-again, off-again relationship, however, continues its cycle of passionate reunions and bitter breakups. Stranded far from home, they drift through the vibrant but alienating streets of Buenos Aires, struggling with jealousy, dependency, and loneliness. The film is a raw, visually stunning, and emotionally powerful portrait of a destructive love affair and the profound ache of exile.",
                    WrittenBy = "Wong Kar-wai",
                    MusicBy = "Danny Chung",
                    Starring = "Leslie Cheung, Tony Leung, Chang Chen",
                    PosterUrls = "https://image.tmdb.org/t/p/original/jIv3EiZIC2tkBmjQ747Lyf5c61b.jpg,https://image.tmdb.org/t/p/original/kO4KjUkQOfWSBw06Bdl7m6AlEP7.jpg,https://image.tmdb.org/t/p/original/50S4FLeBjUKpOiHBFUFXG3JTNFZ.jpg,https://image.tmdb.org/t/p/original/ogV9HDLWbkjhDGgVNHdlus0BSel.jpg"
                },
                // David Lynch
                new Movie
                {
                    Title = "Eraserhead",
                    Director = "David Lynch",
                    ReleaseYear = 1977,
                    ShortSynopsis = "Henry Spencer tries to survive his industrial environment, his angry girlfriend, and the unbearable screams of his newly born mutant child.",
                    Synopsis = "In a bleak, industrial wasteland, a hapless factory worker named Henry Spencer finds himself trapped in a nightmarish existence. After a bizarre dinner with his girlfriend, Mary X, and her unnerving parents, he learns he is the father of a monstrous, reptilian infant. Left alone to care for the constantly wailing creature, Henry's sanity begins to fray as the lines between reality, dream, and hallucination dissolve completely.\n\nShot in stark, high-contrast black and white over a period of several years, this surrealist body horror film is a landmark of midnight cinema. It's a deeply unsettling and atmospheric journey into a man's anxieties about fatherhood, intimacy, and the suffocating pressures of his environment, all set to a meticulously crafted industrial soundscape.",
                    WrittenBy = "David Lynch",
                    MusicBy = "David Lynch, Alan Splet",
                    Starring = "Jack Nance, Charlotte Stewart, Allen Joseph, Jeanne Bates",
                    PosterUrls = "https://image.tmdb.org/t/p/original/mxveW3mGVc0DzLdOmtkZsgd7c3B.jpg,https://image.tmdb.org/t/p/original/sg9T7Xf7UYI3iZF6SzPmaVlN7gQ.jpg,https://image.tmdb.org/t/p/original/wOyk5fVyw4YdmgMie5moxivX6RT.jpg,https://image.tmdb.org/t/p/original/1gggsjyVXYwuiTFNXyTmJbCjEv1.jpg"
                },
                new Movie
                {
                    Title = "Blue Velvet",
                    Director = "David Lynch",
                    ReleaseYear = 1986,
                    ShortSynopsis = "The discovery of a severed human ear in a field leads a young man on an investigation into the dark, violent underbelly of his seemingly idyllic suburban hometown.",
                    Synopsis = "Returning to his picturesque hometown of Lumberton after his father suffers a stroke, college student Jeffrey Beaumont stumbles upon a severed human ear in a vacant lot. His morbid curiosity, shared with the wholesome detective's daughter Sandy, pulls him into a twisted mystery that lies just beneath the town's sunny, picket-fence facade.\n\nHis amateur investigation leads him to the apartment of the beautiful and tormented nightclub singer, Dorothy Vallens, and into the terrifying orbit of the sadistic, gas-huffing psychopath Frank Booth. Jeffrey is dragged into a sadomasochistic underworld of crime, violence, and depravity that forces him to confront the darkness within himself and his community.\n\nThis neo-noir mystery is a masterful and deeply disturbing exploration of the darkness lurking behind the American dream, blending idyllic suburban imagery with shocking violence and surreal, psychological horror.",
                    WrittenBy = "David Lynch",
                    MusicBy = "Angelo Badalamenti",
                    Starring = "Kyle MacLachlan, Isabella Rossellini, Dennis Hopper, Laura Dern, Dean Stockwell",
                    PosterUrls = "https://image.tmdb.org/t/p/original/7hlgzJXLgyECS1mk3LSN3E72l5N.jpg,https://image.tmdb.org/t/p/original/pt7g07WJ3Y5HGQfEA5re7q6ACao.jpg,https://image.tmdb.org/t/p/original/m0iKkzT1tSAptAGJ0vhukktDOeL.jpg,https://image.tmdb.org/t/p/original/hAzbLAgQKXToPvdZj0xb44q5dkJ.jpg"
                },
                new Movie
                {
                    Title = "Lost Highway",
                    Director = "David Lynch",
                    ReleaseYear = 1997,
                    ShortSynopsis = "After a saxophonist is convicted of murdering his wife, he inexplicably morphs into a young mechanic and begins leading a new life.",
                    Synopsis = "Fred Madison, a successful jazz saxophonist, finds his life unraveling. He and his wife, Renee, begin receiving anonymous videotapes that show the exterior, and then the interior, of their home. The mystery escalates into a nightmarish scenario that ends with Fred on death row for Renee's brutal murder. In his cell, a baffling transformation occurs: Fred vanishes, and in his place is Pete Dayton, a young auto mechanic with no memory of how he got there.\n\nReleased from prison, Pete attempts to resume his life, but is soon drawn into a dangerous affair with a gangster's moll who bears a striking resemblance to Renee. This parallel life pulls him deeper into a world of crime, jealousy, and violence, as the two fractured narratives converge in a hallucinatory, cyclical Möbius strip of identity, guilt, and paranoia. The film is a surreal, psychological thriller that defies conventional storytelling.",
                    WrittenBy = "David Lynch, Barry Gifford",
                    MusicBy = "Angelo Badalamenti, Barry Adamson",
                    Starring = "Bill Pullman, Patricia Arquette, Balthazar Getty, Robert Blake, Robert Loggia",
                    PosterUrls = "https://image.tmdb.org/t/p/original/8YptBsCZnELyXuhMqQIvwEtnZPy.jpg,https://image.tmdb.org/t/p/original/5POhfNeFPIi4VUNwCTaK85sh98r.jpg,https://image.tmdb.org/t/p/original/fdTtij6H0sX9AzIjUeynh5zbfm7.jpg,https://image.tmdb.org/t/p/original/faJjuYdq0hHraxx6o8jWEXqWJJH.jpg"
                },
                new Movie
                {
                    Title = "Mulholland Drive",
                    Director = "David Lynch",
                    ReleaseYear = 2001,
                    ShortSynopsis = "After a car wreck on the winding Mulholland Drive renders a woman amnesiac, she and a perky Hollywood-hopeful search for clues and answers across Los Angeles in a twisting venture beyond dreams and reality.",
                    Synopsis = "Following a brutal car crash on Mulholland Drive, a mysterious dark-haired woman takes refuge in a Hollywood apartment, suffering from amnesia. She is discovered by Betty Elms, a bright-eyed, aspiring actress who has just arrived in Los Angeles with dreams of stardom. Together, the two women embark on a quest to uncover the amnesiac's true identity, a journey that pulls them into a dreamlike and increasingly dangerous labyrinth of cryptic clues, strange encounters, and a sinister conspiracy simmering beneath the glamorous surface of the film industry.\n\nAs their investigation deepens, so does their relationship, blurring the lines between reality, fantasy, and nightmare. The film is a surreal, neo-noir masterpiece that shatters its own narrative, exploring the dark, desperate, and fractured nature of Hollywood dreams.",
                    WrittenBy = "David Lynch",
                    MusicBy = "Angelo Badalamenti",
                    Starring = "Naomi Watts, Laura Harring, Justin Theroux, Ann Miller, Robert Forster",
                    PosterUrls = "https://image.tmdb.org/t/p/original/x7A59t6ySylr1L7aubOQEA480vM.jpg,https://image.tmdb.org/t/p/original/d6O9xIMiVL4J3wUbutwWQWSWbXO.jpg,https://image.tmdb.org/t/p/original/tVxGt7uffLVhIIcwuldXOMpFBPX.jpg,https://image.tmdb.org/t/p/original/jCBySUid4azSTKsRkZ57ThLLCL9.jpg"
                },
                new Movie
                {
                    Title = "The Elephant Man",
                    Director = "David Lynch",
                    ReleaseYear = 1980,
                    ShortSynopsis = "A Victorian surgeon rescues a heavily disfigured man who is mistreated while scraping a living as a side-show freak. Behind his monstrous facade, there is revealed a person of kindness, intelligence and sophistication.",
                    Synopsis = "In Victorian London, Dr. Frederick Treves discovers John Merrick, a man afflicted with a horrific congenital disorder, being exploited as a sideshow attraction. Treves arranges for Merrick to be brought to the London Hospital for study, initially viewing him as a scientific curiosity. However, as he gets to know Merrick, he uncovers a soul of great intelligence, sensitivity, and dignity trapped within the monstrous exterior.\n\nMerrick becomes a celebrity in London's high society, but this newfound attention raises complex questions about charity, exploitation, and what it truly means to be human. Filmed in stunning black-and-white, this powerful and deeply moving biographical drama is a heartbreaking story of one man's struggle to be seen not as a freak, but as a person.",
                    WrittenBy = "Christopher De Vore, Eric Bergren, David Lynch",
                    MusicBy = "John Morris",
                    Starring = "Anthony Hopkins, John Hurt, Anne Bancroft, John Gielgud, Wendy Hiller",
                    PosterUrls = "https://image.tmdb.org/t/p/original/u0wpPYjuSt8DIe1Y3Vapnh8jcKE.jpg,https://image.tmdb.org/t/p/original/cKOo7LiYMMtJRUnAOTedi2CWuKe.jpg,https://image.tmdb.org/t/p/original/4yp0TNUNBfik2OHM5uzSWbMpc4Q.jpg,https://image.tmdb.org/t/p/original/1cbdKGxEC4yriksIB7XE5Ohqp76.jpg"
                },

                // Jean-Luc Godard
                new Movie
                {
                    Title = "Breathless",
                    Director = "Jean-Luc Godard",
                    ReleaseYear = 1960,
                    ShortSynopsis = "A small-time thief, wanted for murdering a policeman, tries to persuade a young American student to run away with him to Italy.",
                    Synopsis = "Michel, a charismatic and impulsive petty criminal, models himself after the tough-guy persona of Humphrey Bogart. After stealing a car in Marseille and impulsively killing a policeman, he flees to Paris. There, he reunites with Patricia, an American journalism student, and tries to convince her to run away with him to Italy while he simultaneously attempts to collect a debt to fund their escape.\n\nAs the police close in, the pair drift through a series of conversations in apartments, cafes, and city streets, discussing life, love, and loyalty. Filmed with handheld cameras and revolutionary jump cuts, this iconic film is a landmark of the French New Wave. It's an effortlessly cool and energetic deconstruction of the gangster genre, capturing a spirit of youthful rebellion and cinematic freedom.",
                    WrittenBy = "Jean-Luc Godard, François Truffaut",
                    MusicBy = "Martial Solal",
                    Starring = "Jean-Paul Belmondo, Jean Seberg, Daniel Boulanger",
                    PosterUrls = "https://image.tmdb.org/t/p/original/9Wx0Wdn2EOqeCZU4SP6tlS3LOml.jpg,https://image.tmdb.org/t/p/original/aUaTg7Rz0LeNV3EsiMXTxcOCb19.jpg,https://image.tmdb.org/t/p/original/iAFdhBOJZpaGA8ZF1ao3taLD6Yv.jpg,https://image.tmdb.org/t/p/original/n5Ly6FRUwIyBnrQN8QWp8Qi86x.jpg"
                },
                new Movie
                {
                    Title = "Pierrot le Fou",
                    Director = "Jean-Luc Godard",
                    ReleaseYear = 1965,
                    ShortSynopsis = "Unhappy in his marriage, Ferdinand Griffon runs away with Marianne, an old flame, and heads for the south of France in a spree of crime and adventure.",
                    Synopsis = "Ferdinand Griffon, a man bored with his bourgeois life and marriage, impulsively runs away from a party and escapes to the south of France with Marianne, the family babysitter and a former lover with a dangerous past. Calling him 'Pierrot,' she pulls him into a chaotic, pop-art-colored road trip, leaving a trail of stolen cars and dead bodies in their wake as they are pursued by gangsters.\n\nTheir journey is a frantic and unpredictable escape from society, filled with spontaneous musical numbers, philosophical musings, and cinematic experiments. It's a vibrant and tragic tale of a couple on the run, a signature film of the French New Wave that is both a deconstruction of the crime genre and a poignant story of a doomed romance.",
                    WrittenBy = "Jean-Luc Godard",
                    MusicBy = "Antoine Duhamel",
                    Starring = "Jean-Paul Belmondo, Anna Karina, Graziella Galvani",
                    PosterUrls = "https://image.tmdb.org/t/p/original/i124H6iQB4CawrgFW9aZaZs7OBO.jpg,https://image.tmdb.org/t/p/original/6XIV4JYYRquskjFzYmaA3OjgjKL.jpg,https://image.tmdb.org/t/p/original/7CgGfy016TncbqDMsgVroHT5hPc.jpg"
                },
                new Movie
                {
                    Title = "Masculin Féminin",
                    Director = "Jean-Luc Godard",
                    ReleaseYear = 1966,
                    ShortSynopsis = "A young, romantic idealist gets involved with a budding pop singer and her friends, navigating the social and political landscape of 1960s Paris.",
                    Synopsis = "Paul, a young man fresh from military service, is a romantic and politically-minded idealist trying to find his place in the world. He becomes infatuated with Madeleine, an aspiring yé-yé pop singer who is more concerned with her career and consumer culture than with his political activism. Through a series of vignettes and interviews, the film chronicles their fragile relationship and their interactions with their friends, exploring the attitudes and anxieties of French youth in the mid-1960s.\n\nDescribed by Godard as being about 'the children of Marx and Coca-Cola,' the film is a witty, insightful, and often funny snapshot of a generation caught between revolutionary politics and the allure of pop culture. It captures the spirit of a specific time and place with a unique blend of documentary-style realism and playful cinematic invention.",
                    WrittenBy = "Jean-Luc Godard",
                    MusicBy = "Jean-Jacques Debout",
                    Starring = "Jean-Pierre Léaud, Chantal Goya, Marlène Jobert",
                    PosterUrls = "https://image.tmdb.org/t/p/original/AroNQnZiqjJOWf32lLNZUzWlGvd.jpg,https://image.tmdb.org/t/p/original/q3DOYFQTNTImKsJ0GwJ70GuEAbE.jpg,https://image.tmdb.org/t/p/original/4nWOFJTHAHVbPdiNl3VmjA45DoP.jpg"
                },
                new Movie
                {
                    Title = "Alphaville",
                    Director = "Jean-Luc Godard",
                    ReleaseYear = 1965,
                    ShortSynopsis = "A secret agent is sent to a dystopian city run by a tyrannical supercomputer, where he must find a missing person and destroy the machine.",
                    Synopsis = "Lemmy Caution, a grizzled American secret agent, travels to Alphaville, a futuristic city on another planet, in a Ford Mustang. His mission is to find a missing agent and assassinate the city's creator, Professor von Braun. Alphaville is a totalitarian state ruled by a sentient computer, Alpha 60, which has outlawed all emotion and free thought, executing citizens who show any sign of love, poetry, or grief.\n\nCaution navigates the sterile, oppressive city, teaming up with the professor's daughter, Natacha, who has never known a world with emotion. He must complete his mission while trying to reawaken her humanity and escape the all-seeing eye of Alpha 60. Filmed in contemporary Paris, this dystopian science-fiction film noir is a unique and influential blend of genres, using real locations to create a chilling vision of a dehumanized future.",
                    WrittenBy = "Jean-Luc Godard",
                    MusicBy = "Paul Misraki",
                    Starring = "Eddie Constantine, Anna Karina, Akim Tamiroff",
                    PosterUrls = "https://image.tmdb.org/t/p/original/fFJP3D5fJDFxN7ChqSye1DZ0fTL.jpg,https://image.tmdb.org/t/p/original/vcIJh8WFYgSCqfjNZVi0q2jyVIM.jpg,https://image.tmdb.org/t/p/original/j5n7d0YuFqIo0u6R9VJ22UyAceq.jpg,https://image.tmdb.org/t/p/original/x6LpEHnETtxzLmIwNQLrMVucIve.jpg"
                },
                new Movie
                {
                    Title = "2 or 3 Things I Know About Her",
                    Director = "Jean-Luc Godard",
                    ReleaseYear = 1967,
                    ShortSynopsis = "A day in the life of Juliette Janson, a Parisian housewife who moonlights as a prostitute to afford the latest consumer goods, is used as a lens to examine life in the modern city.",
                    Synopsis = "The 'her' of the title refers to both Juliette Janson, a wife and mother living in a massive new housing project in the Parisian suburbs, and the city of Paris itself. Over the course of a single day, the film follows Juliette as she goes about her daily routine, which includes running errands, caring for her children, and working as a part-time prostitute to make ends meet and afford the consumer goods that define modern life.\n\nThrough a series of fragmented scenes, philosophical voiceovers, and direct-to-camera whispers, Godard creates a kaleidoscopic and critical essay on life in a modern, consumerist society. It's a visually stunning and intellectually rigorous exploration of sociology, capitalism, and the difficulty of representing reality on film.",
                    WrittenBy = "Jean-Luc Godard",
                    MusicBy = "Ludwig van Beethoven",
                    Starring = "Marina Vlady, Anny Duperey, Roger Montsoret",
                    PosterUrls = "https://image.tmdb.org/t/p/original/w1LO1SIrpA4GCjK3ysANr7F3eAN.jpg,https://image.tmdb.org/t/p/original/w0LilVoboSNFajkDLlbFllt59gz.jpg,https://image.tmdb.org/t/p/original/kquvP2icpOjMB3Kz8BcWx4dHPoF.jpg"
                },

                // Krzysztof Kieślowski
                new Movie
                {
                    Title = "Three Colours: Red",
                    Director = "Krzysztof Kieślowski",
                    ReleaseYear = 1994,
                    ShortSynopsis = "A model discovers her neighbor is a retired judge who is eavesdropping on his neighbors' phone calls, and the two form an unlikely bond.",
                    Synopsis = "Valentine, a young model living in Geneva, accidentally hits a dog with her car. When she returns the dog to its owner, she discovers he is a reclusive, embittered retired judge who spends his days illegally listening in on his neighbors' telephone conversations. Though initially repulsed, she finds herself drawn back to him, and the two develop a strange and profound connection across a generational divide.\n\nTheir story runs parallel to that of a young law student, whose life unknowingly mirrors the judge's past. This final, masterful installment of the Three Colours trilogy is a visually stunning and deeply moving meditation on chance, destiny, and the complex, often invisible, connections that bind human lives together.",
                    WrittenBy = "Krzysztof Kieślowski, Krzysztof Piesiewicz",
                    MusicBy = "Zbigniew Preisner",
                    Starring = "Irène Jacob, Jean-Louis Trintignant, Frédérique Feder",
                    PosterUrls = "https://image.tmdb.org/t/p/original/5YIzZaQlwYJY5ZEqHRs9dG9DduB.jpg,https://image.tmdb.org/t/p/original/JHmsBiX1tjCKqAul1lzC20WcAW.jpg,https://image.tmdb.org/t/p/original/irlGcVOG7CJ29yXIB7nVcEQTKbC.jpg,https://image.tmdb.org/t/p/original/pBPhZ3pAPS9RZwZWERSngJek5Uo.jpg"
                },
                new Movie
                {
                    Title = "Three Colours: Blue",
                    Director = "Krzysztof Kieślowski",
                    ReleaseYear = 1993,
                    ShortSynopsis = "Following the death of her husband and child in a car accident, a woman attempts to cut herself off from her past and live a life of complete emotional isolation.",
                    Synopsis = "Julie loses her renowned composer husband and young daughter in a car crash. Shattered by grief, she attempts to erase her past and achieve a state of absolute liberty by severing all personal ties. She sells her country home, moves to an anonymous Parisian apartment, and tries to live a life free from memory, possession, and love. However, the world relentlessly intrudes, forcing her to confront her past through the people she meets and the powerful, unfinished musical score her husband left behind.\n\nThe first film in the Three Colours trilogy is a visually breathtaking and emotionally overwhelming study of grief, memory, and the struggle for freedom. It's a profound and ultimately hopeful story about the inescapable nature of human connection.",
                    WrittenBy = "Krzysztof Kieślowski, Krzysztof Piesiewicz",
                    MusicBy = "Zbigniew Preisner",
                    Starring = "Juliette Binoche, Benoît Régent, Florence Pernel",
                    PosterUrls = "https://image.tmdb.org/t/p/original/33wsWxzsNstI8N7dvuwzFmj1qBd.jpg,https://image.tmdb.org/t/p/original/azyowBDgVxqFMYHbe9spUsSawO2.jpg,https://image.tmdb.org/t/p/original/sHZuik2Tgknq8wJsIW0nbuIFXZ8.jpg,https://image.tmdb.org/t/p/original/xyvAku0xRGeYUe6x5Cz5rofSNmQ.jpg"
                },
                new Movie
                {
                    Title = "Three Colours: White",
                    Director = "Krzysztof Kieślowski",
                    ReleaseYear = 1994,
                    ShortSynopsis = "Humiliated and abandoned by his wife in Paris, a Polish immigrant plots an elaborate and darkly comic revenge.",
                    Synopsis = "Karol Karol, a Polish hairdresser, finds his life in ruins after his French wife, Dominique, divorces him, takes all his money, and leaves him destitute and stranded in Paris. Humiliated and unable to even speak the language, he manages to smuggle himself back to his native Poland inside a suitcase. Once home, he sets out on a new life, determined to achieve success in the new capitalist Poland and, ultimately, to exact a complex and ironic revenge on the woman who wronged him.\n\nThe second, and most comedic, film in the Three Colours trilogy is a clever and poignant black comedy about equality, revenge, and the bittersweet nature of starting over. It's a story of a man trying to reclaim his dignity and prove that he is more than just a victim of circumstance.",
                    WrittenBy = "Krzysztof Kieślowski, Krzysztof Piesiewicz",
                    MusicBy = "Zbigniew Preisner",
                    Starring = "Zbigniew Zamachowski, Julie Delpy, Janusz Gajos",
                    PosterUrls = "https://image.tmdb.org/t/p/original/4jDSCLCWxHutCAZRve8Kkso2UfA.jpg,https://image.tmdb.org/t/p/original/fdIet3NSa27gobMbaUml66oCQNT.jpg,https://image.tmdb.org/t/p/original/4YGo9kIpaAgXns1xDjoCU0nKm4O.jpg,https://image.tmdb.org/t/p/original/knjYj5f8k1xzTFMUnXr5vvKICOi.jpg"
                },
                new Movie
                {
                    Title = "The Double Life of Véronique",
                    Director = "Krzysztof Kieślowski",
                    ReleaseYear = 1991,
                    ShortSynopsis = "Two identical women—one in Poland, the other in France—share a mysterious and profound emotional connection, though they are unaware of each other's existence.",
                    Synopsis = "Weronika, a gifted young singer in Poland, and Véronique, a music teacher in France, are two women who, though strangers, are physically identical and share a deep, intuitive, and inexplicable bond. They have never met, but their lives are subtly intertwined, with one seemingly learning from the unspoken experiences of the other. The film follows their parallel lives, exploring themes of identity, intuition, and the metaphysical connections that transcend physical boundaries.\n\nThis is a beautiful, enigmatic, and deeply poetic film that explores the mysterious and intangible aspects of human existence. It's a visually stunning and emotionally resonant masterpiece that asks profound questions about the nature of the soul and the possibility of unseen doubles.",
                    WrittenBy = "Krzysztof Kieślowski, Krzysztof Piesiewicz",
                    MusicBy = "Zbigniew Preisner",
                    Starring = "Irène Jacob, Halina Gryglaszewska, Kalina Jędrusik",
                    PosterUrls = "https://image.tmdb.org/t/p/original/oqRyO9xrNBRaxqF9pCHHgLuaATx.jpg,https://image.tmdb.org/t/p/original/6qPx9PWXrmYv2Vx4nyx2vk4LiFE.jpg,https://image.tmdb.org/t/p/original/9yycXqhjN4KjpbxbzPdIDMdW9iM.jpg,https://image.tmdb.org/t/p/original/t7NAywfL42IabhAfzIxAeX46S2L.jpg"
                },
                new Movie
                {
                    Title = "A Short Film About Killing",
                    Director = "Krzysztof Kieślowski",
                    ReleaseYear = 1988,
                    ShortSynopsis = "The paths of a cynical, idealistic young lawyer, a brutal and aimless drifter, and a callous taxi driver converge in two acts of killing: one a senseless murder, the other a state-sanctioned execution.",
                    Synopsis = "Over the course of a grey day in Warsaw, the lives of three men are set on a collision course. Jacek is a young, alienated man who wanders the city with a piece of rope, seemingly without purpose. Piotr is an idealistic young lawyer on the cusp of his career, preparing for his final exam. And a coarse taxi driver goes about his day, antagonizing those he meets. Their paths cross in a shocking and brutal act of violence when Jacek senselessly murders the taxi driver.\n\nThe second half of the film follows Piotr as he is appointed to be Jacek's defense attorney. He fights against the cold, impersonal machinery of the state's justice system, which culminates in the second act of killing: Jacek's execution. This powerful and deeply disturbing film is an unflinching and visceral condemnation of capital punishment, arguing that both individual murder and state-sanctioned killing are equally horrific.",
                    WrittenBy = "Krzysztof Kieślowski, Krzysztof Piesiewicz",
                    MusicBy = "Zbigniew Preisner",
                    Starring = "Mirosław Baka, Krzysztof Globisz, Jan Tesarz",
                    PosterUrls = "https://image.tmdb.org/t/p/original/fxdTFJuTL0XVf7xl8FPVITDHxbx.jpg,https://image.tmdb.org/t/p/original/nUhg75REW4bIHWe1K7kxeNE8KXM.jpg,https://image.tmdb.org/t/p/original/x8zenootZTTB1iEdUQA9dnEuLqv.jpg,https://image.tmdb.org/t/p/original/k7sk4yNdoXY7iwp1M9QTZuBDiJS.jpg"
                }
            );
            #endregion
        }
    }
}
