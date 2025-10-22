using System;

namespace Unions
{
	// see: https://github.com/dotnet/csharplang/blob/main/proposals/unions.md for the feature proposal on C# unions.
	// based on that proposal, this is my take on discriminated unions in C# aligns with the proposal and can be used today.
	// another design goals is to keep it out of the way of normal C# programming, so minimal impact on existing codebases,
	// and no god objects.

	// here is an example from the proposal:
	// public union Pet(Cat, Dog, Bird);
	// this could be represented as:
	// public Union<Cat, Dog, Bird> Pet;

	/// <summary>
	/// Represents a union type that can hold a value of either <typeparamref name="T1"/> or <typeparamref name="T2"/>.
	/// This allows a single variable to store one of multiple types, similar to discriminated unions in functional languages.
	/// </summary>
	/// <typeparam name="T1">The first possible type that the union can hold.</typeparam>
	/// <typeparam name="T2">The second possible type that the union can hold.</typeparam>
	public readonly struct Union<T1, T2>
	{
		private readonly object? _value;
		private readonly int _tag; // 1 = T1, 2 = T2

		/// <summary>
		/// Gets the underlying value stored in the union. This may be <c>null</c> if the stored value is null.
		/// </summary>
		public object? Value => _value;

		/// <summary>
		/// Initializes a new <see cref="Union{T1,T2}"/> containing a <typeparamref name="T1"/> value.
		/// </summary>
		/// <param name="value">The value of type <typeparamref name="T1"/> to store in the union.</param>
		public Union(T1 value) { _value = value; _tag = 1; }

		/// <summary>
		/// Initializes a new <see cref="Union{T1,T2}"/> containing a <typeparamref name="T2"/> value.
		/// </summary>
		/// <param name="value">The value of type <typeparamref name="T2"/> to store in the union.</param>
		public Union(T2 value) { _value = value; _tag = 2; }

		/// <summary>
		/// Implicitly converts a value of type <typeparamref name="T1"/> into a <see cref="Union{T1,T2}"/>.
		/// </summary>
		/// <param name="value">The <typeparamref name="T1"/> value to convert.</param>
		public static implicit operator Union<T1, T2>(T1 value) => new Union<T1, T2>(value);

		/// <summary>
		/// Implicitly converts a value of type <typeparamref name="T2"/> into a <see cref="Union{T1,T2}"/>.
		/// </summary>
		/// <param name="value">The <typeparamref name="T2"/> value to convert.</param>
		public static implicit operator Union<T1, T2>(T2 value) => new Union<T1, T2>(value);

		/// <summary>
		/// Attempts to extract the stored value as <typeparamref name="T1"/>.
		/// </summary>
		/// <param name="value">
		/// When this method returns, contains the value of type <typeparamref name="T1"/> if present; otherwise, the default value of <typeparamref name="T1"/>.
		/// </param>
		/// <returns><c>true</c> if the union currently stores a <typeparamref name="T1"/> value; otherwise, <c>false</c>.</returns>
		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }

