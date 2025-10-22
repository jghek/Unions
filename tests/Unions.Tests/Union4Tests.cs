using FluentAssertions;

namespace Unions.Tests;

public class Union4Tests
{
	[Fact]
	public void union4_store_T1_T2_T4()
	{
		var u1 = new Union<int, string, double, bool>(42);
		u1.TryGet(out int i1).Should().BeTrue();
		i1.Should().Be(42);
		u1.TryGet(out string? s1).Should().BeFalse();
		s1.Should().BeNull();
		u1.Value.Should().Be(42);
		u1.ToString().Should().Be("42");

		var u2 = new Union<int, string, double, bool>("Hello");
		u2.TryGet(out string? s2).Should().BeTrue();
		s2.Should().Be("Hello");
		u2.TryGet(out bool b2).Should().BeFalse();
		b2.Should().BeFalse();
		u2.Value.Should().Be("Hello");
		u2.ToString().Should().Be("Hello");

		var u4 = new Union<int, string, double, bool>(true);
		u4.TryGet(out bool b4).Should().BeTrue();
		b4.Should().BeTrue();
		u4.TryGet(out int i4).Should().BeFalse();
		i4.Should().Be(0);
		u4.Value.Should().Be(true);
	}

	[Fact]
	public void union4_implicit_convert_from_T1_and_T4()
	{
		Union<int, string, double, bool> u1 = 5;
		u1.TryGet(out int i).Should().BeTrue();
		i.Should().Be(5);

		Union<int, string, double, bool> u4 = true;
		u4.TryGet(out bool b).Should().BeTrue();
		b.Should().BeTrue();
	}

	[Fact]
	public void union4_handle_null_T2_value()
	{
		Union<int, string, double, bool> u = (string?)null;
		u.TryGet(out string? s).Should().BeTrue();
		s.Should().BeNull();
		u.ToString().Should().Be("null");
	}

	[Fact]
	public void union4_pattern_match_switch_expression()
	{
		Union<int, string, double, bool> u1 = 42;
		Union<int, string, double, bool> u2 = (string?)null;
		string GetTypeName(object? value) => value switch
		{
			int i => $"int:{i}",
			string s => $"string:{s}",
			double d => $"double:{d}",
			bool b => $"bool:{b}",
			null => "null",
			_ => "unknown"
		};
		GetTypeName(u1.Value).Should().Be("int:42");
		GetTypeName(u2.Value).Should().Be("null");
	}
}

