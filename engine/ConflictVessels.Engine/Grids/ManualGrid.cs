using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class ManualGrid : Grid
{
  public ManualGrid(int width, int height, params Vessel[] vessels) : base(width, height, vessels) { }
}