namespace HFFCompetitionAPI.Timing;

public interface ITime
{
	ulong Ticks { get; }
	int Hour { get; }
	int Minute { get; }
	int Second { get; }
	int Millisecond { get; }
	int Microsecond { get; }
	int Nanosecond { get; }
}
