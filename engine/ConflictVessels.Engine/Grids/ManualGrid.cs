using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class ManualGrid : Grid
{
  public ManualGrid(params Vessel[] vessels) : base(vessels) { }
}