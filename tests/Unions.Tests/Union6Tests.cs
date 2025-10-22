using FluentAssertions;

namespace Unions.Tests;

public class Union6Tests
{
	[Fact]
	public void union6_store_T1_T2_T6()
	{
		var guid = new Guid("01234567-89ab-cdef-0123-456789abcdef");

		var u1 = new Union<int, string, double, bool, DateTime, Guid>(42);
		u1.TryGet(out int i1).Should().BeTrue();
		i1.Should().Be(42);
		u1.ToString().Should().Be("42");

		var u2 = new Union<int, string, double, bool, DateTime, Guid>("Hello");
		u2.TryGet(out string? s2).Should().BeTrue();
		s2.Should().Be("Hello");
		u2.ToString().Should().Be("Hello");

		var u6 = new Union<int, string, double, bool, DateTime, Guid>(guid);
		u6.TryGet(out Guid g6).Should().BeTrue();
		g6.Should().Be(guid);
		u6.TryGet(out int i6).Should().BeFalse();
		i6.Should().Be(0);
	}

	[Fact]
	public void union6_implicit_convert_from_T1_and_T6()
	{
		Union<int, string, double, bool, DateTime, Guid> u1 = 1;
		u1.TryGet(out int i).Should().BeTrue();
		i.Should().Be(1);

		var guid = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
		Union<int, string, double, bool, DateTime, Guid> u6 = guid;
		u6.TryGet(out Guid g).Should().BeTrue();
		g.Should().Be(guid);
	}

	[Fact]
	public void union6_handle_null_T2_value()
	{
		Union<int, string, double, bool, DateTime, Guid> u = (string?)null;
		u.TryGet(out string? s).Should().BeTrue();
		s.Should().BeNull();
		u.ToString().Should().Be("null");
	}

	[Fact]
	public void union6_pattern_match_switch_expression()
	{
		Union<int, string, double, bool, DateTime, Guid> u1 = 42;
		Union<int, string, double, bool, DateTime, Guid> u2 = (string?)null;
		string GetTypeName(object? value) => value switch
		{
			int i => $"int:{i}",
			string s => $"string:{s}",
			double d => $"double:{d}",
			bool b => $"bool:{b}",
			DateTime dt => $"datetime:{dt:O}",
			Guid g => $"guid:{g}",
			null => "null",
			_ => "unknown"
		};
		GetTypeName(u1.Value).Should().Be("int:42");
		GetTypeName(u2.Value).Should().Be("null");
	}
}

