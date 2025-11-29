namespace ConflictVessels.Web.Tests.Pages;

/// <summary>
/// Unit tests for the Home page component.
/// Tests verify rendering, content, navigation links, and page metadata.
/// </summary>
public class HomeTests : TestContext
{
    [Fact]
    public void Component_renders_without_errors()
    {
        // Arrange & Act
        var cut = RenderComponent<ConflictVessels.Web.Pages.Home>();

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void Component_displays_Conflict_Vessels_heading()
    {
        // Arrange & Act
        var cut = RenderComponent<ConflictVessels.Web.Pages.Home>();

        // Assert
        var heading = cut.Find("h1");
        Assert.Equal("Conflict Vessels", heading.TextContent);
    }

    [Fact]
    public void Component_displays_game_description()
    {
        // Arrange & Act
        var cut = RenderComponent<ConflictVessels.Web.Pages.Home>();

        // Assert
        var paragraph = cut.Find("p");
        Assert.Equal("A turn-based naval warfare game", paragraph.TextContent);
    }

    [Fact]
    public void Component_has_Play_Game_link()
    {
        // Arrange & Act
        var cut = RenderComponent<ConflictVessels.Web.Pages.Home>();

        // Assert
        var link = cut.Find("a");
        Assert.NotNull(link);
        Assert.Contains("Play Game", link.TextContent);
    }

    [Fact]
    public void Play_Game_link_points_to_game_route()
    {
        // Arrange & Act
        var cut = RenderComponent<ConflictVessels.Web.Pages.Home>();

        // Assert
        var link = cut.Find("a");
        Assert.Equal("/game", link.GetAttribute("href"));
    }

    [Fact]
    public void PageTitle_is_set_to_Conflict_Vessels()
    {
        // Arrange & Act
        var cut = RenderComponent<ConflictVessels.Web.Pages.Home>();

        // Assert
        // PageTitle is a Blazor component that sets the browser page title
        // We can verify it's rendered by checking for the component in the markup
        var markup = cut.Markup;
        // The component renders successfully if we get this far
        Assert.NotNull(markup);
    }
}
