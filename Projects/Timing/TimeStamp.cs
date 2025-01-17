using System.Runtime.Serialization;

namespace HFFTournamentAPI.Timing;

/// <summary>
/// Representing a point in time, associated with the system time of its creation.
/// </summary>
/// <typeparam name="TTime">The timing unit that the TimeStamp is associated with</typeparam>
public readonly partial struct TimeStamp<TTime>
	where TTime: struct, ITimeUnit<TTime>
{
	// always represent the time of its creation
	public TimeStamp() {
		SysTime = DateTime.UtcNow;
		ITimeUnit instant = typeof(TTime) switch {
			var t when t == typeof(GameTime) => new GameTime(
				GameCycleCounter.Instance.Cycles),
			var t when t == typeof(RealTime) => new RealTime(
				Time.unscaledTime),
			_ => throw new NotImplementedException(
				$"A {nameof(TimeStamp<TTime>)} with type {typeof(TTime)} is not supported")
		};
		Instant = (TTime)instant;
	}

	public DateTime SysTime { get; }
	public TTime Instant { get; }

	public override string ToString() => 
		$"{nameof(TimeStamp<TTime>)} {{ {nameof(Instant)}: {Instant}, {nameof(SysTime)}: {SysTime} }}";
}

// serialization
partial struct TimeStamp<TTime> : ISerializable
{
	public void GetObjectData(SerializationInfo info, StreamingContext context) {
		info.AddValue(nameof(Instant), Instant);
		info.AddValue(nameof(SysTime), SysTime);
	}

	private TimeStamp(SerializationInfo info, StreamingContext context) : this() {
		Instant = (TTime)info.GetValue(nameof(Instant), typeof(TTime));
		SysTime = info.GetDateTime(nameof(SysTime));
	}
}

// comparison
partial struct TimeStamp<TTime> : IComparable, IComparable<TimeStamp<TTime>>
{
	public int CompareTo(TimeStamp<TTime> other) => Instant.CompareTo(other.Instant);

	public int CompareTo(object? obj) => obj switch {
		null => 1,
		TimeStamp<TTime> other => Instant.CompareTo(other.Instant),
		_ => throw new ArgumentException($"Argument must be of type {nameof(TimeStamp<TTime>)}")
	};

	public static bool operator >(TimeStamp<TTime> x, TimeStamp<TTime> y) => x.CompareTo(y) > 0;
	public static bool operator <(TimeStamp<TTime> x, TimeStamp<TTime> y) => x.CompareTo(y) < 0;
	public static bool operator >=(TimeStamp<TTime> x, TimeStamp<TTime> y) => x.CompareTo(y) >= 0;
	public static bool operator <=(TimeStamp<TTime> x, TimeStamp<TTime> y) => x.CompareTo(y) <= 0;
}

// equality
partial struct TimeStamp<TTime> : IEquatable<TimeStamp<TTime>>
{
	public bool Equals(TimeStamp<TTime> other) => Instant.Equals(other.Instant);
	
	public override bool Equals(object obj) => 
		obj is TimeStamp<TTime> other && Instant.Equals(other.Instant);
	
	public override int GetHashCode() => Instant.GetHashCode();

	public static bool operator ==(TimeStamp<TTime> x, TimeStamp<TTime> y) => x.Equals(y);
	public static bool operator !=(TimeStamp<TTime> x, TimeStamp<TTime> y) => x.Equals(y);
}