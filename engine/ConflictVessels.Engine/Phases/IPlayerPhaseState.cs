namespace ConflictVessels.Engine.Phases
{
    /// <summary>
    /// Represents a player's board within the game
    /// </summary>
    public interface IPlayerPhaseState
    {
        /// <summary>
        /// Observe the ready state of the board
        /// </summary>
        IObservable<bool> Ready { get; }
        
        /// <summary>
        /// Observe the Defeated state of the board
        /// </summary>
        IObservable<bool> Defeated { get; }
    }
}