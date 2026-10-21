namespace PodatkovniSloj;

/// <summary>One match in the attendance ranking: where it was, the crowd, and the two teams.</summary>
public sealed record AttendanceEntry(string Location, int Attendance, string HomeTeam, string AwayTeam);
