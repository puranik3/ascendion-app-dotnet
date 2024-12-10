using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AscendionAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkshopSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InPerson = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Online = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkshopId = table.Column<int>(type: "int", nullable: false),
                    SequenceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Speaker = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<double>(type: "double", nullable: false),
                    Level = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Abstract = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpvoteCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Workshops",
                columns: new[] { "Id", "Address", "City", "State", "InPerson", "Online", "Category", "Description", "EndDate", "EndTime", "ImageUrl", "Name", "StartDate", "StartTime" },
                values: new object[,]
                {
                    { 1, "Tata Elxsi, Prestige Shantiniketan", "Bangalore", "Karnataka", true, false, "frontend", "<p><strong>AngularJS</strong> (also written as <strong>Angular.js</strong>) is a JavaScript-based open-source front-end web application framework mainly maintained by Google and by a community of individuals and corporations to address many of the challenges encountered in developing single-page applications.</p><p>It aims to simplify both the development and the testing of such applications by providing a framework for client-side model–view–controller (MVC) and model–view–viewmodel (MVVM) architectures, along with components commonly used in rich Internet applications. (This flexibility has led to the acronym MVW, which stands for \"model-view-whatever\" and may also encompass model–view–presenter and model–view–adapter.)</p>", new DateTime(2019, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(13, 30, 0), "https://upload.wikimedia.org/wikipedia/commons/thumb/c/ca/AngularJS_logo.svg/2000px-AngularJS_logo.svg.png", "Angular JS Bootcamp", new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(9, 30, 0) },
                    { 2, "Tata Elxsi, IT Park", "Trivandrum", "Kerala", true, true, "frontend", "<p><strong>React</strong> (also known as <strong>React.js</strong> or <strong>ReactJS</strong>) is a JavaScript library for building user interfaces. It is maintained by Facebook and a community of individual developers and companies.</p><p>React can be used as a base in the development of single-page or mobile applications. Complex React applications usually require the use of additional libraries for state management, routing, and interaction with an API.</p>", new DateTime(2019, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 30, 0), "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/React-icon.svg/640px-React-icon.svg.png", "React JS Masterclass", new DateTime(2019, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(10, 0, 0) },
                    { 3, "HCL, Electronic City Phase 1", "Bangalore", "Karnataka", false, true, "database", "<p><strong>MongoDB</strong> is a cross-platform document-oriented database program. It is issued under the Server Side Public License (SSPL) version 1, which was submitted for certification to the Open Source Initiative but later withdrawn in lieu of SSPL version 2. Classified as a NoSQL database program, MongoDB uses JSON-like documents with schemata. MongoDB is developed by MongoDB Inc.</p><p>MongoDB supports field, range query, and regular expression searches. Queries can return specific fields of documents and also include user-defined JavaScript functions. Queries can also be configured to return a random sample of results of a given size.</p>", new DateTime(2019, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(16, 30, 0), "https://upload.wikimedia.org/wikipedia/commons/3/32/Mongo-db-logo.png", "Crash course in MongoDB", new DateTime(2019, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 30, 0) }
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "Abstract", "Duration", "Level", "Name", "SequenceId", "Speaker", "UpvoteCount", "WorkshopId" },
                values: new object[,]
                {
                    { 1, "In this session you will learn about the basics of Angular JS", 1.0, "Basic", "Introduction to Angular JS", 1, "John Doe", 0, 1 },
                    { 2, "This session will take a closer look at scopes.  Learn what they do, how they do it, and how to get them to do it for you.", 0.5, "Basic", "Scopes in Angular JS", 2, "John Doe", 0, 1 },
                    { 3, "In this session you will learn about the basics of React JS", 0.5, "Basic", "Introduction to React JS", 1, "Paul Smith", 0, 2 },
                    { 4, "Learn how to use JSX to create view and bind data to it", 2.0, "Basic", "JSX", 2, "Paul Smith", 0, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_WorkshopId",
                table: "Sessions",
                column: "WorkshopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Workshops");
        }
    }
}
