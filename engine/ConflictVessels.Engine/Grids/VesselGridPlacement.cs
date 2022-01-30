using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class VesselGridPlacement
{
  public Vessel Vessel { get; init; }
  public VesselPosition? Position { get; private set; }
  public VesselGridPlacement(Vessel vessel)
  {
    Vessel = vessel;
    Position = null;
  }

  public void Place(int x, int y, VesselOrientation orientation)
  {
    Position = new VesselPosition(x, y, orientation);
  }
}