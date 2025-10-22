using FluentAssertions;

namespace Unions.Tests;

public class Union8Tests
{
	[Fact]
	public void union8_store_T1_T2_T8()
	{
		long lng = 123456789L;

		var u1 = new Union<int, string, double, bool, DateTime, Guid, decimal, long>(42);
		u1.TryGet(out int i1).Should().BeTrue();
		i1.Should().Be(42);
		u1.ToString().Should().Be("42");

		var u2 = new Union<int, string, double, bool, DateTime, Guid, decimal, long>("Hello");
		u2.TryGet(out string? s2).Should().BeTrue();
		s2.Should().Be("Hello");
		u2.ToString().Should().Be("Hello");

		var u8 = new Union<int, string, double, bool, DateTime, Guid, decimal, long>(lng);
		u8.TryGet(out long l8).Should().BeTrue();
		l8.Should().Be(lng);
		u8.TryGet(out int i8).Should().BeFalse();
		i8.Should().Be(0);
	}

	[Fact]
	public void union8_implicit_convert_from_T1_and_T8()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal, long> u1 = 123;
		u1.TryGet(out int i).Should().BeTrue();
		i.Should().Be(123);

		Union<int, string, double, bool, DateTime, Guid, decimal, long> u8 = 987654321L;
		u8.TryGet(out long l).Should().BeTrue();
		l.Should().Be(987654321L);
	}

	[Fact]
	public void union8_handle_null_T2_value()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal, long> u = (string?)null;
		u.TryGet(out string? s).Should().BeTrue();
		s.Should().BeNull();
		u.ToString().Should().Be("null");
	}

	[Fact]
	public void union8_pattern_match_switch_expression()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal, long> u1 = 42;
		Union<int, string, double, bool, DateTime, Guid, decimal, long> u2 = (string?)null;
		string GetTypeName(object? value) => value switch
		{
			int i => $"int:{i}",
			string s => $"string:{s}",
			double d => $"double:{d}",
			bool b => $"bool:{b}",
			DateTime dt => $"datetime:{dt:O}",
			Guid g => $"guid:{g}",
			decimal m => $"decimal:{m}",
			long l => $"long:{l}",
			null => "null",
			_ => "unknown"
		};
		GetTypeName(u1.Value).Should().Be("int:42");
		GetTypeName(u2.Value).Should().Be("null");
	}
}

