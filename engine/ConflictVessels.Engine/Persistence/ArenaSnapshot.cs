namespace ConflictVessels.Engine.Persistence;

/// <summary>
/// Snapshot DTO for Arena persistence.
/// Separates serialization concerns from the domain model.
/// </summary>
public record ArenaSnapshot
{
    public List<GridSnapshot> Grids { get; init; } = new();
}

/// <summary>
/// Extension methods for Arena snapshot creation and restoration.
/// </summary>
public static class ArenaPersistence
{
    public static ArenaSnapshot CreateSnapshot(this Arena arena)
    {
        if (arena == null) throw new ArgumentNullException(nameof(arena));

        return new ArenaSnapshot
        {
            Grids = arena.Grids.Select(g => g.CreateSnapshot()).ToList()
        };
    }

    public static Arena RestoreFromSnapshot(ArenaSnapshot snapshot)
    {
        if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

        var grids = snapshot.Grids
            .Select(GridPersistence.RestoreFromSnapshot)
            .ToArray();

        // Use the standard constructor which sets up reactive subscriptions
        return new Arena(grids);
    }
}
