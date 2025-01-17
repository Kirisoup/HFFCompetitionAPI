namespace HFFCompetitionAPI.Timing;

using System.Runtime.Serialization;

/// <summary>
/// Represents a certain span of time.
/// </summary>
/// <typeparam name="TTime">The timing unit that the TimeStamp is associated with</typeparam>
public readonly partial struct TimeSpan<TTime> 
	where TTime : struct, ITimeUnit<TTime>
{
	public TTime Duration { get; }

	public TimeSpan(TTime duration) {
		Duration = duration;
	}

	public TimeSpan(TTime since, TTime until) {
		ITimeUnit duration = (since, until) switch {
			(GameTime gtSince, GameTime gtUntil) => new GameTime(
				gtUntil.Cycles - gtSince.Cycles),
			(RealTime rtSince, RealTime rtUntil) => new RealTime(
				rtUntil.TotalSeconds - rtSince.TotalSeconds),
			_ => throw new NotImplementedException(
				$"A {nameof(TimeSpan<TTime>)} with type {typeof(TTime)} is not supported")
		};
		Duration = (TTime)duration;
	}
}

// arithmetics
partial struct TimeSpan<TTime>
{
	public static TimeSpan<TTime> operator +(TimeSpan<TTime> x, TimeSpan<TTime> y) => 
		new(x.Duration.Add(y.Duration));

	public static TimeSpan<TTime> operator -(TimeSpan<TTime> x, TimeSpan<TTime> y) => 
		new(x.Duration.Sub(y.Duration));

	public static TimeSpan<TTime> operator *(TimeSpan<TTime> x, TimeSpan<TTime> y) => 
		new(x.Duration.Mul(y.Duration));

	public static TimeSpan<TTime> operator /(TimeSpan<TTime> x, TimeSpan<TTime> y) => 
		new(x.Duration.Div(y.Duration));
}

// serialization
partial struct TimeSpan<TTime> : ISerializable
{
	public void GetObjectData(SerializationInfo info, StreamingContext context) {
		info.AddValue(nameof(Duration), Duration);
	}

	private TimeSpan(SerializationInfo info, StreamingContext context) : this() {
		Duration = (TTime)info.GetValue(nameof(Duration), typeof(TTime));
	}
}

// comparison
partial struct TimeSpan<TTime> : IComparable, IComparable<TimeSpan<TTime>>
{
	public int CompareTo(TimeSpan<TTime> other) => Duration.CompareTo(other.Duration);

	public int CompareTo(object obj) => obj switch {
		null => 1,
		TimeSpan<TTime> other => Duration.CompareTo(other.Duration),
		_ => throw new ArgumentException($"Argument must be of type {nameof(TimeSpan<TTime>)}")
	};

	public static bool operator >(TimeSpan<TTime> x, TimeSpan<TTime> y) => x.CompareTo(y) > 0;
	public static bool operator <(TimeSpan<TTime> x, TimeSpan<TTime> y) => x.CompareTo(y) < 0;
	public static bool operator >=(TimeSpan<TTime> x, TimeSpan<TTime> y) => x.CompareTo(y) >= 0;
	public static bool operator <=(TimeSpan<TTime> x, TimeSpan<TTime> y) => x.CompareTo(y) <= 0;
}

// equality
partial struct TimeSpan<TTime> : IEquatable<TimeSpan<TTime>>
{
	public bool Equals(TimeSpan<TTime> other) => Duration.Equals(other.Duration);
	
	public override bool Equals(object obj) => 
		obj is TimeSpan<TTime> other && Duration.Equals(other.Duration);
	
	public override int GetHashCode() => Duration.GetHashCode();

	public static bool operator ==(TimeSpan<TTime> x, TimeSpan<TTime> y) => x.Equals(y);
	public static bool operator !=(TimeSpan<TTime> x, TimeSpan<TTime> y) => x.Equals(y);
}