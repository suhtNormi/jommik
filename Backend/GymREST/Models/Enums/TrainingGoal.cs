using System.Text.Json.Serialization;

namespace GymREST.Models.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TrainingGoal {
    Kaalulangetus,
    Lihaskasv,
    Vastupidavus,
    Ülakeha
}
