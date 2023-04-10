namespace ConflictVessels.Engine.Game;

public interface IGameArena
{
    IObservable<bool> Ready { get; }
}