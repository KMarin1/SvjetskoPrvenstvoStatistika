namespace PodatkovniSloj.Tests;

public sealed class ChampionshipTests
{
    [Theory]
    [InlineData(Championship.Men, "men")]
    [InlineData(Championship.Women, "women")]
    public void ToApiName_gives_the_path_segment_the_api_uses(Championship championship, string expected) =>
        Assert.Equal(expected, championship.ToApiName());

    [Theory]
    [InlineData("men", Championship.Men)]
    [InlineData("women", Championship.Women)]
    [InlineData(" WOMEN ", Championship.Women)]
    public void TryParseApiName_reads_a_name_ignoring_case_and_spaces(string name, Championship expected)
    {
        Assert.True(ChampionshipExtensions.TryParseApiName(name, out var championship));
        Assert.Equal(expected, championship);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("mixed")]
    [InlineData("Men's")]
    public void TryParseApiName_rejects_anything_else(string? name) =>
        Assert.False(ChampionshipExtensions.TryParseApiName(name, out _));

    [Fact]
    public void ToApiName_rejects_an_undefined_value() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => ((Championship)42).ToApiName());
}
