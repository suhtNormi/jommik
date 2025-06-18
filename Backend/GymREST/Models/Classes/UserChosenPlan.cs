using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymREST.Models.Classes
{
    public record UserChosenPlan
    {
        public int Id { get; set;}
        public int UserId { get; set; }
        public User? User { get; set; }
        public int PlanId { get; set; }
        public Plan? Plan{ get; set; }
    }
}