﻿// <auto-generated />
using System;
using AscendionAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AscendionAPI.Migrations
{
    [DbContext(typeof(NZWalksDbContext))]
    [Migration("20241209184155_AddWorkshopSessions")]
    partial class AddWorkshopSessions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("AscendionAPI.Models.Domain.Difficulty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Difficulties");

                    b.HasData(
                        new
                        {
                            Id = new Guid("54466f17-02af-48e7-8ed3-5a4a8bfacf6f"),
                            Name = "Easy"
                        },
                        new
                        {
                            Id = new Guid("ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c"),
                            Name = "Medium"
                        },
                        new
                        {
                            Id = new Guid("f808ddcd-b5e5-4d80-b732-1ca523e48434"),
                            Name = "Hard"
                        });
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Region", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RegionImageUrl")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Regions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                            Code = "AKL",
                            Name = "Auckland",
                            RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                        },
                        new
                        {
                            Id = new Guid("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                            Code = "NTL",
                            Name = "Northland"
                        },
                        new
                        {
                            Id = new Guid("14ceba71-4b51-4777-9b17-46602cf66153"),
                            Code = "BOP",
                            Name = "Bay Of Plenty"
                        },
                        new
                        {
                            Id = new Guid("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                            Code = "WGN",
                            Name = "Wellington",
                            RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                        },
                        new
                        {
                            Id = new Guid("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                            Code = "NSN",
                            Name = "Nelson",
                            RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                        },
                        new
                        {
                            Id = new Guid("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                            Code = "STL",
                            Name = "Southland"
                        });
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abstract")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("varchar(2048)");

                    b.Property<double>("Duration")
                        .HasColumnType("double");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SequenceId")
                        .HasColumnType("int");

                    b.Property<string>("Speaker")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("UpvoteCount")
                        .HasColumnType("int");

                    b.Property<int>("WorkshopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkshopId");

                    b.ToTable("Sessions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Abstract = "In this session you will learn about the basics of Angular JS",
                            Duration = 1.0,
                            Level = "Basic",
                            Name = "Introduction to Angular JS",
                            SequenceId = 1,
                            Speaker = "John Doe",
                            UpvoteCount = 0,
                            WorkshopId = 1
                        },
                        new
                        {
                            Id = 2,
                            Abstract = "This session will take a closer look at scopes.  Learn what they do, how they do it, and how to get them to do it for you.",
                            Duration = 0.5,
                            Level = "Basic",
                            Name = "Scopes in Angular JS",
                            SequenceId = 2,
                            Speaker = "John Doe",
                            UpvoteCount = 0,
                            WorkshopId = 1
                        },
                        new
                        {
                            Id = 3,
                            Abstract = "In this session you will learn about the basics of React JS",
                            Duration = 0.5,
                            Level = "Basic",
                            Name = "Introduction to React JS",
                            SequenceId = 1,
                            Speaker = "Paul Smith",
                            UpvoteCount = 0,
                            WorkshopId = 2
                        },
                        new
                        {
                            Id = 4,
                            Abstract = "Learn how to use JSX to create view and bind data to it",
                            Duration = 2.0,
                            Level = "Basic",
                            Name = "JSX",
                            SequenceId = 2,
                            Speaker = "Paul Smith",
                            UpvoteCount = 0,
                            WorkshopId = 2
                        });
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Walk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("DifficultyId")
                        .HasColumnType("char(36)");

                    b.Property<double>("LengthInKm")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("RegionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("WalkImageUrl")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DifficultyId");

                    b.HasIndex("RegionId");

                    b.ToTable("Walks");
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Workshop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("varchar(2048)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time(6)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.ToTable("Workshops");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "frontend",
                            Description = "<p><strong>AngularJS</strong> (also written as <strong>Angular.js</strong>) is a JavaScript-based open-source front-end web application framework mainly maintained by Google and by a community of individuals and corporations to address many of the challenges encountered in developing single-page applications.</p><p>It aims to simplify both the development and the testing of such applications by providing a framework for client-side model–view–controller (MVC) and model–view–viewmodel (MVVM) architectures, along with components commonly used in rich Internet applications. (This flexibility has led to the acronym MVW, which stands for \"model-view-whatever\" and may also encompass model–view–presenter and model–view–adapter.)</p>",
                            EndDate = new DateTime(2019, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EndTime = new TimeOnly(13, 30, 0),
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/ca/AngularJS_logo.svg/2000px-AngularJS_logo.svg.png",
                            Name = "Angular JS Bootcamp",
                            StartDate = new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StartTime = new TimeOnly(9, 30, 0)
                        },
                        new
                        {
                            Id = 2,
                            Category = "frontend",
                            Description = "<p><strong>React</strong> (also known as <strong>React.js</strong> or <strong>ReactJS</strong>) is a JavaScript library for building user interfaces. It is maintained by Facebook and a community of individual developers and companies.</p><p>React can be used as a base in the development of single-page or mobile applications. Complex React applications usually require the use of additional libraries for state management, routing, and interaction with an API.</p>",
                            EndDate = new DateTime(2019, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EndTime = new TimeOnly(17, 30, 0),
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/React-icon.svg/640px-React-icon.svg.png",
                            Name = "React JS Masterclass",
                            StartDate = new DateTime(2019, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StartTime = new TimeOnly(10, 0, 0)
                        },
                        new
                        {
                            Id = 3,
                            Category = "database",
                            Description = "<p><strong>MongoDB</strong> is a cross-platform document-oriented database program. It is issued under the Server Side Public License (SSPL) version 1, which was submitted for certification to the Open Source Initiative but later withdrawn in lieu of SSPL version 2. Classified as a NoSQL database program, MongoDB uses JSON-like documents with schemata. MongoDB is developed by MongoDB Inc.</p><p>MongoDB supports field, range query, and regular expression searches. Queries can return specific fields of documents and also include user-defined JavaScript functions. Queries can also be configured to return a random sample of results of a given size.</p>",
                            EndDate = new DateTime(2019, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EndTime = new TimeOnly(16, 30, 0),
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/32/Mongo-db-logo.png",
                            Name = "Crash course in MongoDB",
                            StartDate = new DateTime(2019, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StartTime = new TimeOnly(12, 30, 0)
                        });
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Session", b =>
                {
                    b.HasOne("AscendionAPI.Models.Domain.Workshop", "Workshop")
                        .WithMany("Sessions")
                        .HasForeignKey("WorkshopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workshop");
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Walk", b =>
                {
                    b.HasOne("AscendionAPI.Models.Domain.Difficulty", "Difficulty")
                        .WithMany()
                        .HasForeignKey("DifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AscendionAPI.Models.Domain.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Difficulty");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Workshop", b =>
                {
                    b.OwnsOne("AscendionAPI.Models.Domain.Location", "Location", b1 =>
                        {
                            b1.Property<int>("WorkshopId")
                                .HasColumnType("int");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("Address");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("City");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("State");

                            b1.HasKey("WorkshopId");

                            b1.ToTable("Workshops");

                            b1.WithOwner()
                                .HasForeignKey("WorkshopId");

                            b1.HasData(
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
                                });
                        });

                    b.OwnsOne("AscendionAPI.Models.Domain.Modes", "Modes", b1 =>
                        {
                            b1.Property<int>("WorkshopId")
                                .HasColumnType("int");

                            b1.Property<bool>("InPerson")
                                .HasColumnType("tinyint(1)")
                                .HasColumnName("InPerson");

                            b1.Property<bool>("Online")
                                .HasColumnType("tinyint(1)")
                                .HasColumnName("Online");

                            b1.HasKey("WorkshopId");

                            b1.ToTable("Workshops");

                            b1.WithOwner()
                                .HasForeignKey("WorkshopId");

                            b1.HasData(
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
                                });
                        });

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("Modes")
                        .IsRequired();
                });

            modelBuilder.Entity("AscendionAPI.Models.Domain.Workshop", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}