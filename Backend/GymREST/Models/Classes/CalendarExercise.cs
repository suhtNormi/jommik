using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymREST.Models.Classes
{
    public class CalendarExercise
    {
        public int Id { get; set;}
        public int UserId { get; set;}
        public string? Name { get; set;}
        public int Sets { get; set;}
        public int Reps { get; set;}
        public string? Bodypart { get; set;}
        public string? DateAdded { get; set;}
    }
}