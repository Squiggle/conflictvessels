using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Vessels.Tests;

public class VesselTests
{
  [Fact]
  public void Vessel_can_be_created_with_a_size()
  {
    var vessel = new Vessel(2);
    Assert.Equal(2, vessel.Size);
  }
}