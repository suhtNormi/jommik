using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymREST.Models.Classes
{
    public class WorkoutEvent
    {
        public int Id { get; set;}
        public int UserId { get; set;}
        public string? Name { get; set;}
        public List<CalendarExercise> CalendarExercises { get; set;}
        // public int CalendarExerciseId { get; set;}
        public string? Start { get; set;}
        public string? End { get; set;}
    }
}