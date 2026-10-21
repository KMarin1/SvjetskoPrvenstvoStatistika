using System.Text.Json.Serialization;

namespace PodatkovniSloj.Models;

/// <summary>Every <c>type_of_event</c> value that occurs in the 2018 and 2019 data.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<TypeOfEvent>))]
public enum TypeOfEvent
{
    [JsonStringEnumMemberName("goal")]
    Goal,

    [JsonStringEnumMemberName("goal-penalty")]
    GoalPenalty,

    [JsonStringEnumMemberName("goal-own")]
    GoalOwn,

    [JsonStringEnumMemberName("yellow-card")]
    YellowCard,

    [JsonStringEnumMemberName("yellow-card-second")]
    YellowCardSecond,

    [JsonStringEnumMemberName("red-card")]
    RedCard,

    [JsonStringEnumMemberName("substitution-in")]
    SubstitutionIn,

    [JsonStringEnumMemberName("substitution-out")]
    SubstitutionOut,
}
