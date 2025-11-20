namespace ConflictVessels.Engine;

using System;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

public class Game : IDisposable
{
  private readonly BehaviorSubject<GamePhase> phaseSubject = new BehaviorSubject<GamePhase>(GamePhase.Setup);

  private readonly List<Player> players;
  private IDisposable? arenaReadySubscription;

  /// <summary>Identifier</summary>
  public Guid Id { get; init; }

  /// <summary>Game is Active</summary>
  public bool Active
  {
    get => phaseSubject.Value != GamePhase.Ended;
  }

  /// <summary>Total player count</summary>
  public ReadOnlyCollection<Player> Players => players.AsReadOnly();

  /// <summary>Arena for the Game</summary>
  public Arena Arena { get; init; }

  public IObservable<GamePhase> ObservablePhase => phaseSubject;

  public GamePhase Phase
  {
    get => phaseSubject.Value;
    private set => phaseSubject.OnNext(value);
  }

  /// <summary>Observable collection of Player actions</summary>
  public ObservableCollection<string> PlayerActions { get; init; }

  public Game(Arena arena, params Player[] players)
  {
    // defaults can be overridden via params if needed
    Id = Guid.NewGuid();
    this.players = players.ToList();
    Arena = arena;
    PlayerActions = new ObservableCollection<string>();

    // toggle Phase based on the readiness of the Arena
    arenaReadySubscription = arena.ObservableReady.Subscribe(
      onNext: ready =>
      {
        if (Active)
        {
          Phase = ready
            ? GamePhase.Action
            : GamePhase.Setup;
        }
      },
      onError: error =>
      {
        Console.Error.WriteLine($"Error in arena ready subscription: {error.Message}");
      });
  }

  /// <summary>
  /// Create a new Game with defaults
  /// - A normal Arena
  /// - Two Players
  /// </summary>
  public static Game Default()
  {
    return new Game(Arena.Default(), new Player(), new Player());
  }

  /// <summary>A Player can abandon the game</summary>
  public void Abandon(Player player)
  {
    if (!players.Contains(player))
    {
      throw new ArgumentException("Given Player is not participating in this game");
    }
    Phase = GamePhase.Ended;
  }

  public void Dispose()
  {
    arenaReadySubscription?.Dispose();
    Arena?.Dispose();
    phaseSubject?.OnCompleted();
    phaseSubject?.Dispose();
  }
}
