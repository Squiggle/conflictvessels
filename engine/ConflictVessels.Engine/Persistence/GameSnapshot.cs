namespace ConflictVessels.Engine.Persistence;

/// <summary>
/// Data Transfer Object representing a serializable snapshot of Game state.
/// This class is separate from the domain model to avoid coupling game logic with serialization concerns.
/// </summary>
public record GameSnapshot
{
    /// <summary>Game identifier</summary>
    public Guid Id { get; init; }

    /// <summary>Current game phase as string for serialization</summary>
    public string Phase { get; init; } = string.Empty;

    /// <summary>Serialized arena state - using Arena directly for now until Arena gets its own snapshot</summary>
    public Arena Arena { get; init; } = null!;

    /// <summary>Serialized player list - using Player directly for now until Player gets its own snapshot</summary>
    public List<Player> Players { get; init; } = new();

    /// <summary>History of player actions</summary>
    public List<string> PlayerActions { get; init; } = new();
}