		/// <summary>
		/// Attempts to extract the stored value as <typeparamref name="T2"/>.
		/// </summary>
		/// <param name="value">
		/// When this method returns, contains the value of type <typeparamref name="T2"/> if present; otherwise, the default value of <typeparamref name="T2"/>.
		/// </param>
		/// <returns><c>true</c> if the union currently stores a <typeparamref name="T2"/> value; otherwise, <c>false</c>.</returns>
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }

		/// <summary>
		/// Returns a string representation of the current value stored in the union.
		/// </summary>
		/// <returns>The string representation of the stored value, or "null" if the value is null.</returns>
		public override string ToString() => _value?.ToString() ?? "null";
	}

	/// <summary>
	/// Represents a union type that can hold a value of <typeparamref name="T1"/>, <typeparamref name="T2"/>, or <typeparamref name="T3"/>.
	/// This allows a single variable to store one of three types, similar to discriminated unions in functional programming.
	/// </summary>
	/// <typeparam name="T1">The first possible type that the union can hold.</typeparam>
	/// <typeparam name="T2">The second possible type that the union can hold.</typeparam>
	/// <typeparam name="T3">The third possible type that the union can hold.</typeparam>
	public readonly struct Union<T1, T2, T3>
	{
		private readonly object? _value;
		private readonly int _tag;

		/// <summary>
		/// Gets the underlying value stored in the union. Can be <c>null</c>.
		/// </summary>
		public object? Value => _value;

		/// <summary>Initializes a new union containing a <typeparamref name="T1"/> value.</summary>
		public Union(T1 value) { _value = value; _tag = 1; }

		/// <summary>Initializes a new union containing a <typeparamref name="T2"/> value.</summary>
		public Union(T2 value) { _value = value; _tag = 2; }

		/// <summary>Initializes a new union containing a <typeparamref name="T3"/> value.</summary>
		public Union(T3 value) { _value = value; _tag = 3; }

		/// <summary>Implicitly converts a <typeparamref name="T1"/> value to this union.</summary>
		public static implicit operator Union<T1, T2, T3>(T1 value) => new Union<T1, T2, T3>(value);

		/// <summary>Implicitly converts a <typeparamref name="T2"/> value to this union.</summary>
		public static implicit operator Union<T1, T2, T3>(T2 value) => new Union<T1, T2, T3>(value);

		/// <summary>Implicitly converts a <typeparamref name="T3"/> value to this union.</summary>
		public static implicit operator Union<T1, T2, T3>(T3 value) => new Union<T1, T2, T3>(value);

		/// <summary>Attempts to extract the value as <typeparamref name="T1"/>.</summary>
		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }

		/// <summary>Attempts to extract the value as <typeparamref name="T2"/>.</summary>
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }

		/// <summary>Attempts to extract the value as <typeparamref name="T3"/>.</summary>
		public bool TryGet(out T3 value) { if (_tag == 3) { value = (T3)_value!; return true; } value = default!; return false; }

		/// <summary>Returns a string representation of the stored value, or "null" if the value is null.</summary>
		public override string ToString() => _value?.ToString() ?? "null";
	}

	/// <summary>
	/// Represents a union type that can hold a value of <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, or <typeparamref name="T4"/>.
	/// Useful when a single variable may store one of four possible types.
	/// </summary>
	/// <typeparam name="T1">The first possible type.</typeparam>
	/// <typeparam name="T2">The second possible type.</typeparam>
	/// <typeparam name="T3">The third possible type.</typeparam>
	/// <typeparam name="T4">The fourth possible type.</typeparam>
	public readonly struct Union<T1, T2, T3, T4>
	{
		private readonly object? _value;
		private readonly int _tag;
		public object? Value => _value;

		public Union(T1 value) { _value = value; _tag = 1; }
		public Union(T2 value) { _value = value; _tag = 2; }
		public Union(T3 value) { _value = value; _tag = 3; }
		public Union(T4 value) { _value = value; _tag = 4; }

		public static implicit operator Union<T1, T2, T3, T4>(T1 value) => new Union<T1, T2, T3, T4>(value);
		public static implicit operator Union<T1, T2, T3, T4>(T2 value) => new Union<T1, T2, T3, T4>(value);
		public static implicit operator Union<T1, T2, T3, T4>(T3 value) => new Union<T1, T2, T3, T4>(value);
		public static implicit operator Union<T1, T2, T3, T4>(T4 value) => new Union<T1, T2, T3, T4>(value);

		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T3 value) { if (_tag == 3) { value = (T3)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T4 value) { if (_tag == 4) { value = (T4)_value!; return true; } value = default!; return false; }

		public override string ToString() => _value?.ToString() ?? "null";
	}


	/// <summary>
	/// Represents a union type that can hold a value of one of five possible types: <typeparamref name="T1"/>.. <typeparamref name="T5"/>.
	/// Useful when a variable may contain multiple possible types in a type-safe way.
	/// </summary>
	/// <typeparam name="T1">The first possible type.</typeparam>
	/// <typeparam name="T2">The second possible type.</typeparam>
	/// <typeparam name="T3">The third possible type.</typeparam>
	/// <typeparam name="T4">The fourth possible type.</typeparam>
	/// <typeparam name="T5">The fifth possible type.</typeparam>
	public readonly struct Union<T1, T2, T3, T4, T5>
	{
		private readonly object? _value;
		private readonly int _tag;
		public object? Value => _value;

		public Union(T1 value) { _value = value; _tag = 1; }
		public Union(T2 value) { _value = value; _tag = 2; }
		public Union(T3 value) { _value = value; _tag = 3; }
		public Union(T4 value) { _value = value; _tag = 4; }
		public Union(T5 value) { _value = value; _tag = 5; }

		public static implicit operator Union<T1, T2, T3, T4, T5>(T1 value) => new Union<T1, T2, T3, T4, T5>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5>(T2 value) => new Union<T1, T2, T3, T4, T5>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5>(T3 value) => new Union<T1, T2, T3, T4, T5>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5>(T4 value) => new Union<T1, T2, T3, T4, T5>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5>(T5 value) => new Union<T1, T2, T3, T4, T5>(value);

		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T3 value) { if (_tag == 3) { value = (T3)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T4 value) { if (_tag == 4) { value = (T4)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T5 value) { if (_tag == 5) { value = (T5)_value!; return true; } value = default!; return false; }

		public override string ToString() => _value?.ToString() ?? "null";
	}

	/// <summary>
	/// Represents a union type that can hold a value of one of six possible types: <typeparamref name="T1"/>.. <typeparamref name="T6"/>.
	/// Allows a single variable to store one of six types safely.
	/// </summary>
	/// <typeparam name="T1">The first possible type.</typeparam>
	/// <typeparam name="T2">The second possible type.</typeparam>
	/// <typeparam name="T3">The third possible type.</typeparam>
	/// <typeparam name="T4">The fourth possible type.</typeparam>
	/// <typeparam name="T5">The fifth possible type.</typeparam>
	/// <typeparam name="T6">The sixth possible type.</typeparam>
	public readonly struct Union<T1, T2, T3, T4, T5, T6>
	{
		private readonly object? _value;
		private readonly int _tag;
		public object? Value => _value;

		public Union(T1 value) { _value = value; _tag = 1; }
		public Union(T2 value) { _value = value; _tag = 2; }
		public Union(T3 value) { _value = value; _tag = 3; }
		public Union(T4 value) { _value = value; _tag = 4; }
		public Union(T5 value) { _value = value; _tag = 5; }
		public Union(T6 value) { _value = value; _tag = 6; }

		public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T1 value) => new Union<T1, T2, T3, T4, T5, T6>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T2 value) => new Union<T1, T2, T3, T4, T5, T6>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T3 value) => new Union<T1, T2, T3, T4, T5, T6>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T4 value) => new Union<T1, T2, T3, T4, T5, T6>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T5 value) => new Union<T1, T2, T3, T4, T5, T6>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T6 value) => new Union<T1, T2, T3, T4, T5, T6>(value);

		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T3 value) { if (_tag == 3) { value = (T3)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T4 value) { if (_tag == 4) { value = (T4)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T5 value) { if (_tag == 5) { value = (T5)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T6 value) { if (_tag == 6) { value = (T6)_value!; return true; } value = default!; return false; }

		public override string ToString() => _value?.ToString() ?? "null";
	}

	/// <summary>
	/// Represents a union type that can hold a value of one of seven possible types: <typeparamref name="T1"/>.. <typeparamref name="T7"/>.
	/// Allows a single variable to store one of seven types safely.
	/// </summary>
	/// <typeparam name="T1">The first possible type.</typeparam>
	/// <typeparam name="T2">The second possible type.</typeparam>
	/// <typeparam name="T3">The third possible type.</typeparam>
	/// <typeparam name="T4">The fourth possible type.</typeparam>
	/// <typeparam name="T5">The fifth possible type.</typeparam>
	/// <typeparam name="T6">The sixth possible type.</typeparam>
	/// <typeparam name="T7">The seventh possible type.</typeparam>
	public readonly struct Union<T1, T2, T3, T4, T5, T6, T7>
	{
		private readonly object? _value;
		private readonly int _tag;
		public object? Value => _value;

		public Union(T1 value) { _value = value; _tag = 1; }
		public Union(T2 value) { _value = value; _tag = 2; }
		public Union(T3 value) { _value = value; _tag = 3; }
		public Union(T4 value) { _value = value; _tag = 4; }
		public Union(T5 value) { _value = value; _tag = 5; }
		public Union(T6 value) { _value = value; _tag = 6; }
		public Union(T7 value) { _value = value; _tag = 7; }

		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T1 value) => new Union<T1, T2, T3, T4, T5, T6, T7>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T2 value) => new Union<T1, T2, T3, T4, T5, T6, T7>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T3 value) => new Union<T1, T2, T3, T4, T5, T6, T7>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T4 value) => new Union<T1, T2, T3, T4, T5, T6, T7>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T5 value) => new Union<T1, T2, T3, T4, T5, T6, T7>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T6 value) => new Union<T1, T2, T3, T4, T5, T6, T7>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T7 value) => new Union<T1, T2, T3, T4, T5, T6, T7>(value);

		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T3 value) { if (_tag == 3) { value = (T3)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T4 value) { if (_tag == 4) { value = (T4)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T5 value) { if (_tag == 5) { value = (T5)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T6 value) { if (_tag == 6) { value = (T6)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T7 value) { if (_tag == 7) { value = (T7)_value!; return true; } value = default!; return false; }

		public override string ToString() => _value?.ToString() ?? "null";
	}

	/// <summary>
	/// Represents a union type that can hold a value of one of eight possible types: <typeparamref name="T1"/>.. <typeparamref name="T8"/>.
	/// Allows a single variable to store one of eight types safely.
	/// </summary>
	/// <typeparam name="T1">The first possible type.</typeparam>
	/// <typeparam name="T2">The second possible type.</typeparam>
	/// <typeparam name="T3">The third possible type.</typeparam>
	/// <typeparam name="T4">The fourth possible type.</typeparam>
	/// <typeparam name="T5">The fifth possible type.</typeparam>
	/// <typeparam name="T6">The sixth possible type.</typeparam>
	/// <typeparam name="T7">The seventh possible type.</typeparam>
	/// <typeparam name="T8">The eighth possible type.</typeparam>
	public readonly struct Union<T1, T2, T3, T4, T5, T6, T7, T8>
	{
		private readonly object? _value;
		private readonly int _tag;
		public object? Value => _value;

		public Union(T1 value) { _value = value; _tag = 1; }
		public Union(T2 value) { _value = value; _tag = 2; }
		public Union(T3 value) { _value = value; _tag = 3; }
		public Union(T4 value) { _value = value; _tag = 4; }
		public Union(T5 value) { _value = value; _tag = 5; }
		public Union(T6 value) { _value = value; _tag = 6; }
		public Union(T7 value) { _value = value; _tag = 7; }
		public Union(T8 value) { _value = value; _tag = 8; }

		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T2 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T3 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T4 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T5 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T6 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T7 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T8 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8>(value);

		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T3 value) { if (_tag == 3) { value = (T3)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T4 value) { if (_tag == 4) { value = (T4)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T5 value) { if (_tag == 5) { value = (T5)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T6 value) { if (_tag == 6) { value = (T6)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T7 value) { if (_tag == 7) { value = (T7)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T8 value) { if (_tag == 8) { value = (T8)_value!; return true; } value = default!; return false; }

		public override string ToString() => _value?.ToString() ?? "null";
	}

	/// <summary>
	/// Represents a union type that can hold a value of one of nine possible types: <typeparamref name="T1"/>.. <typeparamref name="T9"/>.
	/// Allows a single variable to store one of nine types safely.
	/// </summary>
	/// <typeparam name="T1">The first possible type.</typeparam>
	/// <typeparam name="T2">The second possible type.</typeparam>
	/// <typeparam name="T3">The third possible type.</typeparam>
	/// <typeparam name="T4">The fourth possible type.</typeparam>
	/// <typeparam name="T5">The fifth possible type.</typeparam>
	/// <typeparam name="T6">The sixth possible type.</typeparam>
	/// <typeparam name="T7">The seventh possible type.</typeparam>
	/// <typeparam name="T8">The eighth possible type.</typeparam>
	/// <typeparam name="T9">The ninth possible type.</typeparam>
	public readonly struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		private readonly object? _value;
		private readonly int _tag;
		public object? Value => _value;

		public Union(T1 value) { _value = value; _tag = 1; }
		public Union(T2 value) { _value = value; _tag = 2; }
		public Union(T3 value) { _value = value; _tag = 3; }
		public Union(T4 value) { _value = value; _tag = 4; }
		public Union(T5 value) { _value = value; _tag = 5; }
		public Union(T6 value) { _value = value; _tag = 6; }
		public Union(T7 value) { _value = value; _tag = 7; }
		public Union(T8 value) { _value = value; _tag = 8; }
		public Union(T9 value) { _value = value; _tag = 9; }

		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T2 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T3 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T4 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T5 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T6 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T7 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T8 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
		public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T9 value) => new Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);

		public bool TryGet(out T1 value) { if (_tag == 1) { value = (T1)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T2 value) { if (_tag == 2) { value = (T2)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T3 value) { if (_tag == 3) { value = (T3)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T4 value) { if (_tag == 4) { value = (T4)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T5 value) { if (_tag == 5) { value = (T5)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T6 value) { if (_tag == 6) { value = (T6)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T7 value) { if (_tag == 7) { value = (T7)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T8 value) { if (_tag == 8) { value = (T8)_value!; return true; } value = default!; return false; }
		public bool TryGet(out T9 value) { if (_tag == 9) { value = (T9)_value!; return true; } value = default!; return false; }

		public override string ToString() => _value?.ToString() ?? "null";
	}

}
