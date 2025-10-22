using FluentAssertions;

namespace Unions.Tests;

public class Union9Tests
{
	[Fact]
	public void union9_store_T1_T2_T9()
	{
		var ts = TimeSpan.FromSeconds(5);

		var u1 = new Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan>(42);
		u1.TryGet(out int i1).Should().BeTrue();
		i1.Should().Be(42);
		u1.ToString().Should().Be("42");

		var u2 = new Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan>("Hello");
		u2.TryGet(out string? s2).Should().BeTrue();
		s2.Should().Be("Hello");
		u2.ToString().Should().Be("Hello");

		var u9 = new Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan>(ts);
		u9.TryGet(out TimeSpan t9).Should().BeTrue();
		t9.Should().Be(ts);
		u9.TryGet(out int i9).Should().BeFalse();
		i9.Should().Be(0);
	}

	[Fact]
	public void union9_implicit_convert_from_T1_and_T9()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan> u1 = 256;
		u1.TryGet(out int i).Should().BeTrue();
		i.Should().Be(256);

		Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan> u9 = TimeSpan.FromMinutes(3);
		u9.TryGet(out TimeSpan t).Should().BeTrue();
		t.Should().Be(TimeSpan.FromMinutes(3));
	}

	[Fact]
	public void union9_handle_null_T2_value()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan> u = (string?)null;
		u.TryGet(out string? s).Should().BeTrue();
		s.Should().BeNull();
		u.ToString().Should().Be("null");
	}

	[Fact]
	public void union9_pattern_match_switch_expression()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan> u1 = 42;
		Union<int, string, double, bool, DateTime, Guid, decimal, long, TimeSpan> u2 = (string?)null;
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
			TimeSpan ts => $"timespan:{ts}",
			null => "null",
			_ => "unknown"
		};
		GetTypeName(u1.Value).Should().Be("int:42");
		GetTypeName(u2.Value).Should().Be("null");
	}
}

