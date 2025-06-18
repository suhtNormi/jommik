using GymREST.Models.Enums;

namespace GymREST.Models.Classes;
public class InitialData {
    public int Id { get; set;}
    public int UserId { get; set;}
    // public User? User{ get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Gender { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public TrainingGoal Goal { get; set; }
    public int Frequency { get; set; }
    public TrainingLevel Level { get; set; }
    public string? ProfileImageUrl { get; set; }
}

