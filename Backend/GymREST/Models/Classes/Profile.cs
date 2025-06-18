using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymREST.Models.Classes
{
    public class Profile
    {
        public int Id { get; set;}
        public int UserId { get; set; }
        public int InitialDataId { get; set; }
        public InitialData? InitialData{ get; set; }
    }
}