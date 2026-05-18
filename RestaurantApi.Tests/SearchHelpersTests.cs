using RestaurantApi.Services;
using Xunit;

namespace RestaurantApi.Tests;

public class SearchHelpersTests
{
    [Fact]
    public void ToIlikePattern_wraps_with_percent_signs()
    {
        Assert.Equal("%pizza%", SearchHelpers.ToIlikePattern("pizza"));
    }

    [Fact]
    public void ToIlikePattern_escapes_percent_underscore_and_backslash()
    {
        Assert.Equal(@"%50\%\_off\\%", SearchHelpers.ToIlikePattern(@"50%_off\"));
    }

    [Fact]
    public void ToIlikePattern_trims_input()
    {
        Assert.Equal("%pizza%", SearchHelpers.ToIlikePattern("  pizza  "));
    }
}
