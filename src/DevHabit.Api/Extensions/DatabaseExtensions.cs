using DevHabit.Api.Database;
using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Extensions;

internal static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }

    public static async Task SeedDataAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await dbContext.Habits.AnyAsync())
        {
            app.Logger.LogInformation("Database already seeded. Skipping.");
            return;
        }

        app.Logger.LogInformation("Seeding database...");

        var habitsToSeed = new List<Habit>
        {
            new()
            {
                Id = "habit_1",
                Name = "Read for 15 minutes",
                Description = "Read any book for at least 15 minutes a day.",
                Type = HabitType.Binary,
                Target = new Target
                {
                    Value = 3,
                    Unit = "hours"
                },
                Frequency = new Frequency { Type = FrequencyType.Daily, TimesPerPeriod = 1 },
                Status = HabitStatus.Ongoing,
                IsArchived = false,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = "habit_2",
                Name = "Run",
                Description = "Go for a run.",
                Type = HabitType.Measurable,
                Frequency = new Frequency { Type = FrequencyType.Weekly, TimesPerPeriod = 3 },
                Target = new Target { Value = 5, Unit = "km" },
                Status = HabitStatus.Ongoing,
                IsArchived = false,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        await dbContext.Habits.AddRangeAsync(habitsToSeed);
        await dbContext.SaveChangesAsync();

        app.Logger.LogInformation("Database seeded successfully.");
    }
}
