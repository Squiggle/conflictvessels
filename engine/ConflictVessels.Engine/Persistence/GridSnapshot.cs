using ConflictVessels.Engine.Grids;

namespace ConflictVessels.Engine.Persistence;

/// <summary>
/// Snapshot DTO for Grid persistence.
/// Separates serialization concerns from the domain model.
/// </summary>
public record GridSnapshot
{
    public int Width { get; init; }
    public int Height { get; init; }
    public List<VesselGridPlacementSnapshot> Vessels { get; init; } = new();
}

/// <summary>
/// Extension methods for Grid snapshot creation and restoration.
/// </summary>
public static class GridPersistence
{
    public static GridSnapshot CreateSnapshot(this Grid grid)
    {
        if (grid == null) throw new ArgumentNullException(nameof(grid));

        return new GridSnapshot
        {
            Width = grid.Width,
            Height = grid.Height,
            Vessels = grid.Vessels.Select(v => v.CreateSnapshot()).ToList()
        };
    }

    public static Grid RestoreFromSnapshot(GridSnapshot snapshot)
    {
        if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

        var placements = snapshot.Vessels
            .Select(VesselGridPlacementPersistence.RestoreFromSnapshot)
            .ToList();

        // Use the JSON constructor to bypass the internal constructor
        return new Grid(snapshot.Width, snapshot.Height, placements);
    }
}
