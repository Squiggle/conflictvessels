using System.Reactive.Linq;
using ConflictVessels.Engine.Game;

namespace ConflictVessels.Engine.Phases;

public class GamePhase : IGamePhase
{
    private readonly IPlayerPhaseState[] _playerBoards;

    public GamePhase(params IPlayerPhaseState[] playerBoards)
    {
        _playerBoards = playerBoards;
    }

    public IObservable<bool> PhaseState => _playerBoards.Select(board => board.Defeated).CombineLatest().Select(defeated => defeated.Any(r => r));
}