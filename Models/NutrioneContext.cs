using Microsoft.EntityFrameworkCore;

namespace NutrionE.Models
{
    public class NutrioneContext : DbContext
    {
        public NutrioneContext(DbContextOptions options) : base(options) {}

        public DbSet<Meal> Meals { get; set; }

        public DbSet<DailyDiet> DailyDiets { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<DailyRoutine> DailyRoutines { get; set;}

        public DbSet<Exercise> Exercises { get; set; }
    }
}
