namespace ConflictVessels.Engine.Game;

using System;
using System.Collections;
using System.Reactive.Linq;
using System.Reactive.Subjects;

/// <summary>
/// Coordinates a series of Phases of a game.
/// Handles the overall state of the game (which phase)
/// and handles the termination of the game session.
/// </summary>
public class GameSession
{
    private readonly IEnumerator _phasesEnumerator;
    private IDisposable? _phaseSubscription;
    private readonly BehaviorSubject<bool> _active = new(true);

    /// <summary>
    /// Game session identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Fetch the current active phase of the game
    /// </summary>
    public IGamePhase CurrentPhase => (IGamePhase)_phasesEnumerator.Current;

    /// <summary>
    /// Observe the squeuence of phases in the game
    /// </summary>
    public IObservable<IGamePhase> Phases { get; init; }

    /// <summary>
    /// Observe the active state of the game
    /// </summary>
    public IObservable<Boolean> Active => _active.AsObservable();

    /// <summary>
    /// Create a new game session
    /// defaults as active
    /// </summary>
    /// <param name="phases">Sequence of game phases</param>
    /// <exception cref="ArgumentException">Throws if no phases are present</exception>
    public GameSession(params IGamePhase[] phases)
    {
        if (phases.Length == 0)
        {
            throw new ArgumentException("Game session must have at least one phase");
        }

        Id = Guid.NewGuid();

        Phases = phases.ToObservable();
        _phasesEnumerator = phases.GetEnumerator();

        // observe the current phase and handle events
        Phases.Subscribe(PhaseActive, error => throw error, GameCompleted);

        // start on the first phase
        _phasesEnumerator.MoveNext();
    }

    private void GameCompleted()
    {
       
    }

    private void PhaseActive(IGamePhase phase)
    {
        _phaseSubscription = phase.PhaseState.Subscribe(completed => {
            if (completed)
            {
                _phaseSubscription?.Dispose();
                if (!_phasesEnumerator.MoveNext())
                {
                    _active.OnNext(false);
                }
            }
        });
    }
}
