namespace HFFTournamentAPI.Timing;

public interface ITimeUnit : ITime;
public interface ITimeUnit<Self> : ITimeUnit,
	IEquatable<Self>,
	IComparable,
	IComparable<Self>
	where Self: ITimeUnit<Self>
{
	Self Add(Self other);
	Self Sub(Self other);
	Self Mul(Self other);
	Self Div(Self other);
}