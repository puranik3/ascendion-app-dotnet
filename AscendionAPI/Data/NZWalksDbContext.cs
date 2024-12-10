using System;
using Microsoft.EntityFrameworkCore;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Data;

public class NZWalksDbContext : DbContext
{
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Walk> Walks { get; set; }

    public DbSet<Workshop> Workshops { get; set; }
    public DbSet<Session> Sessions { get; set; }

    public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
	{
	}

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating(modelBuilder);

        // If you the seeding code below after a while since creating the app and working on it, drop all records from the tables in the DB, and add a migration and update the database using the new migration
        // dotnet ef migrations add SeedDataMigrationForDifficultiesAndRegions
        // dotnet ef database update --verbose

        // Seed data for Difficulties
        // Easy, Medium, Hard
        var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("54466f17-02af-48e7-8ed3-5a4a8bfacf6f"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("f808ddcd-b5e5-4d80-b732-1ca523e48434"),
                    Name = "Hard"
                }
            };

        // Seed difficulties to the database
        modelBuilder.Entity<Difficulty>().HasData(difficulties);

        // Seed data for Regions
        var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

        modelBuilder.Entity<Region>().HasData(regions);

        // Define one-to-many relationship between Workshop and Session
        modelBuilder.Entity<Workshop>()
            .HasMany(w => w.Sessions)
            .WithOne(s => s.Workshop)
            .HasForeignKey(s => s.WorkshopId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        // Configure 'Location' as an owned by 'Workshop'
        modelBuilder.Entity<Workshop>()
            .OwnsOne(w => w.Location, l =>
            {
                l.Property(loc => loc.Address).HasColumnName("Address");
                l.Property(loc => loc.City).HasColumnName("City");
                l.Property(loc => loc.State).HasColumnName("State");
            });

        // Configure 'Modes' as an owned by 'Workshop'
        modelBuilder.Entity<Workshop>()
            .OwnsOne(w => w.Modes, l =>
            {
                l.Property(loc => loc.InPerson).HasColumnName("InPerson");
                l.Property(loc => loc.Online).HasColumnName("Online");
            });

        //  Seed data for workshops
        var workshops = new List<Workshop>
        {
            new Workshop
            {
                Name = "Angular JS Bootcamp",
                Category = "frontend",
                Id = 1,
                Description = "<p><strong>AngularJS</strong> (also written as <strong>Angular.js</strong>) is a JavaScript-based open-source front-end web application framework mainly maintained by Google and by a community of individuals and corporations to address many of the challenges encountered in developing single-page applications.</p><p>It aims to simplify both the development and the testing of such applications by providing a framework for client-side model–view–controller (MVC) and model–view–viewmodel (MVVM) architectures, along with components commonly used in rich Internet applications. (This flexibility has led to the acronym MVW, which stands for \"model-view-whatever\" and may also encompass model–view–presenter and model–view–adapter.)</p>",
                StartDate = new DateTime(2019, 1, 1),
                EndDate = new DateTime(2019, 1, 3),
                StartTime = new TimeOnly(9, 30),
                EndTime = new TimeOnly(13, 30),
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/ca/AngularJS_logo.svg/2000px-AngularJS_logo.svg.png"
            },
            new Workshop
            {
                Name = "React JS Masterclass",
                Category = "frontend",
                Id = 2,
                Description = "<p><strong>React</strong> (also known as <strong>React.js</strong> or <strong>ReactJS</strong>) is a JavaScript library for building user interfaces. It is maintained by Facebook and a community of individual developers and companies.</p><p>React can be used as a base in the development of single-page or mobile applications. Complex React applications usually require the use of additional libraries for state management, routing, and interaction with an API.</p>",
                StartDate = new DateTime(2019, 1, 14),
                EndDate = new DateTime(2019, 1, 16),
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(17, 30),
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/React-icon.svg/640px-React-icon.svg.png"
            },
            new Workshop
            {
                Name = "Crash course in MongoDB",
                Category = "database",
                Id = 3,
                Description = "<p><strong>MongoDB</strong> is a cross-platform document-oriented database program. It is issued under the Server Side Public License (SSPL) version 1, which was submitted for certification to the Open Source Initiative but later withdrawn in lieu of SSPL version 2. Classified as a NoSQL database program, MongoDB uses JSON-like documents with schemata. MongoDB is developed by MongoDB Inc.</p><p>MongoDB supports field, range query, and regular expression searches. Queries can return specific fields of documents and also include user-defined JavaScript functions. Queries can also be configured to return a random sample of results of a given size.</p>",
                StartDate = new DateTime(2019, 1, 20),
                EndDate = new DateTime(2019, 1, 22),
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(16, 30),
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/32/Mongo-db-logo.png"
            },
        };

        modelBuilder.Entity<Workshop>().HasData(workshops);

        // Seed data for Location (Owned Type)
        modelBuilder.Entity<Workshop>()
            .OwnsOne(w => w.Location)
            .HasData(
                new
                {
                    WorkshopId = 1,
                    Address = "Tata Elxsi, Prestige Shantiniketan",
                    City = "Bangalore",
                    State = "Karnataka"
                },
                new
                {
                    WorkshopId = 2,
                    Address = "Tata Elxsi, IT Park",
                    City = "Trivandrum",
                    State = "Kerala"
                },
                new
                {
                    WorkshopId = 3,
                    Address = "HCL, Electronic City Phase 1",
                    City = "Bangalore",
                    State = "Karnataka"
                }
            );

        // Seed data for Modes (Owned Type)
        modelBuilder.Entity<Workshop>()
            .OwnsOne(w => w.Modes)
            .HasData(
                new
                {
                    WorkshopId = 1,
                    InPerson = true,
                    Online = false
                },
                new
                {
                    WorkshopId = 2,
                    InPerson = true,
                    Online = true
                },
                new
                {
                    WorkshopId = 3,
                    InPerson = false,
                    Online = true
                }
            );

        // Seed data for sessions
        var sessions = new List<Session>
        {
            new Session
            {
                Id = 1,
                WorkshopId = 1,
                SequenceId = 1,
                Name = "Introduction to Angular JS",
                Speaker = "John Doe",
                Duration = 1,
                Level = "Basic",
                Abstract = "In this session you will learn about the basics of Angular JS"
            },
            new Session
            {
                Id = 2,
                WorkshopId = 1,
                SequenceId = 2,
                Name = "Scopes in Angular JS",
                Speaker = "John Doe",
                Duration = 0.5,
                Level = "Basic",
                Abstract = "This session will take a closer look at scopes.  Learn what they do, how they do it, and how to get them to do it for you."
            },
            new Session
            {
                Id = 3,
                WorkshopId = 2,
                SequenceId = 1,
                Name = "Introduction to React JS",
                Speaker = "Paul Smith",
                Duration = 0.5,
                Level = "Basic",
                Abstract = "In this session you will learn about the basics of React JS",
            },
            new Session
            {
                Id = 4,
                WorkshopId = 2,
                SequenceId = 2,
                Name = "JSX",
                Speaker = "Paul Smith",
                Duration = 2,
                Level = "Basic",
                Abstract = "Learn how to use JSX to create view and bind data to it",
            },
        };

        modelBuilder.Entity<Session>().HasData(sessions);
    }
}