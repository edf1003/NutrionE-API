namespace NutrionE.Models.DTOs.User
{
    public class UserDTO
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
}
