using FluentAssertions;

namespace Unions.Tests;

public class Union5Tests
{
	[Fact]
	public void union5_store_T1_T2_T5()
	{
		var date = new DateTime(2020, 1, 2);

		var u1 = new Union<int, string, double, bool, DateTime>(42);
		u1.TryGet(out int i1).Should().BeTrue();
		i1.Should().Be(42);
		u1.ToString().Should().Be("42");

		var u2 = new Union<int, string, double, bool, DateTime>("Hello");
		u2.TryGet(out string? s2).Should().BeTrue();
		s2.Should().Be("Hello");
		u2.ToString().Should().Be("Hello");

		var u5 = new Union<int, string, double, bool, DateTime>(date);
		u5.TryGet(out DateTime d5).Should().BeTrue();
		d5.Should().Be(date);
		u5.TryGet(out int i5).Should().BeFalse();
		i5.Should().Be(0);
	}

	[Fact]
	public void union5_implicit_convert_from_T1_and_T5()
	{
		Union<int, string, double, bool, DateTime> u1 = 9;
		u1.TryGet(out int i).Should().BeTrue();
		i.Should().Be(9);

		var date = new DateTime(1999, 12, 31);
		Union<int, string, double, bool, DateTime> u5 = date;
		u5.TryGet(out DateTime d).Should().BeTrue();
		d.Should().Be(date);
	}

	[Fact]
	public void union5_handle_null_T2_value()
	{
		Union<int, string, double, bool, DateTime> u = (string?)null;
		u.TryGet(out string? s).Should().BeTrue();
		s.Should().BeNull();
		u.ToString().Should().Be("null");
	}

	[Fact]
	public void union5_pattern_match_switch_expression()
	{
		Union<int, string, double, bool, DateTime> u1 = 42;
		Union<int, string, double, bool, DateTime> u2 = (string?)null;
		string GetTypeName(object? value) => value switch
		{
			int i => $"int:{i}",
			string s => $"string:{s}",
			double d => $"double:{d}",
			bool b => $"bool:{b}",
			DateTime dt => $"datetime:{dt:O}",
			null => "null",
			_ => "unknown"
		};
		GetTypeName(u1.Value).Should().Be("int:42");
		GetTypeName(u2.Value).Should().Be("null");
	}
}

