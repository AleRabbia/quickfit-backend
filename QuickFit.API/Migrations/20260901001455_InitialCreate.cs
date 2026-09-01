using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuickFit.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "user"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "meal_plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Goal = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DietType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DailyCalories = table.Column<int>(type: "integer", nullable: false),
                    DailyProtein = table.Column<int>(type: "integer", nullable: false),
                    DailyCarbs = table.Column<int>(type: "integer", nullable: false),
                    DailyFats = table.Column<int>(type: "integer", nullable: false),
                    MealsPerDay = table.Column<int>(type: "integer", nullable: false),
                    Allergies = table.Column<string>(type: "json", nullable: false),
                    Intolerances = table.Column<string>(type: "json", nullable: false),
                    DislikedFoods = table.Column<string>(type: "json", nullable: false),
                    Budget = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CookingTime = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    GeneratedByAI = table.Column<bool>(type: "boolean", nullable: false),
                    AIPrompt = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_meal_plans_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_achievements_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Height = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ActivityLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_profiles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_progress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Waist = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Chest = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Hips = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Arms = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Legs = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    BodyFatPercentage = table.Column<decimal>(type: "numeric(4,2)", nullable: true),
                    MuscleMass = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Photos = table.Column<string>(type: "json", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    WeeklyWorkouts = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_progress_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workout_plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Goal = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ExperienceLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    DaysPerWeek = table.Column<int>(type: "integer", nullable: false),
                    TrainingStyle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TrainingPlace = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Equipment = table.Column<string>(type: "json", nullable: true),
                    MedicalHistory = table.Column<string>(type: "text", nullable: true),
                    DislikedExercises = table.Column<string>(type: "text", nullable: true),
                    TrainingFocus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    GeneratedByAI = table.Column<bool>(type: "boolean", nullable: false),
                    AIPrompt = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workout_plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workout_plans_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MealPlanId = table.Column<int>(type: "integer", nullable: false),
                    MealType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TimeOfDay = table.Column<TimeSpan>(type: "time", nullable: true),
                    Calories = table.Column<int>(type: "integer", nullable: false),
                    Protein = table.Column<int>(type: "integer", nullable: false),
                    Carbs = table.Column<int>(type: "integer", nullable: false),
                    Fats = table.Column<int>(type: "integer", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    RecipeUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_meals_meal_plans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "meal_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workout_sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutPlanId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EstimatedCalories = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workout_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workout_sessions_workout_plans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "workout_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meal_foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MealId = table.Column<int>(type: "integer", nullable: false),
                    FoodName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Calories = table.Column<int>(type: "integer", nullable: false),
                    Protein = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Carbs = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Fats = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_foods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_meal_foods_meals_MealId",
                        column: x => x.MealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meal_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MealId = table.Column<int>(type: "integer", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    MealType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_meal_logs_meals_MealId",
                        column: x => x.MealId,
                        principalTable: "meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_meal_logs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutSessionId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: false),
                    RestSeconds = table.Column<int>(type: "integer", nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: true),
                    Reps = table.Column<int>(type: "integer", nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    VideoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Tips = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exercises_workout_sessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "workout_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workout_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WorkoutSessionId = table.Column<int>(type: "integer", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    CaloriesBurned = table.Column<int>(type: "integer", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workout_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workout_logs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workout_logs_workout_sessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "workout_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exercises_WorkoutSessionId",
                table: "exercises",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_meal_foods_MealId",
                table: "meal_foods",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_meal_logs_MealId",
                table: "meal_logs",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_meal_logs_UserId_Date",
                table: "meal_logs",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_meal_plans_UserId",
                table: "meal_plans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_meals_MealPlanId",
                table: "meals",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_user_achievements_UserId",
                table: "user_achievements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_UserId",
                table: "user_profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_progress_UserId_Date",
                table: "user_progress",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workout_logs_UserId_Date",
                table: "workout_logs",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_workout_logs_WorkoutSessionId",
                table: "workout_logs",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_workout_plans_UserId",
                table: "workout_plans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_workout_sessions_WorkoutPlanId",
                table: "workout_sessions",
                column: "WorkoutPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exercises");

            migrationBuilder.DropTable(
                name: "meal_foods");

            migrationBuilder.DropTable(
                name: "meal_logs");

            migrationBuilder.DropTable(
                name: "user_achievements");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "user_progress");

            migrationBuilder.DropTable(
                name: "workout_logs");

            migrationBuilder.DropTable(
                name: "meals");

            migrationBuilder.DropTable(
                name: "workout_sessions");

            migrationBuilder.DropTable(
                name: "meal_plans");

            migrationBuilder.DropTable(
                name: "workout_plans");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
