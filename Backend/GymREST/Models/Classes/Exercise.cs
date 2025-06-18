using System.Text.Json.Serialization;

namespace GymREST.Models.Classes
{
    public class Exercise
    {
        public int Id { get; init; }
        public string? Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string? Bodypart { get; set; }
        public string? Notes { get; set; }
        [JsonIgnore]
        public ICollection<PlanExerciseRel> PlanExerciseRels{ get; set; } = new List<PlanExerciseRel>();

        
    }
}