using Microsoft.EntityFrameworkCore;
using QuickFit.API.Models.Entities;

namespace QuickFit.API.Data
{
    public class QuickFitDbContext : DbContext
    {
        public QuickFitDbContext(DbContextOptions<QuickFitDbContext> options) 
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealFood> MealFoods { get; set; }
        public DbSet<MealLog> MealLogs { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasDefaultValue("user");
            });

            // User Profile
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("user_profiles");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();
                
                entity.HasOne(e => e.User)
                    .WithOne(u => u.UserProfile)
                    .HasForeignKey<UserProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Workout Plan
            modelBuilder.Entity<WorkoutPlan>(entity =>
            {
                entity.ToTable("workout_plans");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.WorkoutPlans)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasMany(e => e.WorkoutSessions)
                    .WithOne(s => s.WorkoutPlan)
                    .HasForeignKey(s => s.WorkoutPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Workout Session
            modelBuilder.Entity<WorkoutSession>(entity =>
            {
                entity.ToTable("workout_sessions");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.WorkoutPlanId);
                
                entity.HasMany(e => e.Exercises)
                    .WithOne(ex => ex.WorkoutSession)
                    .HasForeignKey(ex => ex.WorkoutSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Exercise
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("exercises");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.WorkoutSessionId);
            });

            // Workout Log
            modelBuilder.Entity<WorkoutLog>(entity =>
            {
                entity.ToTable("workout_logs");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.Date });
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.WorkoutLogs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.WorkoutSession)
                    .WithMany(s => s.WorkoutLogs)
                    .HasForeignKey(e => e.WorkoutSessionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Meal Plan
            modelBuilder.Entity<MealPlan>(entity =>
            {
                entity.ToTable("meal_plans");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.MealPlans)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasMany(e => e.Meals)
                    .WithOne(m => m.MealPlan)
                    .HasForeignKey(m => m.MealPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Meal
            modelBuilder.Entity<Meal>(entity =>
            {
                entity.ToTable("meals");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.MealPlanId);
                
                entity.HasMany(e => e.MealFoods)
                    .WithOne(mf => mf.Meal)
                    .HasForeignKey(mf => mf.MealId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Meal Food
            modelBuilder.Entity<MealFood>(entity =>
            {
                entity.ToTable("meal_foods");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.MealId);
            });

            // Meal Log
            modelBuilder.Entity<MealLog>(entity =>
            {
                entity.ToTable("meal_logs");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.Date });
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.MealLogs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Meal)
                    .WithMany(m => m.MealLogs)
                    .HasForeignKey(e => e.MealId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // User Progress
            modelBuilder.Entity<UserProgress>(entity =>
            {
                entity.ToTable("user_progress");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.Date });
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.ProgressRecords)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // User Achievements
            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.ToTable("user_achievements");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Achievements)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}