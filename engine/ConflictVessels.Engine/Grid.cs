namespace ConflictVessels.Engine;

public class Grid
{
  public int Width { get; init; }
  public int Height { get; init; }

  internal Grid()
  {
    Width = 10;
    Height = 10;
  }
}