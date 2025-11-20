using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Grids;

public class Grid : IDisposable
{
  // subjects for observable properties
  private readonly BehaviorSubject<bool> readySubject = new BehaviorSubject<bool>(false);

  public int Width { get; init; }
  public int Height { get; init; }

  public bool Ready
  {
    get => readySubject.Value;
    protected set => readySubject.OnNext(value);
  }

  public IObservable<bool> ObservableReady => readySubject;

  private readonly List<VesselGridPlacement> vessels;
  public ReadOnlyCollection<VesselGridPlacement> Vessels => vessels.AsReadOnly();

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

  internal Grid(int width, int height, params Vessel[] vessels)
  {
    Width = width;
    Height = height;
    this.vessels = vessels.Select(v => new VesselGridPlacement(v)).ToList();
  }

  /// <summary>Place a vessel on the board</summary>
  public void Place(Vessel vessel, int x, int y, VesselOrientation orientation)
  {
    var placement = vessels.SingleOrDefault(v => v.Vessel == vessel);
    if (placement == null)
    {
      throw new AggregateException("Vessel is not on this board");
    }

    placement.Place(x, y, orientation);

    if (Vessels.All(v => v.Position != null))
    {
      Ready = true;
    }
  }

  /// <summary>Create a default grid with a selection of vessels</summary>
  public static Grid Default()
  {
    return new Grid(10, 10, new Vessel[] {
      new Vessel(2),
      new Vessel(3),
      new Vessel(3),
      new Vessel(4),
      new Vessel(5)
    });
  }

  public void Dispose()
  {
    readySubject?.OnCompleted();
    readySubject?.Dispose();
  }
}
