namespace ConflictVessels.Engine.Vessels;

public class VesselPosition
{
  public VesselPosition(int x, int y, VesselOrientation orientation)
  {
    X = x;
    Y = y;
    Orientation = orientation;
  }

  public int X { get; init; }
  public int Y { get; init; }
  public VesselOrientation Orientation { get; init; }
}