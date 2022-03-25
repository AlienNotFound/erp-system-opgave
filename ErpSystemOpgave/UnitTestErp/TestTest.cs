using Xunit;

public class TestTest
{
    [Fact]
    public void TheTestThatTestsTheTests()
    {
        Assert.True(true);
    }

    [Fact]
    public void TheTestThatTestsTheTestsFails()
    {
        Assert.True(false);
    }
}