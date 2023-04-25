namespace ConflictVessels.Engine.Vessels;

public class Vessel : IPlaceableItem
{
  public int Size { get; init; }
  public int Symbol { get; init; }

    public Vessel(int size, char symbol = 'V')
  {
    Size = size;
    Symbol = symbol;
  }
}