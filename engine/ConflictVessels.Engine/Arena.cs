using System.Reactive.Subjects;
using ConflictVessels.Engine.Grids;

namespace ConflictVessels.Engine;

/// <summary>Stateful</summary>
public class Arena
{
  private readonly BehaviorSubject<bool> readySubject = new BehaviorSubject<bool>(false);
  public List<Grid> Grids { get; init; }

  public bool Ready
  {
    get => readySubject.Value;
    protected set => readySubject.OnNext(value);
  }

  public IObservable<bool> ObservableReady => readySubject;

  internal Arena(params Grid[] grids)
  {
    Grids = grids.ToList();
    Ready = grids.All(grid => grid.Ready);
  }

  public static Arena Default()
  {
    return new Arena(new Grid(), new Grid());
  }
}