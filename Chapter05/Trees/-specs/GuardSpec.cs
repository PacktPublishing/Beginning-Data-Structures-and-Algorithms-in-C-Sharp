using System;
using Xunit;

public class GuardSpec
{
	public bool TestBool { get; set; }
	public string TestString { get; set; }

	[Fact]
	public void GreaterThan()
	{
		const string expectedError = "Guard Greater: value is '0' but must be greater than '1'.";

		//int
		Guard.GreaterThan(0, 1, "value");
		Exception ex = Assert.Throws<Exception>(() => Guard.GreaterThan(1, 0, "value"));
		Assert.Equal(expectedError, ex.Message);

		//double
		Guard.GreaterThan(0.0, 1.0, "value");
		ex = Assert.Throws<Exception>(() => Guard.GreaterThan(1.0, 0.0, "value"));
		Assert.Equal(expectedError, ex.Message);
	}

	[Fact]
	public void GreaterThanOrEqualTo()
	{
		const string expectedError = "Guard Greater: value is '10' but must be greater than '100'.";

		//int
		Guard.GreaterThanOrEqualTo(100, 1000, "value");
		Guard.GreaterThanOrEqualTo(100, 100, "value");
		Exception ex = Assert.Throws<Exception>(() => Guard.GreaterThan(100, 10, "value"));
		Assert.Equal(expectedError, ex.Message);

		//double
		Guard.GreaterThanOrEqualTo(100.0, 1000.0, "value");
		Guard.GreaterThanOrEqualTo(100.0, 100.0, "value");
		ex = Assert.Throws<Exception>(() => Guard.GreaterThan(100.0, 10.0, "value"));
		Assert.Equal(expectedError, ex.Message);
	}

	[Fact]
	public void GuardAgainstFalse()
	{
		Guard.Against(false, "Unused");
		Guard.Against(false, () => "Unused");
	}

	[Fact]
	public void GuardAgainstTrue()
	{
		const string expectedError = "Guard Against: message";

		Exception ex = Assert.Throws<Exception>(() => Guard.Against(true, "message"));
		Assert.Equal(expectedError, ex.Message);

		ex = Assert.Throws<Exception>(() => Guard.Against(true, () => "message"));
		Assert.Equal(expectedError, ex.Message);
	}

	[Fact]
	public void GuardAssertFalseThrows()
	{
		const string expectedError = "Guard Assert: TestBool was False";
		TestBool = false;

		Exception ex = Assert.Throws<Exception>(() => Guard.Assert(TestBool, "TestBool was False"));
		Assert.Equal(expectedError, ex.Message);

		ex = Assert.Throws<Exception>(() => Guard.Assert(TestBool, () => string.Format("TestBool was {0}", TestBool)));
		Assert.Equal(expectedError, ex.Message);
	}

	[Fact]
	public void GuardAssertTrue()
	{
		Guard.Assert(true, "Unused");
		Guard.Assert(true, () => "Unused");
	}

	[Fact]
	public void LessThan()
	{
		const string expectedError = "Guard Less: value is '1' but must be less than '0'.";

		//int
		Guard.LessThan(1, 0, "value");
		Exception ex = Assert.Throws<Exception>(() => Guard.LessThan(0, 1, "value"));
		Assert.Equal(expectedError, ex.Message);

		//double
		Guard.LessThan(1.0, 0.0, "value");
		ex = Assert.Throws<Exception>(() => Guard.LessThan(0.0, 1.0, "value"));
		Assert.Equal(expectedError, ex.Message);
	}

	[Fact]
	public void LessThanOrEqualTo()
	{
		const string expectedError = "Guard Less or Equal: value is '1' but must be less than or equal to '0'.";

		//int
		Guard.LessThanOrEqualTo(1, 0, "value");
		Guard.LessThanOrEqualTo(0, 0, "value");
		Exception ex = Assert.Throws<Exception>(() => Guard.LessThanOrEqualTo(0, 1, "value"));
		Assert.Equal(expectedError, ex.Message);
		
		//double
		Guard.LessThanOrEqualTo(1.0, 0.0, "value");
		Guard.LessThanOrEqualTo(0.0, 0.0, "value");
		ex = Assert.Throws<Exception>(() => Guard.LessThanOrEqualTo(0.0, 1.0, "value"));
		Assert.Equal(expectedError, ex.Message);
	}

	[Fact]
	public void NotNullOrEmpty()
	{
		TestString = "a value";
		Guard.NotNullOrEmpty(TestString, () => TestString);
	}

	[Fact]
	public void NotNullOrEmptyThrowsWhenEmpty()
	{
		const string expectedError = "TestString cannot be empty.\r\nParameter name: TestString";
		TestString = string.Empty;

		Exception ex = Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty(TestString, () => TestString));
		Assert.Equal(expectedError, ex.Message);
	}

	[Fact]
	public void NotNullOrEmptyThrowsWhenNull()
	{
		const string expectedError = "TestString cannot be null.\r\nParameter name: TestString";
		TestString = null;

		Exception ex = Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrEmpty(TestString, () => TestString));
		Assert.Equal(expectedError, ex.Message);
	}
}