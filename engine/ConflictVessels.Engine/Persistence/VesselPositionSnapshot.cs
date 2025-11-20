using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;

namespace ConflictVessels.Engine.Persistence;

/// <summary>
/// Snapshot DTO for VesselPosition persistence.
/// Separates serialization concerns from the domain model.
/// </summary>
public record VesselPositionSnapshot
{
    public Coords Coords { get; init; } = null!;
    public VesselOrientation Orientation { get; init; }
}

/// <summary>
/// Extension methods for VesselPosition snapshot creation and restoration.
/// </summary>
public static class VesselPositionPersistence
{
    public static VesselPositionSnapshot CreateSnapshot(this VesselPosition position)
    {
        if (position == null) throw new ArgumentNullException(nameof(position));

        return new VesselPositionSnapshot
        {
            Coords = position.Coords,
            Orientation = position.Orientation
        };
    }

    public static VesselPosition RestoreFromSnapshot(VesselPositionSnapshot snapshot)
    {
        if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

        return new VesselPosition(
            snapshot.Coords.X,
            snapshot.Coords.Y,
            snapshot.Orientation
        );
    }
}
