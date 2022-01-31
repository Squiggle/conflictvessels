using System.Linq;
using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class AutoGrid : Grid
{
  public AutoGrid(int width, int height, params Vessel[] vessels) : base(width, height, vessels)
  {
    foreach (var vessel in Vessels)
    {
      // vertical or horizontal?
      var orientation = new Random().Next(0, 2) == 0
        ? VesselOrientation.Horizontal
        : VesselOrientation.Vertical;
      // available grid space, considering the size of the vessel
      var availableGrid = orientation switch
      {
        VesselOrientation.Horizontal => new Grid(Width - (vessel.Vessel.Size - 1), Height),
        VesselOrientation.Vertical => new Grid(Width, Height - (vessel.Vessel.Size - 1)),
        _ => throw new ArgumentException(nameof(orientation))
      };
      // occupied coords plus their shadows
      IEnumerable<Coords> occupiedCoords = Vessels
        .SelectMany(v => v.Coords)
        .SelectMany(c => c.Shadow(vessel.Vessel.Size, orientation));
      // subtract all available grid spaces from all occupied or shadowed spaces
      var options = availableGrid.Coords().Except(occupiedCoords).ToList();
      if (options.Count == 0)
      {
        throw new GridFullException(vessel.Vessel, orientation);
      }
      var placementCoords = options[new Random().Next(options.Count - 1)];
      Place(vessel.Vessel, placementCoords.X, placementCoords.Y, orientation);
    }
  }
}

public class GridFullException : Exception
{
  public Vessel VesselThatCannotFit { get; init; }
  public VesselOrientation Orientation { get; init; }

  public GridFullException(Vessel vessel, VesselOrientation orientation) : base()
  {
    VesselThatCannotFit = vessel;
  }

  public override string Message => $"Vessel of size {VesselThatCannotFit.Size} cannot fit in a {Orientation} position on this grid";
}