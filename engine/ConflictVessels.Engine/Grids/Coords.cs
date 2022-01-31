using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class Coords
{
  public int X { get; init; }
  public int Y { get; init; }

  public Coords(int x, int y)
  {
    X = x;
    Y = y;
  }

  public IEnumerable<Coords> Shadow(int size, VesselOrientation orientation)
  {
    for (var i = 0; i < size; i++)
    {
      yield return orientation switch
      {
        VesselOrientation.Horizontal => new Coords(X - i, Y),
        VesselOrientation.Vertical => new Coords(X, Y - i),
        _ => throw new ArgumentException(nameof(orientation))
      };
    }
  }

  public override int GetHashCode()
  {
    unchecked
    {
      return HashCode.Combine(X, Y);
    }
  }

  public override bool Equals(object? obj) =>
      obj is not null
      && obj.GetType() == typeof(Coords)
      && obj.GetHashCode() == GetHashCode();

  public static bool operator ==(Coords a, Coords b) =>
    a is not null && b is not null && a.Equals(b);

  public static bool operator !=(Coords a, Coords b) => !(a == b);
}