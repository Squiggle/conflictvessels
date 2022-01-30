namespace ConflictVessels.Engine.Vessels;

public class Vessel
{
  public int Size { get; init; }

  internal Vessel(int size)
  {
    Size = size;
  }
}