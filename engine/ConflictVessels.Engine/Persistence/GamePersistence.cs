namespace ConflictVessels.Engine.Persistence;

/// <summary>
/// Provides methods to create snapshots from Game instances and restore Game instances from snapshots.
/// This separates persistence concerns from the domain model.
/// </summary>
public static class GamePersistence
{
    /// <summary>
    /// Creates a serializable snapshot of the current game state.
    /// Note: Arena and Players are stored as their serializable representations.
    /// For now, these use the domain objects directly, but can be replaced with dedicated DTOs later.
    /// </summary>
    public static GameSnapshot CreateSnapshot(this Game game)
    {
        if (game == null) throw new ArgumentNullException(nameof(game));

        return new GameSnapshot
        {
            Id = game.Id,
            Phase = game.Phase.ToString(),
            Arena = game.Arena,
            Players = game.Players.ToList(),
            PlayerActions = game.PlayerActions.ToList()
        };
    }

    /// <summary>
    /// Restores a Game instance from a snapshot.
    /// This method properly reconstructs the domain object with all reactive subscriptions.
    /// </summary>
    public static Game RestoreFromSnapshot(GameSnapshot snapshot)
    {
        if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

        // Parse phase enum
        if (!Enum.TryParse<GamePhase>(snapshot.Phase, out var phase))
        {
            throw new InvalidOperationException($"Invalid game phase: {snapshot.Phase}");
        }

        // Use domain objects directly from snapshot
        // (In the future, Arena and Player will have their own snapshot/restore methods)
        var arena = snapshot.Arena;
        var players = snapshot.Players.ToArray();

        // Use the standard constructor which sets up reactive subscriptions
        var game = new Game(arena, players)
        {
            Id = snapshot.Id
        };

        // Restore player actions
        foreach (var action in snapshot.PlayerActions)
        {
            game.PlayerActions.Add(action);
        }

        // Override phase after construction if it's different from the calculated phase
        // This handles the case where a game was saved in a specific phase
        if (game.Phase != phase)
        {
            // Use reflection to set the phase directly since the setter is private
            var phaseProperty = typeof(Game).GetProperty(nameof(Game.Phase));
            phaseProperty?.SetValue(game, phase);
        }

        return game;
    }
}
