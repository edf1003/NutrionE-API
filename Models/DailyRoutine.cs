namespace NutrionE.Models
{
    public class DailyRoutine
    {
        public long Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public ExerciseType ExerciseType { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public List<Exercise> Exercises { get; set; }
    }
}
