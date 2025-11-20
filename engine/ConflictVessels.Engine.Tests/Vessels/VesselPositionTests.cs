using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Tests.Vessels;

public class VesselPositionTests
{
  [Fact]
  public void VesselPosition_describes_a_position_and_orientation()
  {
    var position = new VesselPosition(1, 2, VesselOrientation.Vertical);
    Assert.Equal(1, position.Coords.X);
    Assert.Equal(2, position.Coords.Y);
    Assert.Equal(VesselOrientation.Vertical, position.Orientation);
  }
}