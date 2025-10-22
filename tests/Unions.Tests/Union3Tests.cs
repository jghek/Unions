using FluentAssertions;

namespace Unions.Tests;

public class Union3Tests
{
	[Fact]
	public void union3_store_T1_T2_T3()
	{
		// T1
		var u1 = new Union<int, string, double>(42);
		u1.TryGet(out int i1).Should().BeTrue();
		i1.Should().Be(42);
		u1.TryGet(out string? s1).Should().BeFalse();
		s1.Should().BeNull();
		u1.Value.Should().Be(42);
		u1.ToString().Should().Be("42");

		// T2
		var u2 = new Union<int, string, double>("Hello");
		u2.TryGet(out string? s2).Should().BeTrue();
		s2.Should().Be("Hello");
		u2.TryGet(out double d2).Should().BeFalse();
		d2.Should().Be(0);
		u2.Value.Should().Be("Hello");
		u2.ToString().Should().Be("Hello");

		// T3
		var u3 = new Union<int, string, double>(3.14);
		u3.TryGet(out double d3).Should().BeTrue();
		d3.Should().Be(3.14);
		u3.TryGet(out int i3).Should().BeFalse();
		i3.Should().Be(0);
		u3.Value.Should().Be(3.14);
	}

	[Fact]
	public void union3_implicit_convert_from_T1_and_T3()
	{
		Union<int, string, double> u1 = 100;
		u1.TryGet(out int i).Should().BeTrue();
		i.Should().Be(100);
		u1.TryGet(out string? s).Should().BeFalse();
		s.Should().BeNull();

		Union<int, string, double> u3 = 2.5;
		u3.TryGet(out double d).Should().BeTrue();
		d.Should().Be(2.5);
		u3.TryGet(out int i2).Should().BeFalse();
		i2.Should().Be(0);
	}

	[Fact]
	public void union3_handle_null_T2_value()
	{
		Union<int, string, double> u = (string?)null;
		u.TryGet(out string? s).Should().BeTrue();
		s.Should().BeNull();
		u.TryGet(out int i).Should().BeFalse();
		i.Should().Be(0);
		u.Value.Should().BeNull();
		u.ToString().Should().Be("null");
	}

	[Fact]
	public void union3_pattern_match_switch_expression()
	{
		Union<int, string, double> u1 = 7;
		Union<int, string, double> u2 = (string?)null;
		string GetTypeName(object? value) => value switch
		{
			int i => $"int:{i}",
			string s => $"string:{s}",
			double d => $"double:{d}",
			null => "null",
			_ => "unknown"
		};
		GetTypeName(u1.Value).Should().Be("int:7");
		GetTypeName(u2.Value).Should().Be("null");
	}
}

