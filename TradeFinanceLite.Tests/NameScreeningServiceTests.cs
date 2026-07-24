using Xunit;

public class NameScreeningServiceTests
{
    [Fact]
    public void Screen_ExactWatchlistMatch_ReturnsFlaggedTrue()
    {
        // Arrange
        string name = "Zenith Global Holdings";

        // Act
        var (isFlagged, matchedName, score) = NameScreeningService.Screen(name);

        // Assert
        Assert.True(isFlagged);
        Assert.Equal(100, score);
        Assert.Equal("Zenith Global Holdings", matchedName);
    }

    [Fact]
    public void Screen_CompletelyDifferentName_ReturnsFlaggedFalse()
    {
        // Arrange
        string name = "Karachi Textile Mills";

        // Act
        var (isFlagged, matchedName, score) = NameScreeningService.Screen(name);

        // Assert
        Assert.False(isFlagged);
    }

    [Theory]
    [InlineData("Al-Rashid Trading Co")]
    [InlineData("Crimson Star Exports")]
    [InlineData("Northgate Industries Ltd")]
    public void Screen_KnownWatchlistNames_AreFlagged(string watchlistName)
    {
        // Act
        var (isFlagged, _, _) = NameScreeningService.Screen(watchlistName);

        // Assert
        Assert.True(isFlagged);
    }
}