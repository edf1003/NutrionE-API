namespace NutrionE.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public DietType DietType { get; set; }

        public string Name { get; set; }

        public int Age { get; set; } 

        public decimal Weight { get; set; }

        public decimal Height { get; set; }

        public Gender Gender { get; set; }

        public DateTimeOffset EnrollmentDate { get; set; }

        public decimal BasalMetabolism { get; set; }

        public DietObjective DietObjective { get; set; }
    }

    public enum DietType
    {
        Carnivore,
        Omnivore,
        Paleo,
        Vegan,
        Vegetarian,
        Mediterranean
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum DietObjective
    {
        GainWeight,
        LoseWeight,
        MaintainWeight
    }

}
