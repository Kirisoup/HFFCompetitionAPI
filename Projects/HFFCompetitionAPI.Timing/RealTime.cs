using System.Runtime.Serialization;

namespace HFFCompetitionAPI.Timing;

public readonly partial record struct RealTime(float TotalSeconds) : ITimeUnit<RealTime>
{
	public ulong Ticks => (ulong)(TotalSeconds * TimeSpan.TicksPerSecond);

	public int Hour => (int)(TotalSeconds / 3600);
	public int Minute => (int)(TotalSeconds / 60 % 60);
	public int Second => (int)(TotalSeconds % 60);

	public int Millisecond => (int)(Ticks / TimeSpan.TicksPerMillisecond % 1000);
	public int Microsecond => (int)(Ticks / (TimeSpan.TicksPerMillisecond / 1000) % 1000);
	public int Nanosecond => (int)(Ticks % (TimeSpan.TicksPerMillisecond / 1000) * 100);

	public RealTime Add(RealTime other) => new(TotalSeconds + other.TotalSeconds);
	public RealTime Sub(RealTime other) => new(TotalSeconds - other.TotalSeconds);
	public RealTime Mul(RealTime other) => new(TotalSeconds * other.TotalSeconds);
	public RealTime Div(RealTime other) => new(TotalSeconds / other.TotalSeconds);

	public override string ToString() => $"{nameof(RealTime)}({TotalSeconds})";
}

partial record struct RealTime : ISerializable
{
	public void GetObjectData(SerializationInfo info, StreamingContext context) {
		info.AddValue(nameof(TotalSeconds), TotalSeconds);
	}

	private RealTime(SerializationInfo info, StreamingContext context) : 
		this(info.GetUInt64(nameof(TotalSeconds))) {}
}

partial record struct RealTime : IComparable, IComparable<RealTime>
{
	public int CompareTo(RealTime other) => TotalSeconds.CompareTo(other.TotalSeconds);

	public int CompareTo(object? obj) {
		if (obj is null) return 1;
		if (obj is not RealTime other) 
			throw new ArgumentException($"Argument must be of type {nameof(RealTime)}");
		return TotalSeconds.CompareTo(other.TotalSeconds);
	}
}
