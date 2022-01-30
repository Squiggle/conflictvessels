using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Tests.Vessels;

public class VesselPositionTests
{
  public void VesselPosition_describes_a_position_and_orientation()
  {
    var position = new VesselPosition(1, 2, VesselOrientation.Vertical);
    Assert.Equal(1, position.X);
    Assert.Equal(2, position.Y);
    Assert.Equal(VesselOrientation.Vertical, position.Orientation);
  }
}