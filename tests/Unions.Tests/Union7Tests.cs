using FluentAssertions;

namespace Unions.Tests;

public class Union7Tests
{
	[Fact]
	public void union7_store_T1_T2_T7()
	{
		var dec = 9.99m;

		var u1 = new Union<int, string, double, bool, DateTime, Guid, decimal>(42);
		u1.TryGet(out int i1).Should().BeTrue();
		i1.Should().Be(42);
		u1.ToString().Should().Be("42");

		var u2 = new Union<int, string, double, bool, DateTime, Guid, decimal>("Hello");
		u2.TryGet(out string? s2).Should().BeTrue();
		s2.Should().Be("Hello");
		u2.ToString().Should().Be("Hello");

		var u7 = new Union<int, string, double, bool, DateTime, Guid, decimal>(dec);
		u7.TryGet(out decimal d7).Should().BeTrue();
		d7.Should().Be(dec);
		u7.TryGet(out int i7).Should().BeFalse();
		i7.Should().Be(0);
	}

	[Fact]
	public void union7_implicit_convert_from_T1_and_T7()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal> u1 = 8;
		u1.TryGet(out int i).Should().BeTrue();
		i.Should().Be(8);

		Union<int, string, double, bool, DateTime, Guid, decimal> u7 = 123.45m;
		u7.TryGet(out decimal d).Should().BeTrue();
		d.Should().Be(123.45m);
	}

	[Fact]
	public void union7_handle_null_T2_value()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal> u = (string?)null;
		u.TryGet(out string? s).Should().BeTrue();
		s.Should().BeNull();
		u.ToString().Should().Be("null");
	}

	[Fact]
	public void union7_pattern_match_switch_expression()
	{
		Union<int, string, double, bool, DateTime, Guid, decimal> u1 = 42;
		Union<int, string, double, bool, DateTime, Guid, decimal> u2 = (string?)null;
		string GetTypeName(object? value) => value switch
		{
			int i => $"int:{i}",
			string s => $"string:{s}",
			double d => $"double:{d}",
			bool b => $"bool:{b}",
			DateTime dt => $"datetime:{dt:O}",
			Guid g => $"guid:{g}",
			decimal m => $"decimal:{m}",
			null => "null",
			_ => "unknown"
		};
		GetTypeName(u1.Value).Should().Be("int:42");
		GetTypeName(u2.Value).Should().Be("null");
	}
}

