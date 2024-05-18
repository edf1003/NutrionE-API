namespace NutrionE.Models
{
    public class DailyDiet
    {
        public long Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public DietType DietType { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public List<Meal> Meals { get; set; }
    }
}
