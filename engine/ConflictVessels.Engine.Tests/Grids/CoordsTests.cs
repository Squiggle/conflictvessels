using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Tests.Grids;

public class CoordsTests
{
  [Fact]
  public void Coords_calculates_horizontal_shadows_correctly()
  {
    var coord = new Coords(5, 5);

    var horizontalShadow = coord.Shadow(3, VesselOrientation.Horizontal);
    Assert.All(horizontalShadow, coord => Assert.Equal(5, coord.Y));
    Assert.Collection(horizontalShadow,
      c => Assert.Equal(5, c.X),
      c => Assert.Equal(4, c.X),
      c => Assert.Equal(3, c.X)
    );
  }

  [Fact]
  public void Coords_calculates_vertical_shadows_correctly()
  {
    var coord = new Coords(5, 5);

    var verticalShadow = coord.Shadow(3, VesselOrientation.Vertical);
    Assert.All(verticalShadow, coord => Assert.Equal(5, coord.X));
    Assert.Collection(verticalShadow,
      c => Assert.Equal(5, c.Y),
      c => Assert.Equal(4, c.Y),
      c => Assert.Equal(3, c.Y)
    );
  }

  [Fact]
  public void Coords_test_equality()
  {
    var coord = new Coords(1, 1);
    var sameCoord = new Coords(1, 1);
    var differentCoord = new Coords(1, 2);

    Assert.Equal(coord, sameCoord);
    Assert.NotEqual(coord, differentCoord);
  }
}