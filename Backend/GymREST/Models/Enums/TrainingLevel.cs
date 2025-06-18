using System.Text.Json.Serialization;

namespace GymREST.Models.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))] 
public enum TrainingLevel {
    Algaja,
    Kesktase,
    Edasijõudnud
}
