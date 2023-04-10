using System.Linq;
using ConflictVessels.Engine.Grids;

namespace Checker
{
  public static class GridExtensions
  {
    public static void Print(this Grid grid)
    {
      $"Width: {grid.Width}".Out();
      $"Height: {grid.Height}".Out();
      " --- ".Out();
      $"Vessels: {string.Join(", ", grid.Vessels.Select(v => v.Vessel.Size))}".Out();
      " --- ".Out();

      var vesselCoords = grid.Vessels.SelectMany(v => v.Coords).ToList();
      var square = (Coords coords) => vesselCoords.Contains(coords)
        ? "V "
        : "  ";

      $" {string.Join("", Enumerable.Range(0, grid.Width).Select(_ => "__"))}".Out();
      foreach (var row in grid.Coords().Chunk(grid.Width))
      {
        $"|{string.Join("", row.Select(square))}".Out();
      }
    }
  }
}