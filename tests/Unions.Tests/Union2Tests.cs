using FluentAssertions;

namespace Unions.Tests;

public class Union2Tests
{
	[Fact]
	public void union2_store_T1_value_when_constructed_with_T1()
	{
		// Arrange
		var union = new Union<int, string>(42);

		// Act
		bool gotT1 = union.TryGet(out int t1Value);
		bool gotT2 = union.TryGet(out string t2Value);

		// Assert
		gotT1.Should().BeTrue();
		t1Value.Should().Be(42);
		gotT2.Should().BeFalse();
		t2Value.Should().BeNull();
		union.Value.Should().Be(42);
		union.ToString().Should().Be("42");
	}

	[Fact]
	public void union2_store_T2_value_when_constructed_with_T2()
	{
		// Arrange
		var union = new Union<int, string>("Hello");

		// Act
		bool gotT1 = union.TryGet(out int t1Value);
		bool gotT2 = union.TryGet(out string t2Value);

		// Assert
		gotT2.Should().BeTrue();
		t2Value.Should().Be("Hello");
		gotT1.Should().BeFalse();
		t1Value.Should().Be(0);
		union.Value.Should().Be("Hello");
		union.ToString().Should().Be("Hello");
	}

	[Fact]
	public void union2_implicit_convert_from_T1()
	{
		// Arrange
		Union<int, string> union = 100;

		// Act
		bool gotT1 = union.TryGet(out int t1Value);
		bool gotT2 = union.TryGet(out string t2Value);

		// Assert
		gotT1.Should().BeTrue();
		t1Value.Should().Be(100);
		gotT2.Should().BeFalse();
		t2Value.Should().BeNull();
	}

	[Fact]
	public void union2_implicit_convert_from_T2()
	{
		// Arrange
		Union<int, string> union = "World";

		// Act
		bool gotT1 = union.TryGet(out int t1Value);
		bool gotT2 = union.TryGet(out string t2Value);

		// Assert
		gotT2.Should().BeTrue();
		t2Value.Should().Be("World");
		gotT1.Should().BeFalse();
		t1Value.Should().Be(0);
	}

	[Fact]
	public void union2_handle_null_T2_value()
	{
		// Arrange
		Union<int, string> union = (string?)null;

		// Act
		bool gotT2 = union.TryGet(out string t2Value);
		bool gotT1 = union.TryGet(out int t1Value);

		// Assert
		gotT2.Should().BeTrue();
		t2Value.Should().BeNull();
		gotT1.Should().BeFalse();
		t1Value.Should().Be(0);
		union.Value.Should().BeNull();
		union.ToString().Should().Be("null");
	}

	[Fact]
	public void union2_pattern_match_switch_statement()
	{
		// Arrange
		Union<int, string> union1 = 10;
		Union<int, string> union2 = "Hello";

		// Act & Assert
		string GetTypeName(object? value)
		{
			switch (value)
			{
				case int i:
					return $"int:{i}";
				case string s:
					return $"string:{s}";
				case null:
					return "null";
				default:
					return "unknown";
			}
		}

		GetTypeName(union1.Value).Should().Be("int:10");
		GetTypeName(union2.Value).Should().Be("string:Hello");
	}

	[Fact]
	public void union2_pattern_match_switch_expression()
	{
		// Arrange
		Union<int, string> union1 = 42;
		Union<int, string> union2 = (string?)null;

		// Act & Assert
		string GetTypeName(object? value) => value switch
		{
			int i => $"int:{i}",
			string s => $"string:{s}",
			null => "null",
			_ => "unknown"
		};

		GetTypeName(union1.Value).Should().Be("int:42");
		GetTypeName(union2.Value).Should().Be("null");
	}
}

