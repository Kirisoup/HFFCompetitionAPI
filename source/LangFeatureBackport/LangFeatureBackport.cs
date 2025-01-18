#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
	using System.ComponentModel;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class IsExternalInit;
}
#endif

#if !NET7_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	internal sealed class RequiredMemberAttribute : Attribute;

	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute
	{
		public string FeatureName { get; } = featureName;
		public bool IsOptional { get; init; }

		public const string RefStructs = nameof(RefStructs);
		public const string RequiredMembers = nameof(RequiredMembers);
	}
}
#endif

#if !NET7_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	internal sealed class SetsRequiredMembersAttribute : Attribute;
}
#endif

#if !NET5_0_OR_GREATER
namespace System
{
	using System.Runtime.CompilerServices;

	public readonly struct Index : IEquatable<Index>
	{
		private readonly int _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Index(int value, bool fromEnd = false) {
			if (value < 0) throw new ArgumentOutOfRangeException(
				nameof(value), "value must be non-negative");
			if (fromEnd) _value = ~value;
			else _value = value;
		}

		private Index(int value) => _value = value;

		public static Index Start => new(0);

		public static Index End => new(~0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Index FromStart(int value) => value >= 0 ?
			new(value) :
			throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Index FromEnd(int value) => value >= 0 ?
			new(~value) :
			throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative");

		public int Value => _value >= 0 ? _value : ~_value;

		public bool IsFromEnd => _value < 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetOffset(int length) => !IsFromEnd ? _value : _value + length + 1;

		public override bool Equals(object? value) => value is Index index && _value == index._value;

		public bool Equals(Index other) => _value == other._value;

		public override int GetHashCode() => _value;

		public static implicit operator Index(int value) => FromStart(value);

		public override string ToString() => (IsFromEnd ? "^" : "") + ((uint)Value).ToString();
	}

	public readonly struct Range(Index start, Index end) : IEquatable<Range>
	{
		public Index Start { get; } = start;

		public Index End { get; } = end;

		public override bool Equals(object? value) => value is Range r &&
			r.Start.Equals(Start) && r.End.Equals(End);

		public bool Equals(Range other) => other.Start.Equals(Start) && other.End.Equals(End);

		public override int GetHashCode() => Start.GetHashCode() * 31 + End.GetHashCode();

		public override string ToString() => $"{Start}..{End}";

		public static Range StartAt(Index start) => new(start, Index.End);

		public static Range EndAt(Index end) => new(Index.Start, end);

		public static Range All => new(Index.Start, Index.End);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public (int Offset, int Length) GetOffsetAndLength(int length)
		{
			(int start, int end) = (
				!Start.IsFromEnd ? Start.Value : length - Start.Value,
				!End.IsFromEnd ? End.Value : length - End.Value
			);
			return (uint)end <= (uint)length && (uint)start <= (uint)end ?
				(start, end - start) :
				throw new ArgumentOutOfRangeException(nameof(length));
		}
	}
}
namespace System.Runtime.CompilerServices
{
	internal static class RuntimeHelpers
	{
		public static T[] GetSubArray<T>(T[] array, Range range) {
			if (array is null) throw new ArgumentNullException(nameof(array));

			(int offset, int length) = range.GetOffsetAndLength(array.Length);

			if (default(T) is not null || typeof(T[]) == array.GetType()) {
				if (length is 0) return [];

				var dest = new T[length];
				Array.Copy(array, offset, dest, 0, length);
				return dest;
			} else {
				var dest = (T[])Array.CreateInstance(array.GetType().GetElementType(), length);
				Array.Copy(array, offset, dest, 0, length);
				return dest;
			}
		}
	}
}
#endif