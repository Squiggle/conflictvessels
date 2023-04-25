using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ConflictVessels.Engine.Phases;
using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class Grid : IPlayerPhaseState
{
    private readonly BehaviorSubject<bool> defeatedSubject = new BehaviorSubject<bool>(false);
    private readonly List<VesselGridPlacement> placements;
    public int Width { get; init; }
    public int Height { get; init; }

    public IObservable<bool> Ready => placements
        .Select(p => p.Placed)
        .CombineLatest()
        .Select(placed => placed.All(r => r));

    public IObservable<bool> Defeated => defeatedSubject;

    public ReadOnlyCollection<VesselGridPlacement> Vessels => placements.AsReadOnly();

    public IEnumerable<Coords> Coords()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                yield return new Coords(x, y);
            }
        }
    }

    internal Grid(int width, int height, params IPlaceableItem[] vessels)
    {
        Width = width;
        Height = height;
        placements = vessels.Select(v => new VesselGridPlacement(v)).ToList();
    }

    /// <summary>
    /// Place a vessel on the board
    /// </summary>
    public void Place(IPlaceableItem vessel, int x, int y, VesselOrientation orientation)
    {
        var placement = placements.SingleOrDefault(v => v.Vessel == vessel);
        if (placement == null)
        {
            throw new AggregateException("Vessel is not on this board");
        }

        placement.Place(x, y, orientation);
    }

    // /// <summary>Create a default grid with a selection of vessels</summary>
    // public static Grid Default()
    // {
    //   return new Grid(10, 10, new Vessel[] {
    //     new Vessel(2),
    //     new Vessel(3),
    //     new Vessel(3),
    //     new Vessel(4),
    //     new Vessel(5)
    //   });
    // }
}
