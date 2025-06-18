using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymREST.Models.Classes
{
    public class PlanDto
    {
    public int Id { get; set; }
    public string? Title { get; set; }
    public List<ExerciseDto>? Exercises { get; set; }
    public string? Description { get; set; }
    public string? Goal { get; set; }
    public string? Level { get; set; }
    }
}