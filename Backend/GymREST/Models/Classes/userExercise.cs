using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymREST.Models.Classes
{
    public class UserExercise
    {
       public int Id { get; init; }
        public string? Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string? Notes { get; set; }
        public DateTime DateTime { get; set; }
    }

}