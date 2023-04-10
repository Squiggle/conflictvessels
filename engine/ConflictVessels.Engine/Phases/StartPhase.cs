using System.Reactive.Linq;
using ConflictVessels.Engine.Game;

namespace ConflictVessels.Engine.Phases;

public class StartPhase : IGamePhase
{
    private readonly IPlayerBoard[] _playerBoards;

    public StartPhase(params IPlayerBoard[] playerBoards)
    {
        _playerBoards = playerBoards;
    }

    public IObservable<bool> PhaseState => _playerBoards.Select(board => board.Ready).CombineLatest().Select(ready => ready.All(r => r));
}