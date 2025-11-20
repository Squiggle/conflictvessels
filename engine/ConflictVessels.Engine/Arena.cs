using System.Reactive.Subjects;
using ConflictVessels.Engine.Grids;

namespace ConflictVessels.Engine;

/// <summary>Stateful</summary>
public class Arena : IDisposable
{
  private readonly BehaviorSubject<bool> readySubject = new BehaviorSubject<bool>(false);
  private readonly List<IDisposable> gridSubscriptions = new List<IDisposable>();

  public List<Grid> Grids { get; init; }

  public bool Ready
  {
    get => readySubject.Value;
    protected set => readySubject.OnNext(value);
  }

  public IObservable<bool> ObservableReady => readySubject;

  public Arena(params Grid[] grids)
  {
    Grids = grids.ToList();
    InitializeGridSubscriptions();
  }

  private void InitializeGridSubscriptions()
  {
    Ready = Grids.All(grid => grid.Ready);

    // Subscribe to each grid's ready state to update Arena ready state
    foreach (var grid in Grids)
    {
      var subscription = grid.ObservableReady.Subscribe(
        onNext: _ => UpdateReadyState(),
        onError: error =>
        {
          Console.Error.WriteLine($"Error in grid ready subscription: {error.Message}");
        });
      gridSubscriptions.Add(subscription);
    }
  }

  private void UpdateReadyState()
  {
    Ready = Grids.All(grid => grid.Ready);
  }

  public static Arena Default()
  {
    return new Arena(Grid.Default(), Grid.Default());
  }

  public void Dispose()
  {
    foreach (var subscription in gridSubscriptions)
    {
      subscription?.Dispose();
    }
    gridSubscriptions.Clear();

    foreach (var grid in Grids)
    {
      grid?.Dispose();
    }

    readySubject?.OnCompleted();
    readySubject?.Dispose();
  }
}