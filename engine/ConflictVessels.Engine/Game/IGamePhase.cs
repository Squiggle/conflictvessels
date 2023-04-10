using System;

namespace ConflictVessels.Engine.Game;

public interface IGamePhase
{
    IObservable<bool> PhaseState { get; }
} 