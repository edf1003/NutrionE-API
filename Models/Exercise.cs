namespace NutrionE.Models
{
    public class Exercise
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long DailyRoutineId { get; set; }
    }

    public enum ExerciseType
    {
        Leg,
        Gluteus,
        Back,
        Chest,
        Arm,
        Cardio
    }

}
