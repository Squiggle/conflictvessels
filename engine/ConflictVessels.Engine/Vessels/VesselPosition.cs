using ConflictVessels.Engine.Grids;

namespace ConflictVessels.Engine.Vessels;

public class VesselPosition
{
  public Coords Coords { get; init; }

  public VesselOrientation Orientation { get; init; }

  public VesselPosition(int x, int y, VesselOrientation orientation)
  {
    Coords = new Coords(x, y);
    Orientation = orientation;
  }
}