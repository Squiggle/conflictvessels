using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class AutoGrid : Grid
{
  public AutoGrid(params Vessel[] vessels) : base(vessels)
  {
    for (var i = 0; i < Vessels.Count; i++)
    {
      // for now, place all vessels vertically in a row
      Place(Vessels[i].Vessel, i, 0, VesselOrientation.Vertical);
      // TODO
      // place this vessel on the grid randomly
      // ensuring no overlap with other placed vessels
    }
  }
}