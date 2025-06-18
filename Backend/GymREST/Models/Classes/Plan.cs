namespace GymREST.Models.Classes
{
    public class Plan
    {
        public int Id { get; init; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Goal { get; set; }
        public string? Level { get; set; }

        public ICollection<PlanExerciseRel> PlanExerciseRels{ get; set; } = new List<PlanExerciseRel>();
    }
}