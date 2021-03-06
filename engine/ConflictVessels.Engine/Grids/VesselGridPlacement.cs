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

  public IEnumerable<Coords> Coords
  {
    get
    {
      if (Position != null)
      {
        // calculate the coords at a given point on the vessel
        Func<Coords, int, Coords> coordsAt = Position.Orientation switch
        {
          VesselOrientation.Horizontal =>
            (Coords coords, int i) => new Coords(coords.X + i, coords.Y),
          VesselOrientation.Vertical =>
            (Coords coords, int i) => new Coords(coords.X, coords.Y + i),
          _ => throw new ArgumentException(nameof(Position.Orientation))
        };

        for (var i = 0; i < Vessel.Size; i++)
        {
          yield return coordsAt(Position.Coords, i);
        }
      }
    }
  }
}