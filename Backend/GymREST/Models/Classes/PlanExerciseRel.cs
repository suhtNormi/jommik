namespace GymREST.Models.Classes
{
    public class PlanExerciseRel
    {
        public int Id { get; init; }
        public int PlanId { get; set; }
        public Plan? Plan { get; set; }
        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
    }
}