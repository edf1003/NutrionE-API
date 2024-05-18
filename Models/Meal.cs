namespace NutrionE.Models
{
    public class Meal
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string FirstPlate { get; set; }

        public string SecondPlate { get; set; }

        public string Dessert { get; set; }

        public long DailyDietId { get; set; }

        public DailyDiet DailyDiet { get; set; }
    }
}
