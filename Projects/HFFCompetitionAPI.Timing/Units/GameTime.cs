using System.Runtime.Serialization;

namespace HFFCompetitionAPI.Timing;

public readonly partial record struct GameTime(ulong Cycles) : ITimeUnit<GameTime>
{
	public ulong Ticks => (ulong)(Cycles * Time.fixedDeltaTime * TimeSpan.TicksPerSecond);

	public int Hour => (int)(Cycles * Time.fixedDeltaTime / 3600);
	public int Minute =>(int)(Cycles * Time.fixedDeltaTime / 60 % 60);
	public int Second => (int)(Cycles * Time.fixedDeltaTime % 60);

	public int Millisecond => (int)(Ticks / TimeSpan.TicksPerMillisecond % 1000);
	public int Microsecond => (int)(Ticks / (TimeSpan.TicksPerMillisecond / 1000) % 1000);
	public int Nanosecond => (int)(Ticks % (TimeSpan.TicksPerMillisecond / 1000) * 100);

	public GameTime Add(GameTime other) => new(Cycles + other.Cycles);
	public GameTime Sub(GameTime other) => new(Cycles - other.Cycles);
	public GameTime Mul(GameTime other) => new(Cycles * other.Cycles);
	public GameTime Div(GameTime other) => new(Cycles / other.Cycles);

	public override string ToString() => $"{nameof(GameTime)}({Cycles})";
}

partial record struct GameTime : ISerializable
{
	public void GetObjectData(SerializationInfo info, StreamingContext context) {
		info.AddValue(nameof(Cycles), Cycles);
	}

	private GameTime(SerializationInfo info, StreamingContext context) : 
		this(info.GetUInt64(nameof(Cycles))) {}
}

partial record struct GameTime : IComparable, IComparable<GameTime>
{
	public int CompareTo(GameTime other) => Cycles.CompareTo(other.Cycles);

	public int CompareTo(object? obj) {
		if (obj is null) return 1;
		if (obj is not GameTime other) 
			throw new ArgumentException($"Argument must be of type {nameof(GameTime)}");
		return Cycles.CompareTo(other.Cycles);
	}
}